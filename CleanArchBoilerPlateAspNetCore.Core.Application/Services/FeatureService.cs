using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Feature;
using CleanArchBoilerPlateAspNetCore.Core.Application.Exceptions;
using CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FeatureService> _logger;

        private readonly IMapper _mapper;

        public FeatureService(IUnitOfWork unitOfWork, IFeatureRepository featureRepository, IMapper mapper, ILogger<FeatureService> logger)
        {
            _unitOfWork = unitOfWork;
            _featureRepository = featureRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the specified feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FeatureDto> GetByIdAsync(int id)
        {
            Feature entity = await _featureRepository.GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogError("Unable to find the feature with the Id \"{id}\" in the database.", id);
                throw new NotFoundException("Feature", id);
            }

            FeatureDto dto = _mapper.Map<FeatureDto>(entity);

            return dto;
        }

        /// <summary>
        /// Get all the features
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<FeatureDto>> GetAllAsync()
        {
            return await Task.Run(() => _featureRepository
                .GetAll()
                .OrderBy(o => o.Sequence)
                .ProjectTo<FeatureDto>(_mapper.ConfigurationProvider)
                .ToList());
        }

        /// <summary>
        /// Create a new feature
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<FeatureDto> AddAsync(CreateFeatureDto dto, ClaimsPrincipal user)
        {
            //Validate that the new feature name doesn't exist
            List<Feature> existingFeatures = _featureRepository.GetAll().Where(f => f.Name.Equals(dto.Name)).ToList();

            if (existingFeatures.Count > 0)
            {
                _logger.LogError("There is already a feature with the name \"{dto.Name}\".", dto.Name);
                throw new DuplicateValueException("Feature", "Name", dto.Name);
            }

            _logger.LogTrace("Adding a new feature from DTO [{@dto}]", dto);

            //use the automapper
            Feature entityToAdd = _mapper.Map<Feature>(dto);

            //retrieve the actual max sequence value for the features
            int nextSequence = _featureRepository.GetAll().Select(f => f.Sequence).Max().GetValueOrDefault() + 1;

            //set the default values for a new feature
            entityToAdd.Sequence = nextSequence;
            entityToAdd.Status = FeatureStatus.In_Work;
            entityToAdd.CreatedBy = user.Identity.Name;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.LastModifiedBy = user.Identity.Name;
            entityToAdd.LastModifiedOn = DateTime.Now;

            _logger.LogTrace("Adding a new feature from mapped object [{@mappedObject}]", entityToAdd);

            //add the feature
            Feature entity = await _featureRepository.AddAsync(entityToAdd);

            try
            {
                //save the changes in the DB
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO: Send error 500?
            }

            return _mapper.Map<FeatureDto>(entity);
        }

        /// <summary>
        /// Delete a feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteByIdAsync(int id)
        {
            //TODO: we need to validate that the feature is not use by anything before deleting it (feature values, context, rules, etc.)

            _featureRepository.DeleteById(id);

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Update a feature entity with a Json Patch object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<FeatureDto> UpdatePatchAsync(int id, JsonPatchDocument<UpdateFeatureDto> patchDoc, ClaimsPrincipal user)
        {
            //When we update partially, we need to get the actuel entity from the DB, this makes EF to track the modification made to this entity
            Feature entity = await _featureRepository.GetByIdAsync(id);

            //the entity was not found in the DB
            if (entity == null)
            {
                _logger.LogError("Unable to find the feature with the Id \"{id}\" in the database.", id);
                throw new NotFoundException("Feature", id);
            }

            //Convert the entity in the dto, because the Json Patch must be applied on the dto and we want to make some validation before accepting the changes
            UpdateFeatureDto dto = _mapper.Map<UpdateFeatureDto>(entity);

            //Apply the json patch to the object
            patchDoc.ApplyTo(dto);

            //if the entity is archived, we can only changed its status to something else, we cannot update its other properties
            //if the new status is still "Archived", it means another property is being updated and it is not allowed
            if (entity.Status == FeatureStatus.Archived && dto.Status == FeatureStatus.Archived)
            {
                _logger.LogError("Unable to update another property than the status of an archived feature. Trying to update the feature with the Id \"{id}\"", id);
                throw new NotUpdatableException("Feature", id);
            }

            //validate the new status is valid
            if (entity.Status != dto.Status)
            {
                bool success = Enum.IsDefined(typeof(FeatureStatus), dto.Status);
                if (!success)
                {
                    _logger.LogError("The status Id \"{id}\" is not a valid feature status.", id);
                    throw new InvalidValueException("Feature", "Status", dto.Status);
                }
            }

            //if the feature status is updated to "Archived", we need to set the sequence to null
            if (entity.Status != FeatureStatus.Archived && dto.Status == FeatureStatus.Archived)
            {
                dto.Sequence = null;
                SetSequenceToNull(entity.Sequence);
            }
            //if the feature status is updated from "Archived", we need to set the sequence to max sequence + 1
            else if (entity.Status == FeatureStatus.Archived && dto.Status != FeatureStatus.Archived)
            {
                int? maxSequence = _featureRepository.GetAll().Where(f => f.Status != FeatureStatus.Archived).Select(f => f.Sequence).Max().GetValueOrDefault();
                dto.Sequence = maxSequence + 1;
            }
            //if the sequence has changed and the new status is not "Archived", we need to update the other features' sequence accordingly
            else if (entity.Sequence != dto.Sequence && dto.Status != FeatureStatus.Archived)
            {
                dto.Sequence = UpdateSequence(dto.Sequence, entity.Sequence);
            }

            //Update the entity with the updated object
            //We cannot use the mapper, because it creates a new Feature entity with the same Id as the one we got from the DB,
            //therefore it makes EF to generate an error when we attach it, because there are 2 entities with the same Id being tracked.
            //Thus, we need to manually copy the new values in the existing Feature entity (the one we got from "GetByIdAsync(...)".
            entity.Id = dto.Id;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Sequence = dto.Sequence;
            entity.Status = dto.Status;
            entity.Comments = dto.Comments;
            //We never update the "CreatedBy" and "CreatedOn" properties.
            //entity.CreatedBy = dto.CreatedBy;
            //entity.CreatedOn = dto.CreatedOn;
            entity.LastModifiedBy = user.Identity.Name;
            entity.LastModifiedOn = DateTime.Now;

            //As the Feature entity is already being tracked, we only need to save the changes once they are applied to it.
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FeatureDto>(entity);
        }

        /// <summary>
        /// Get all the possible status for a feature
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<FeatureStatusDto>> GetStatus()
        {
            return Task.Run(() => Enum.GetValues(typeof(FeatureStatus))
                .Cast<FeatureStatus>()
                .Select(v => new FeatureStatusDto()
                {
                    Id = (int)v,
                    Name = v.Description()
                }));
        }

        /// <summary>
        /// Update the sequence to keep a logical order
        /// </summary>
        /// <param name="newSequence"></param>
        /// <param name="oldSequence"></param>
        /// <returns>The actuel sequence to set on the object</returns>
        private int? UpdateSequence(int? newSequence, int? oldSequence)
        {
            int? sequenceToSet = newSequence;
            int? maxSequence = _featureRepository.GetAll().Where(f => f.Status != FeatureStatus.Archived).Select(f => f.Sequence).Max().GetValueOrDefault();

            //if the new sequence is lower than 1, we set it to 1
            if (newSequence < 1)
            {
                newSequence = 1;
                sequenceToSet = 1;
            }
            //if the new sequence is higher than the current maximum sequence, we set it to the maximum sequence
            else if (newSequence > maxSequence)
            {
                newSequence = maxSequence;
                sequenceToSet = maxSequence;
            }

            //set lower and upper bound the retrieve the features to update
            int? lowerBound;
            int? upperBound;
            int increaseValue;
            if (newSequence > oldSequence)      //new sequence is higher, we want to decrease the sequence for the features between the new and old sequences
            {
                lowerBound = oldSequence + 1;
                upperBound = newSequence;
                increaseValue = -1;
            }
            else                                //new sequence is lower, we want to increase the sequence for the features between the new and old sequences
            {
                lowerBound = newSequence;
                upperBound = oldSequence - 1;
                increaseValue = 1;
            }

            //get the features that we need to update the sequence
            List<Feature> featuresToUpdate = _featureRepository.GetAll()
                .Where(f => f.Sequence >= lowerBound
                    && f.Sequence <= upperBound
                    && f.Status != FeatureStatus.Archived)
                .ToList();

            //update the sequence of the features
            featuresToUpdate.ForEach(f => f.Sequence = f.Sequence + increaseValue);

            //return the new sequence, because it might have change if the value was too low or too high
            return sequenceToSet;
        }

        /// <summary>
        /// Update the sequence to keep a logical order when a feature's sequence is set to null (when the feature is archived) 
        /// </summary>
        /// <param name="oldSequence"></param>
        private void SetSequenceToNull(int? oldSequence)
        {
            int? maxSequence = _featureRepository.GetAll().Where(f => f.Status != FeatureStatus.Archived).Select(f => f.Sequence).Max().GetValueOrDefault();

            //get the features that we need to update the sequence
            List<Feature> featuresToUpdate = _featureRepository.GetAll()
                .Where(f => f.Sequence >= oldSequence
                    && f.Sequence <= maxSequence
                    && f.Status != FeatureStatus.Archived)
                .ToList();

            //update the sequence of the features, we want to decrease all the sequence of the features with a higher sequence than the one that is set to null
            featuresToUpdate.ForEach(f => f.Sequence = f.Sequence - 1);
        }
    }
}

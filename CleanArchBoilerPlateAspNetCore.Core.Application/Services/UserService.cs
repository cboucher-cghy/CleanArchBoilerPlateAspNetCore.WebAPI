using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Role;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.User;
using CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            User entity = await _userRepository.GetByIdAsync(id);

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await Task.Run(() => _userRepository
                .GetAll()
                .OrderBy(u => u.DisplayName)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToList());
        }

        public async Task<UserDto> AddAsync(UserDto dto, ClaimsPrincipal user)
        {
            User userToAdd = _mapper.Map<User>(dto);

            //set the default values
            //TODO: Needs to call the AD to get the complete name and email of the created user to add it to the DB.
            userToAdd.CreatedBy = user.Identity.Name;
            userToAdd.CreatedOn = DateTime.Now;
            userToAdd.LastModifiedBy = user.Identity.Name;
            userToAdd.LastModifiedOn = DateTime.Now;

            User entity = await _userRepository.AddAsync(userToAdd);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(entity);
        }

        public async Task DeleteByIdAsync(string id)
        {
            _userRepository.DeleteById(id);

            await _unitOfWork.SaveChangesAsync();
        }

        //public async Task<UserDto> UpdatePatchAsync(string id, JsonPatchDocument<UserDto> patchDoc, ClaimsPrincipal user)
        //{
        //    //When we update partially, we need to get the actuel entity from the DB, this makes EF to track the modification made to this entity.
        //    User entity = await _userRepository.GetByIdAsync(id);

        //    UserDto dto = ToDto(entity);

        //    patchDoc.ApplyTo(dto);

        //    //We cannot use the ToEntity() method, because it creates a new Feature entity with the same Id as the one we got from the DB,
        //    //therefore it makes EF to generate an error, because there are 2 entities with the same Id being tracked.
        //    //Thus, we need to copy the new values in the existing Feature entity (the one we got from "GetByIdAsync(...)".
        //    entity.Id = dto.Id;
        //    entity.Id = dto.Id;
        //    entity.DisplayName = dto.DisplayName;
        //    entity.Email = dto.Email;
        //    entity.Departement = dto.Departement;
        //    entity.JobTitle = dto.JobTitle;
        //    //We never update the "CreatedBy" and "CreatedOn" properties.
        //    //entity.CreatedBy = dto.CreatedBy;
        //    //entity.CreatedOn = dto.CreatedOn;
        //    entity.LastModifiedBy = user.Identity.Name;
        //    entity.LastModifiedOn = DateTime.Now;

        //    //As the Feature entity is already being tracked, we only need to save the changes once they are applied to it.
        //    await _unitOfWork.SaveChangesAsync();

        //    return dto;
        //}

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(string id)
        {
            return await Task.Run(() => _roleRepository
                .GetAll()
                .Where(r => r.Users.Any(u => u.Id == id))
                .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                .ToList());
        }

        public async Task<RoleDto> AddRoleToUserAsync(string userId, int roleId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            Role role = await _roleRepository.GetByIdAsync(roleId);

            user.Roles.Add(role);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoleDto>(role);
        }

        public async Task DeleteRoleToUserAsync(string userId, int roleId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            Role role = await _roleRepository.GetByIdAsync(roleId);
        }
    }
}

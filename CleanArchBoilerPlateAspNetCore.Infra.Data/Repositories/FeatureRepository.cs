using CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using CleanArchBoilerPlateAspNetCore.Infra.Data.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Infra.Data.Repositories
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FeatureRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<Feature> GetAll()
        {
            return _dbContext.Set<Feature>();
        }

        public async Task<Feature> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Feature>().FindAsync(id);
        }

        public async Task<Feature> AddAsync(Feature entity)
        {
            return (await _dbContext.Set<Feature>().AddAsync(entity)).Entity;
        }



        public void DeleteById(int id)
        {
            Feature feature = new Feature() { Id = id };
            _dbContext.Features.Attach(feature);
            _dbContext.Features.Remove(feature);
        }

        //TODO: These code is left as an example, it must be removed once there is an implementation in another repository.
        //public void Delete(Feature entity)
        //{
        //    _dbContext.Set<Feature>().Remove(entity);
        //}

        //public void Update(Feature entity)
        //{
        //    //Ca ne fonctionne pas, parce qu'on doit avoir acces a l'entite original pour faire cette methode, ce qui implique de faire une requete supplementaire a la BD.
        //    // SetValues will only mark as modified the properties that have different values to those in the tracked entity.
        //    // This means that when the update is sent, only those columns that have actually changed will be updated. (And if nothing has changed, then no update will be sent at all.)
        //    // Source: https://entityframeworkcore.com/knowledge-base/54169736/what-is-better-way-to-update-data-in-ef-core
        //    //_dbContext.Entry(entity).CurrentValues.SetValues(entity);

        //    _dbContext.Set<Feature>().Update(entity);
        //}
    }
}

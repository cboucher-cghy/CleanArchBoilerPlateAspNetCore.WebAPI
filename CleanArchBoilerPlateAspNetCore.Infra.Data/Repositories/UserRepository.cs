using Microsoft.EntityFrameworkCore;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using CleanArchBoilerPlateAspNetCore.Infra.Data.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<User> GetAll()
        {
            return _dbContext.Set<User>();
        }
        public IQueryable<User> GetAllWithRoles()
        {
            return _dbContext.Set<User>().Include(u => u.Roles);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _dbContext.Set<User>().FindAsync(id);
        }

        public async Task<User> AddAsync(User entity)
        {
            await _dbContext.Set<User>().AddAsync(entity);
            return entity;
        }

        public void DeleteById(string id)
        {
            User entity = new User() { Id = id };
            _dbContext.Set<User>().Attach(entity);
            _dbContext.Set<User>().Remove(entity);
        }

        public void Update(User entity)
        {
            //Ca ne fonctionne pas, parce qu'on doit avoir acces a l'entite original pour faire cette methode, ce qui implique de faire une requete supplementaire a la BD.
            // SetValues will only mark as modified the properties that have different values to those in the tracked entity.
            // This means that when the update is sent, only those columns that have actually changed will be updated. (And if nothing has changed, then no update will be sent at all.)
            // Source: https://entityframeworkcore.com/knowledge-base/54169736/what-is-better-way-to-update-data-in-ef-core
            //_dbContext.Entry(entity).CurrentValues.SetValues(entity);

            _dbContext.Set<User>().Update(entity);
        }
    }
}

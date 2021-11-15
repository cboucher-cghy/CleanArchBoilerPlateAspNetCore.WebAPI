using CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using CleanArchBoilerPlateAspNetCore.Infra.Data.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Infra.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<Role> GetAll()
        {
            return _dbContext.Set<Role>();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Role>().FindAsync(id);
        }
    }
}

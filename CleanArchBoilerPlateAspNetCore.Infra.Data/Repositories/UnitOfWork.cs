using CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces;
using CleanArchBoilerPlateAspNetCore.Infra.Data.DbContexts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Infra.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed;

        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); ;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

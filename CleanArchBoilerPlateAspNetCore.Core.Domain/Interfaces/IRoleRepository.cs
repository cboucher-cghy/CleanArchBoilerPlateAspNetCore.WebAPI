using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetAll();

        Task<Role> GetByIdAsync(int id);
    }
}

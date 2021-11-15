using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();

        IQueryable<User> GetAllWithRoles();

        Task<User> GetByIdAsync(string id);

        Task<User> AddAsync(User entity);

        void Update(User entity);

        void DeleteById(string id);
    }
}

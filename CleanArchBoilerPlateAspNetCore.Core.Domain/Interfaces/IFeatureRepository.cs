using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces
{
    public interface IFeatureRepository
    {
        IQueryable<Feature> GetAll();

        Task<Feature> GetByIdAsync(int id);

        Task<Feature> AddAsync(Feature entity);

        void DeleteById(int id);
    }
}

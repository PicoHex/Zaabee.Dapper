using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zaabee.Dapper.Repository.Abstractions
{
    public interface IZaabeeRepositoryAsync
    {
        Task<int> AddAsync<T>(T persistentObject);
        Task<int> AddRangeAsync<T>(List<T> persistentObjects);
        Task<int> RemoveAsync<T>(T persistentObject);
        Task<int> RemoveAllAsync<T>(List<T> persistentObjects);
        Task<int> RemoveAllAsync<T>(List<object> ids);
        Task<int> RemoveAllAsync<T>();
        Task<int> UpdateAsync<T>(T persistentObject);
        Task<int> UpdateAllAsync<T>(List<T> persistentObjects);
        Task<T> GetAsync<T>(object id);
        Task<List<T>> GetAllAsync<T>(List<object> ids);
        Task<List<T>> GetAllAsync<T>();
    }
}
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Zaabee.Dapper.Repository.Abstractions;

namespace Zaabee.Dapper.Repository
{
    public class ZaabeeRepository : IZaabeeRepository
    {
        private readonly IDbConnection _dbConnection;

        public ZaabeeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AddAsync<T>(T persistentObject)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> AddRangeAsync<T>(List<T> persistentObjects)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> RemoveAsync<T>(T persistentObject)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> RemoveAllAsync<T>(List<T> persistentObjects)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> RemoveAllAsync<T>(List<object> ids)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> RemoveAllAsync<T>()
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> UpdateAsync<T>(T persistentObject)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> UpdateAllAsync<T>(List<T> persistentObjects)
        {
            throw new System.NotImplementedException();
        }

        public async Task<T> GetAsync<T>(object id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync<T>(List<object> ids)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync<T>()
        {
            throw new System.NotImplementedException();
        }

        public int Add<T>(T persistentObject)
        {
            throw new System.NotImplementedException();
        }

        public int AddRange<T>(List<T> persistentObjects)
        {
            throw new System.NotImplementedException();
        }

        public int Remove<T>(T persistentObject)
        {
            throw new System.NotImplementedException();
        }

        public int RemoveAll<T>(List<T> persistentObjects)
        {
            throw new System.NotImplementedException();
        }

        public int RemoveAll<T>(List<object> ids)
        {
            throw new System.NotImplementedException();
        }

        public int RemoveAll<T>()
        {
            throw new System.NotImplementedException();
        }

        public int Update<T>(T persistentObject)
        {
            throw new System.NotImplementedException();
        }

        public int UpdateAll<T>(List<T> persistentObjects)
        {
            throw new System.NotImplementedException();
        }

        public T Get<T>(object id)
        {
            throw new System.NotImplementedException();
        }

        public List<T> GetAll<T>(List<object> ids)
        {
            throw new System.NotImplementedException();
        }

        public List<T> GetAll<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}
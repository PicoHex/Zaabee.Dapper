using System.Collections.Generic;

namespace Zaabee.Dapper.Repository
{
    public interface IRepository
    {
        int Add<T>(T persistentObject);
        int Add<T>(IEnumerable<T> persistentObjects);
        int Delete<T>(T persistentObject);
        int Delete<T>(IEnumerable<T> persistentObject);
        int Update<T>(T persistentObject);
        int Update<T>(IEnumerable<T> persistentObjects);
        T Get<T>(object id);
        List<T> Get<T>(List<object> id);
    }
}
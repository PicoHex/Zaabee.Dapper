using System.Collections.Generic;

namespace Zaabee.Dapper.Repository.Abstractions
{
    public interface IZaabeeRepositorySync
    {
        int Add<T>(T persistentObject);
        int AddRange<T>(List<T> persistentObjects);
        int Remove<T>(T persistentObject);
        int RemoveAll<T>(List<T> persistentObjects);
        int RemoveAll<T>(List<object> ids);
        int RemoveAll<T>();
        int Update<T>(T persistentObject);
        int UpdateAll<T>(List<T> persistentObjects);
        T Get<T>(object id);
        List<T> GetAll<T>(List<object> ids);
        List<T> GetAll<T>();
    }
}
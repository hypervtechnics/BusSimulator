using BusSimulator.Core.Data;

using System.Collections.Generic;

namespace BusSimulator.Core.DataAccess.Interfaces
{
    public interface IDataAccess<T> where T : RepositoryObject
    {
        T GetById(int id);

        List<T> GetAll();

        void Add(T obj);

        void Update(T obj);

        void Delete(T obj);
    }
}

using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IService<T>
    where T : BaseEntity
    {
        void Add(T entity);

        T GetById(int id);

        IList<T> GetAll();

        void Update(T entity);

        void Remove(T entityToRemove);

        void Remove(int id);
    }
}
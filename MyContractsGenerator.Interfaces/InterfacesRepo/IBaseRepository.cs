using System.Linq;

namespace MyContractsGenerator.Interfaces.InterfacesRepo
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        ///     Gets the table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>the entity or null</returns>
        T GetById(int id);

        /// <summary>
        ///     Adds an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        ///     Removes an entity
        /// </summary>
        /// <param name="entityToRemove"></param>
        void Remove(T entityToRemove);

        /// <summary>
        ///     Remove the given id
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);

        /// <summary>
        ///     Saves context changes
        /// </summary>
        void SaveChanges();

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        ///     Creates a new IDalTransactionScope wrapping a new transaction.
        /// </summary>
        IDalTransactionScope BeginTransaction();
    }
}
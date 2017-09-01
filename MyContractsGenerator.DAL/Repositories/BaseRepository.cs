//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Classe de base du Repository
// </summary>
//------------------------------------------------------------------------------
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    /// <summary>
    ///     The base repository
    /// </summary>
    /// <typeparam name="T">Generic type parameter : a domain class</typeparam>
    public class BaseRepository<T> : IBaseRepository<T>
    where T : class
    {
        /// <summary>
        ///     Context for the database
        /// </summary>
        private readonly DbContext DbContext;

        /// <summary>
        ///     Set the database belongs to
        /// </summary>
        private readonly DbSet<T> DbSet;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context">context</param>
        public BaseRepository(DbContext context)
        {
            Requires.ArgumentNotNull(context, "context");

            this.DbContext = context;
            this.DbSet = context.Set<T>();

            this.DbContext.Configuration.LazyLoadingEnabled = false;
            this.DbContext.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        ///     Gets the table
        /// </summary>
        public IQueryable<T> Table
        {
            get { return this.DbSet; }
        }

        /// <summary>
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>the entity or null</returns>
        public virtual T GetById(int id)
        {
            Requires.ArgumentGreaterThanZero(id, "id");

            var entity = this.DbSet.Find(id);
            return entity;
        }

        /// <summary>
        ///     Adds an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Add(T entity)
        {
            Requires.ArgumentNotNull(entity, "entity");
            DbEntityEntry entityEntry = this.DbContext.Entry(entity);
            if (entityEntry.State != EntityState.Detached)
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                entity = this.DbSet.Add(entity);
            }

            return entity;
        }

        public virtual void Update(T entity)
        {
            this.DbSet.Attach(entity);
            this.DbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        ///     Removes an entity
        /// </summary>
        /// <param name="entityToRemove"></param>
        public virtual void Remove(T entityToRemove)
        {
            Requires.ArgumentNotNull(entityToRemove, "entityToRemove");

            DbEntityEntry entityEntry = this.DbContext.Entry(entityToRemove);
            if (entityEntry.State != EntityState.Deleted)
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entityToRemove);
                this.DbSet.Remove(entityToRemove);
            }
        }

        /// <summary>
        ///     Remove the given id
        /// </summary>
        /// <param name="id"></param>
        public virtual void Remove(int id)
        {
            Requires.ArgumentGreaterThanZero(id, "id");
            var entity = this.GetById(id);
            this.Remove(entity);
        }

        /// <summary>
        ///     Saves context changes
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException("Entity Validation Failed - errors follow:\n" + sb, ex);

                // Add the original exception as the innerException
            }
        }

        /// <summary>
        ///     Creates a new IDalTransactionScope wrapping a new transaction.
        /// </summary>
        public IDalTransactionScope BeginTransaction()
        {
            return new DalTransactionScope(this.DbContext.Database.BeginTransaction());
        }
    }
}

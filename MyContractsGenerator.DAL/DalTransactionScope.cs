using System;
using System.Data.Entity;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL
{
    /// <summary>
    ///     Implementation of IDalTransactionScope.
    /// </summary>
    public class DalTransactionScope : IDalTransactionScope
    {
        private bool complete;
        private DbContextTransaction trans;

        public DalTransactionScope(DbContextTransaction trans)
        {
            this.trans = trans;
        }

        /// <summary>
        ///     Sets the current transaction as complete.
        ///     Must be called just before the end of the parent 'using'.
        /// </summary>
        public void Complete()
        {
            this.complete = true;
        }

        /// <summary>
        ///     Ends the current transaction.
        ///     If Complete was called, the transaction is committed; otherwise, the transaction is rollbacked.
        ///     The underlying transaction is then disposed.
        /// </summary>
        public void Dispose()
        {
            if (this.trans != null)
            {
                if (this.complete)
                {
                    this.trans.Commit();
                }
                else
                {
                    this.trans.Rollback();
                }
            }

            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.trans?.Dispose();
                this.trans = null;
            }
        }
    }
}
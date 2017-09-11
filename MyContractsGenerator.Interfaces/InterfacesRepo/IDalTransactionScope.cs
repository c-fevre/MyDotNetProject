﻿using System;

namespace MyContractsGenerator.Interfaces.InterfacesRepo
{
    public interface IDalTransactionScope : IDisposable
    {
        /// <summary>
        ///     Sets the current transaction as complete.
        ///     Must be called just before the end of the parent 'using'.
        /// </summary>
        void Complete();
    }
}
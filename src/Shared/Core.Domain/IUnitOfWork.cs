﻿using System;

namespace Core.Domain
{
    // <summary>
    /// Interface for the IUnitOfWork. Inherits from disposable. Will be used to return the DbContext in it's implementation.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
    }
}

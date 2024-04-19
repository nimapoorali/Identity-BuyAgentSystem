using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsDisposed { get; }
        Task<int> SaveChangesAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Persistence.Repositories.Interface
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}

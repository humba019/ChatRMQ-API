using ChatProducer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Persistence.Repositories.Interface
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> ListAsync();
        Task AddAsync(Message message);
        Task<Message> FindByIdAsync(int id);
        void Update(Message message);
        void Remove(Message message);
    }
}

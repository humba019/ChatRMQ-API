using ChatProducer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Persistence.Repositories.Interface
{
    public interface IChatRepository
    {
        Task<IEnumerable<Chat>> ListAsync();
        Task AddAsync(Chat chat);
        Task<Chat> FindByIdAsync(int id);
        Task<Chat> FindByToAsync(string to);
        Task<Chat> FindByFromAsync(string from);
        void Update(Chat chat);
        void Remove(Chat chat);
    }
}


using ChatProducer.Domain.Models;
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
        Task<List<Message>> FindByIdChatAsync(int id);
        Task<List<Message>> FindByClientEmailAsync(string email);
        Task<List<Message>> FindByClientEmailFromAsync(string email);
        Task<List<Message>> FindByClientEmailToAsync(string email);
        void Update(Message message);
        void Remove(Message message);
    }
}

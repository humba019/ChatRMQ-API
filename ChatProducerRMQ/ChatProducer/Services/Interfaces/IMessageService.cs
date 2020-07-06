
using ChatProducer.Domain.Models;
using ChatProducer.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> ListAsync();
        Task<List<Message>> ListByChatIdAsync(int id);
        Task<List<Message>> ListByClientEmailAsync(string email, string consume);
        Task<uint> CountMessageByClientEmailAsync(string email);
        Task<MessageResponse> SaveAsync(Message message);
        Task<MessageResponse> UpdateAsync(int id, Message message);
        Task<MessageResponse> DeleteAsync(int id);
    }
}

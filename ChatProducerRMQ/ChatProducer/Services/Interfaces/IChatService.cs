
using ChatProducer.Domain.Models;
using ChatProducer.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<Chat>> ListAsync();
        Task<ChatResponse> SaveAsync(Chat chat);
        Task<ChatResponse> UpdateAsync(int id, Chat chat);
        Task<ChatResponse> DeleteAsync(int id);
    }
}

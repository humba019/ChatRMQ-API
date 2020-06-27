
using ChatProducer.Domain.Models;
using ChatProducer.Persistence.Contexts;
using ChatProducer.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Persistence.Repositories
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base (context){  }
        public async Task AddAsync(Message message)
        {
            await _context.Message.AddAsync(message);
        }

        public async Task<Message> FindByIdAsync(int id)
        {
            return await _context.Message.FindAsync(id);
        }

        public async Task<IEnumerable<Message>> ListAsync()
        {
            return await _context.Message.ToListAsync();
        }

        public void Remove(Message message)
        {
            _context.Message.Remove(message);
        }

        public void Update(Message message)
        {
            _context.Message.Update(message);
        }
    }
}

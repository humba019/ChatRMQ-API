
using ChatProducer.Configs;
using ChatProducer.Domain.Models;
using ChatProducer.Persistence.Contexts;
using ChatProducer.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatProducer.Persistence.Repositories
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base (context){  }
        public async Task AddAsync(Message messageObj)
        {
            DateTime time = DateTime.Now;
            messageObj.DateSended = time.ToString("f");
            await _context.Message.AddAsync(messageObj);
        }

        public async Task<Message> FindByIdAsync(int id)
        {
            return await _context.Message.FindAsync(id);
        }

        public async Task<List<Message>> FindByIdChatAsync(int id)
        {
            List<Message> messages = new List<Message>();
            foreach (Message message in await _context.Message.ToListAsync())
            {
                if (message.ChatId.Equals(id))
                {
                    Chat chat = await _context.Chat.FindAsync(message.ChatId);
                    message.Chat = chat;
                    messages.Add(message);
                }
            }

            return messages;
        }
        public async Task<List<Message>> FindByClientEmailAsync(string email)
        {
            List<Message> messages = new List<Message>();
            foreach (Message message in await _context.Message.ToListAsync())
            {
                Chat chat = await _context.Chat.FindAsync(message.ChatId);
                message.Chat = chat;

                if (message.Chat.From.Equals(email) || message.Chat.To.Equals(email))
                {
                    messages.Add(message);
                }
            }

            return messages;
        }
        public async Task<List<Message>> FindByClientEmailFromAsync(string email)
        {
            List<Message> messages = new List<Message>();
            foreach (Message message in await _context.Message.ToListAsync())
            {
                Chat chat = await _context.Chat.FindAsync(message.ChatId);
                message.Chat = chat;

                if (message.Chat.From.Equals(email))
                {
                    messages.Add(message);
                }
            }

            return messages;
        }
        public async Task<List<Message>> FindByClientEmailToAsync(string email)
        {
            List<Message> messages = new List<Message>();
            foreach (Message message in await _context.Message.ToListAsync())
            {
                Chat chat = await _context.Chat.FindAsync(message.ChatId);
                message.Chat = chat;

                if (message.Chat.To.Equals(email))
                {
                    messages.Add(message);
                }
            }

            return messages;
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

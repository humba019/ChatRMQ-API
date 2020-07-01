
using ChatProducer.Domain.Models;
using ChatProducer.Persistence.Contexts;
using ChatProducer.Persistence.Repositories.Interface;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Persistence.Repositories
{
    public class ChatRepository : BaseRepository, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context){ }
        public async Task AddAsync(Chat chat)
        {
            await _context.Chat.AddAsync(chat);
        }

        public async Task<Chat> FindByIdAsync(int id)
        {
            return await _context.Chat.FindAsync(id);
        }

        public async Task<Chat> FindByToAsync(string to)
        {
            Chat toChat = new Chat();
            foreach (Chat chat in await _context.Chat.ToListAsync()) 
            {
                if(chat.To.Equals(to))
                {
                    toChat = chat;
                }
            }

            return await _context.Chat.FindAsync(toChat.ChatId);
        }

        public async Task<Chat> FindByFromAsync(string from)
        {
            Chat fromChat = new Chat();
            foreach (Chat chat in await _context.Chat.ToListAsync())
            {
                if (chat.From.Equals(from))
                {
                    fromChat = chat;
                }
            }

            return await _context.Chat.FindAsync(fromChat.ChatId);
        }

        public async Task<IEnumerable<Chat>> ListAsync()
        {
            return await _context.Chat.ToListAsync();
        }

        public async Task<List<Chat>> FindAllChatsByEmailAsync(string email)
        {
            List<Chat> chats = new List<Chat>();
            foreach (Chat chat in await _context.Chat.ToListAsync())
            {
                if (chat.From.Equals(email))
                {
                    chats.Add(chat);
                }
            }

            return chats;
        }

        public void Remove(Chat chat)
        {
            _context.Chat.Remove(chat);
        }

        public void Update(Chat chat)
        {
            _context.Chat.Update(chat);
        }
    }
}

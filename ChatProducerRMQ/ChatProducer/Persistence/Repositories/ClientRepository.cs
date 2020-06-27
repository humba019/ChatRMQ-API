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
    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository(AppDbContext context) : base( context ){}
        public async Task AddAsync(Client client)
        {
            await _context.Client.AddAsync(client);
        }

        public async Task<Client> FindByIdAsync(string email)
        {
            return await _context.Client.FindAsync(email);
        }

        public async Task<IEnumerable<Client>> ListAsync()
        {
            return await _context.Client.ToListAsync();
        }

        public void Remove(Client client)
        {
            _context.Client.Remove(client);
        }

        public void Update(Client client)
        {
            _context.Client.Update(client);
        }
    }
}

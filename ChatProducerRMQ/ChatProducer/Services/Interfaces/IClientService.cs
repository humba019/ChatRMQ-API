﻿
using ChatProducer.Domain.Models;
using ChatProducer.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> ListAsync();
        Task<List<Client>> FindAllClientsDiffByEmailAsync(string email);
        Task<List<ClientChat>> FindAllChatsByEmailAsync(string email);
        Task<ClientResponse> SaveAsync(Client client);
        Task<ClientResponse> UpdateAsync(string email, Client client);
        Task<ClientResponse> DeleteAsync(string email);
        Task<ClientResponse> FindByEmailAsync(string email);
    }
}

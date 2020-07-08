
using ChatProducer.Domain.Models;
using ChatProducer.Persistence.Repositories.Interface;
using ChatProducer.Services.Communication;
using ChatProducer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services
{
    public class ClientService : IClientService
    {
        public readonly IClientRepository _clientRepository;
        public readonly IChatRepository _chatRepository;
        public readonly IUnitOfWork _unitOfWork;
        public ClientService(IClientRepository clientRepository, IChatRepository chatRepository, IUnitOfWork unitOfWork)
        {
            this._clientRepository = clientRepository;
            this._chatRepository = chatRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ClientResponse> DeleteAsync(string email)
        {
            var exist = await _clientRepository.FindByIdAsync(email);

            if (exist == null)
            {
                return new ClientResponse("Client not found");
            }

            try
            {
                _clientRepository.Remove(exist);
                await _unitOfWork.CompleteAsync();

                return new ClientResponse(exist);
            }
            catch (Exception e)
            {
                return new ClientResponse($"An error occurred when deleting the client: { e.Message }");
            }
        }

        public async Task<List<ClientChat>> FindAllChatsByEmailAsync(string email)
        {
            List<ClientChat> clients = new List<ClientChat>();
            foreach (Chat chat in await _chatRepository.FindAllChatsByEmailAsync(email)) 
            {
                if (!chat.To.Equals(email))
                {
                    Client client = await _clientRepository.FindByIdAsync(chat.To);
                    ClientChat clientChat = new ClientChat();
                    clientChat.ClientName = client.ClientName;
                    clientChat.ClientEmail = client.ClientEmail;
                    clientChat.Chat = chat;
                    clients.Add(clientChat);
                }
            }
            return clients;
        }

        public async Task<List<Client>> FindAllClientsDiffByEmailAsync(string email)
        {
            List<Client> clients = new List<Client>();
            foreach (Client client in await _clientRepository.ListAsync())
            {
                if (!client.ClientEmail.Equals(email))
                {
                    clients.Add(client);
                }
            }

            return clients;
        }

        public async Task<ClientResponse> FindByEmailAsync(string email)
        {
            var exist = await _clientRepository.FindByIdAsync(email);
            if (exist == null)
            {
                return new ClientResponse("Client not found");
            }

            return new ClientResponse(exist);
        }

        public async Task<IEnumerable<Client>> ListAsync()
        {
            return await _clientRepository.ListAsync();
        }

        public async Task<ClientResponse> SaveAsync(Client client)
        {
            try
            {
                Client clientIn = await _clientRepository.FindByIdAsync(client.ClientEmail);
                await _clientRepository.AddAsync(client);
                await _unitOfWork.CompleteAsync();

                return new ClientResponse(client);
            }
            catch (Exception e)
            {
                return new ClientResponse($"An error occurred when saving the client: {e.Message}");
            }
        }

        public async Task<ClientResponse> UpdateAsync(string email, Client client)
        {
            var exist = await _clientRepository.FindByIdAsync(email);
            if (exist == null)
                return new ClientResponse("Client not found");

            exist.ClientName = client.ClientName;
            exist.ClientEmail = client.ClientEmail;

            try
            {
                _clientRepository.Update(exist);
                await _unitOfWork.CompleteAsync();

                return new ClientResponse(exist);
            }
            catch (Exception e)
            {
                return new ClientResponse($"An error occurred when updating the client: { e.Message }");
            }
        }
    }
}

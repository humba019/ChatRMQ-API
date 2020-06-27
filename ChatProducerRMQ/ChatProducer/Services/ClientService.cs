
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
        public readonly IUnitOfWork _unitOfWork;
        public ClientService(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            this._clientRepository = clientRepository;
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

        public async Task<IEnumerable<Client>> ListAsync()
        {
            return await _clientRepository.ListAsync();
        }

        public async Task<ClientResponse> SaveAsync(Client client)
        {
            try
            {
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

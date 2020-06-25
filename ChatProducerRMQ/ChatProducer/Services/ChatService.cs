using ChatProducer.Domain;
using ChatProducer.Persistence.Repositories.Interface;
using ChatProducer.Services.Communication;
using ChatProducer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services
{
    public class ChatService : IChatService
    {
        public readonly IChatRepository _chatRepository;
        public readonly IClientRepository _clientRepository;
        public readonly IUnitOfWork _unitOfWork;
        public ChatService(IChatRepository chatRepository, IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            this._chatRepository = chatRepository;
            this._clientRepository = clientRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ChatResponse> DeleteAsync(int id)
        {
            var exist = await _chatRepository.FindByIdAsync(id);

            if (exist == null)
            {
                return new ChatResponse("Chat not found");
            }

            try
            {
                _chatRepository.Remove(exist);
                await _unitOfWork.CompleteAsync();

                return new ChatResponse(exist);
            }
            catch (Exception e)
            {
                return new ChatResponse($"An error occurred when deleting the chat: { e.Message }");
            }
        }

        public async Task<IEnumerable<Chat>> ListAsync()
        {
            return await _chatRepository.ListAsync();
        }

        public async Task<ChatResponse> SaveAsync(Chat chat)
        {
            try
            {
                var searchTo = await _clientRepository.FindByIdAsync(chat.To);
                if (searchTo == null)
                    return new ChatResponse($"(to)Client {chat.To} not found"); 

                var searchFrom = await _clientRepository.FindByIdAsync(chat.From);
                if (searchFrom == null)
                    return new ChatResponse($"(from)Client {chat.From} not found"); 

                var searchFromChat = await _chatRepository.FindByFromAsync(chat.From);
                if (searchFromChat != null)
                    return new ChatResponse($"(from)Change {chat.From} to insert ");

                var searchToChat = await _chatRepository.FindByToAsync(chat.To);
                if (searchToChat != null)
                    return new ChatResponse($"(to)Change {chat.To} to insert ");

                await _chatRepository.AddAsync(chat);
                await _unitOfWork.CompleteAsync();

                return new ChatResponse(chat);
            }
            catch (Exception e)
            {
                return new ChatResponse($"An error occurred when saving the chat: {e.Message}");
            }
        }

        public async Task<ChatResponse> UpdateAsync(int id, Chat chat)
        {
            var exist = await _chatRepository.FindByIdAsync(id);
            if (exist == null)
                return new ChatResponse("Chat not found");

            exist.To = chat.To;
            exist.From = chat.From;

            try
            {
                _chatRepository.Update(exist);
                await _unitOfWork.CompleteAsync();

                return new ChatResponse(exist);
            }
            catch (Exception e)
            {
                return new ChatResponse($"An error occurred when updating the chat: { e.Message }");
            }
        }
    }
}

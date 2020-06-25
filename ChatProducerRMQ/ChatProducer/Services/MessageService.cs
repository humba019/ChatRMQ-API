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
    public class MessageService : IMessageService
    {
        public readonly IMessageRepository _messageRepository;
        public readonly IChatRepository _chatRepository;
        public readonly IUnitOfWork _unitOfWork;
        public MessageService(IMessageRepository messageRepository, IChatRepository chatRepository, IUnitOfWork unitOfWork)
        {
            this._messageRepository = messageRepository;
            this._chatRepository = chatRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<MessageResponse> DeleteAsync(int id)
        {
            var exist = await _messageRepository.FindByIdAsync(id);

            if (exist == null)
            {
                return new MessageResponse("Message not found");
            }

            try
            {
                _messageRepository.Remove(exist);
                await _unitOfWork.CompleteAsync();

                return new MessageResponse(exist);
            }
            catch (Exception e)
            {
                return new MessageResponse($"An error occurred when deleting the message: { e.Message }");
            }
        }

        public async Task<IEnumerable<Message>> ListAsync()
        {
            return await _messageRepository.ListAsync();
        }

        public async Task<MessageResponse> SaveAsync(Message message)
        {
            try
            {
                var search = await _chatRepository.FindByIdAsync(message.ChatId);
                if (search == null){ return new MessageResponse($"Chat {message.ChatId} not found"); }

                await _messageRepository.AddAsync(message);
                await _unitOfWork.CompleteAsync();

                return new MessageResponse(message);
            }
            catch (Exception e)
            {
                return new MessageResponse($"An error occurred when saving the message: {e.Message}");
            }
        }

        public async Task<MessageResponse> UpdateAsync(int id, Message message)
        {
            var exist = await _messageRepository.FindByIdAsync(id);
            if (exist == null)
                return new MessageResponse("Message not found");

            exist.MessageContent = message.MessageContent;
            exist.ChatId = message.ChatId;

            try
            {
                _messageRepository.Update(exist);
                await _unitOfWork.CompleteAsync();

                return new MessageResponse(exist);
            }
            catch (Exception e)
            {
                return new MessageResponse($"An error occurred when updating the message: { e.Message }");
            }
        }
    }
}


using ChatProducer.Domain.Models;
using ChatProducer.Persistence.Repositories.Interface;
using ChatProducer.Services.Communication;
using ChatProducer.Services.Interfaces;
using ChatProducer.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public async Task<List<Message>> ListByChatIdAsync(int id)
        {
            var connection = EnumExtensions.FactoryConfig.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: $"message_C{id}",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var messageObj = JsonSerializer.Deserialize<Message>(message);
                };

                channel.BasicConsume(queue: $"message_C{id}",
                                     autoAck: true,
                                     consumer: consumer);
            }

            return await _messageRepository.FindByIdChatAsync(id);
        }

        public async Task<List<Message>> ListByClientEmailAsync(string email, string consume)
        {
            List<Message> messages = await _messageRepository.FindByClientEmailAsync(email);

            if (consume != "false") 
            {
                foreach (Message message in messages)
                {
                    if (message.Chat.To == email)
                    {
                        var connection = EnumExtensions.FactoryConfig.CreateConnection();
                        using (var channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: $"message_C{message.Chat.ChatId}",
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);

                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                var messageObj = JsonSerializer.Deserialize<Message>(message);
                            };

                            channel.BasicConsume(queue: $"message_C{message.Chat.ChatId}",
                                                 autoAck: true,
                                                 consumer: consumer);
                        }
                    }
                }
            }

            return messages;
        }
        public async Task<uint> CountMessageByClientEmailAsync(string email)
        {
            uint ucount = 0;
            foreach (Message message in await _messageRepository.FindByClientEmailFromAsync(email))
            {
                var connection = EnumExtensions.FactoryConfig.CreateConnection();
                using (var channel = connection.CreateModel())
                {
                    QueueDeclareOk result = channel.QueueDeclare(queue: $"message_C{message.Chat.ChatId}",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    ucount = result.MessageCount;

                }

            }

            return ucount;
        }

        public async Task<MessageResponse> SaveAsync(Message messageObj)
        {
            try
            {
                var search = await _chatRepository.FindByIdAsync(messageObj.ChatId);
                if (search == null){ return new MessageResponse($"Chat {messageObj.ChatId} not found"); }

                var connection = EnumExtensions.FactoryConfig.CreateConnection();
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: $"message_C{messageObj.ChatId}",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                    string message = JsonSerializer.Serialize(messageObj);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                            routingKey: $"message_C{messageObj.ChatId}",
                                            basicProperties: null,
                                            body: body);
                }
                await _messageRepository.AddAsync(messageObj);
                await _unitOfWork.CompleteAsync();

                return new MessageResponse(messageObj);
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

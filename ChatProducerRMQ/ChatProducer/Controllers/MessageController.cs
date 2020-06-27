
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using ChatProducer.Domain.Models;
using ChatProducer.Extensions;
using ChatProducer.Resources;
using ChatProducer.Resources.Entity;
using ChatProducer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ChatProducer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/message")]
    public class MessageController : Controller
    {
        private ILogger<ClientController> _logger;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessageController(
            ILogger<ClientController> logger, 
            IMessageService messageService, 
            IMapper mapper)
        {
            _logger = logger;
            _messageService = messageService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IEnumerable<MessageResource>> GetAllAsync()
        {
            var messages = await _messageService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Message>, IEnumerable<MessageResource>>(messages);

            return resources;
        }

        [HttpPost]
        public async Task<ActionResult<MessageResource>> PostMessageAsync([FromBody] SaveMessageResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var messageObj = _mapper.Map<SaveMessageResource, Message>(resource);
            var result = await _messageService.SaveAsync(messageObj);

            if (!result.Success)
                return BadRequest(result.Message);

            var messageResource = _mapper.Map<Message, MessageResource>(result.MessageObj);
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: $"message_C{messageResource.Chat.ChatId}",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = JsonSerializer.Serialize(messageResource);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: $"message_C{messageResource.Chat.ChatId}",
                                         basicProperties: null,
                                         body: body);
                }


                return Ok(messageResource);

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro no POST para api/client ", ex);

                return BadRequest(result.Message);
            }

        }
        
    }
}

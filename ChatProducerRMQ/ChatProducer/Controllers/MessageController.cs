
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using ChatProducer.Configs;
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

        [HttpGet("find/{id}")]
        public async Task<IEnumerable<MessageResource>> GetAllByChatIdAsync(int id)
        {
            var messages = await _messageService.ListByChatIdAsync(id);
            var resources = _mapper.Map<List<Message>, List<MessageResource>>(messages);

            return resources;
        }

        [HttpGet("find-message/{email}/{consume}")]
        public async Task<IEnumerable<MessageResource>> GetAllByClientEmailAsync(string email, string consume)
        {
            var messages = await _messageService.ListByClientEmailAsync(email, consume);
            var resources = _mapper.Map<List<Message>, List<MessageResource>>(messages);

            return resources;
        }

        [HttpGet("count-message/{email}")]
        public async Task<ActionResult<uint>> CountByClientEmailAsync(string email)
        {
            uint messages = await _messageService.CountMessageByClientEmailAsync(email);

            return Ok(messages);
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
            {
                return BadRequest(result.Message);
            }

            var messageResource = _mapper.Map<Message, MessageResource>(result.MessageObj);

            return Ok(messageResource);

        }
        
    }
}

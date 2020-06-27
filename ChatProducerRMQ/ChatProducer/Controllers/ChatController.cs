
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ChatProducer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IEnumerable<ChatResource>> GetAllAsync()
        {
            var chats = await _chatService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Chat>, IEnumerable<ChatResource>>(chats);

            return resources;
        }

        [HttpPost]
        public async Task<ActionResult<ChatResource>> PostChatAsync([FromBody] SaveChatResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            
            var chat = _mapper.Map<SaveChatResource, Chat>(resource);
            var result = await _chatService.SaveAsync(chat);

            if (!result.Success)
                return BadRequest(result.Message);

            var chatResource = _mapper.Map<Chat, ChatResource>(result.Chat);

            return Ok(chatResource);

        }
    }
}

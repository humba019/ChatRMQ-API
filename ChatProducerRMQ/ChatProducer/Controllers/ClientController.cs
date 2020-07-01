
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
using ChatProducer.Services.Communication;
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
    [Route("api/client")]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ClientResource>> GetAllAsync()
        {
            var clients = await _clientService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientResource>>(clients);

            return resources;
        }

        [HttpGet("find/{email}")]
        public async Task<ActionResult<ClientResource>> GetByEmailAsync(string email)
        {
            var result = await _clientService.FindByEmailAsync(email);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var clientResource = _mapper.Map<Client, ClientResource>(result.Client);

            return Ok(clientResource);
        }
        [HttpGet("find-chat/{email}")]
        public async Task<List<ClientChat>> GetChatsByEmailAsync(string email)
        {
            var clients = await _clientService.FindAllChatsByEmailAsync(email);

            return clients;
        }

        [HttpPost]
        public async Task<ActionResult<ClientResource>> PostClientAsync([FromBody] SaveClientResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var client = _mapper.Map<SaveClientResource, Client>(resource);
            var result = await _clientService.SaveAsync(client);

            
            if (!result.Success)
                return BadRequest(result.Message);

            var clientResource = _mapper.Map<Client, ClientResource>(result.Client);

            return Ok(clientResource);

        }

    }
}

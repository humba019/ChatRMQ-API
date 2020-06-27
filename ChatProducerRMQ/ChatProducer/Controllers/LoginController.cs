
using AutoMapper;
using ChatProducer.Domain.Models;
using ChatProducer.Extensions;
using ChatProducer.Resources;
using ChatProducer.Resources.Entity;
using ChatProducer.Services;
using ChatProducer.Services.Communication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private IConfiguration _config;
        private readonly IMapper _mapper;
        
        public LoginController(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<TokenResource>> PostLoginToken([FromBody] SaveTokenResource resource)
        {
            var jwt = new JwtService(_config);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            //
            var result = await jwt.GenerateSecurityToken(resource);

            if (!result.Success)
                return BadRequest(result.Message);

            var tokenResource = _mapper.Map<Token, TokenResource>(result.Token);

            return Ok(tokenResource);

        }
    }
}


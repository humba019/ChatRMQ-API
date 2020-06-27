using ChatProducer.Domain.Models;
using ChatProducer.Extensions;
using ChatProducer.Resources;
using ChatProducer.Resources.Entity;
using ChatProducer.Services.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatProducer.Services
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly string _expDate;

        public JwtService(IConfiguration config)
        {
            _secret = config.GetSection("JwtConfig").GetSection("secret").Value;
            _expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
        }

        public async Task<TokenResponse> GenerateSecurityToken(SaveTokenResource client)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, client.ClientEmail)
                    })
                ,
                    Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                Token tokenObj = new Token();
                tokenObj.ClientEmail = client.ClientEmail;
                tokenObj.ClientToken = tokenHandler.WriteToken(token);

                return new TokenResponse(tokenObj);
            }
            catch (Exception e)
            {
                return new TokenResponse($"An error occurred when generation token to {client.ClientEmail}: {e.Message}");
            }

        }
    }
}

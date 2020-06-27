using ChatProducer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services.Communication
{
    public class TokenResponse : BaseResponse
    {
        public Token Token { get; private set; }
        public TokenResponse(bool success, string message, Token token) : base(success, message)
        {
            Token = token;
        }
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="client">Saved advertencia.</param>
        /// <returns>Response.</returns>
        public TokenResponse(Token token) : this(true, string.Empty, token)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public TokenResponse(string message) : this(false, message, null)
        { }
    }
}

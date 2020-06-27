
using ChatProducer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services.Communication
{
    public class ClientResponse : BaseResponse
    {
        public Client Client { get; private set; }
        public ClientResponse(bool success, string message, Client client) : base(success, message)
        {
            Client = client;
        }
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="client">Saved advertencia.</param>
        /// <returns>Response.</returns>
        public ClientResponse(Client client) : this(true, string.Empty, client)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ClientResponse(string message) : this(false, message, null)
        { }
    }
}

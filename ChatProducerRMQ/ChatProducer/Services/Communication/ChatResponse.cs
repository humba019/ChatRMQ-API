using ChatProducer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services.Communication
{
    public class ChatResponse : BaseResponse
    {
        public Chat Chat { get; private set; }
        public ChatResponse(bool success, string message, Chat chat) : base(success, message)
        {
            Chat = chat;
        }
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="chat">Saved advertencia.</param>
        /// <returns>Response.</returns>
        public ChatResponse(Chat chat) : this(true, string.Empty, chat)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ChatResponse(string message) : this(false, message, null)
        { }
    }
}

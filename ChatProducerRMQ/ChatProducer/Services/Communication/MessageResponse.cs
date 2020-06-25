using ChatProducer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Services.Communication
{
    public class MessageResponse : BaseResponse
    {
        public Message MessageObj { get; private set; }
        public MessageResponse(bool success, string message, Message messageobj) : base(success, message)
        {
            MessageObj = messageobj;
        }
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="message">Saved advertencia.</param>
        /// <returns>Response.</returns>
        public MessageResponse(Message messageobj) : this(true, string.Empty, messageobj)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="messageobj">Error message.</param>
        /// <returns>Response.</returns>
        public MessageResponse(string message) : this(false, message, null)
        { }
    }
}

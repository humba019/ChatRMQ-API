using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatProducer.Configs
{
    public class RabbitConfig
    {
        protected readonly ConnectionFactory _rmq;

        public RabbitConfig()
        {
            _rmq = new ConnectionFactory() { HostName = "localhost" }; 
        }
    }
}

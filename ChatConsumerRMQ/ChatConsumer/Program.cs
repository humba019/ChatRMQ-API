using ChatConsumer.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ChatConsumer
{
    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "message_C1",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueDeclare(queue: "message_C2",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var messageObj = JsonSerializer.Deserialize<Message>(message);
                      
                    Console.WriteLine($"From: {messageObj.Chat.From}\nTo: {messageObj.Chat.To}\nMessage: {messageObj.MessageId} | {messageObj.MessageContent} ");
                };
                channel.BasicConsume(queue: "message_C1",
                                     autoAck: true,
                                     consumer: consumer);
                channel.BasicConsume(queue: "message_C2",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}

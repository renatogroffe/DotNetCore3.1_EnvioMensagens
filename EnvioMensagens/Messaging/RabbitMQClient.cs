using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog.Core;

namespace EnvioMensagens.Messaging
{
    public class RabbitMQClient : IClientMessageBroker
    {
        private IConfiguration _configuration;
        private Logger _logger;
        private string _destination;

        public void Initialize(IConfiguration configuration, Logger logger, string destination)
        {
            _configuration = configuration;
            _logger = logger;
            _destination = destination;
        }

        public void SendMessages(string[] messages)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_configuration.GetConnectionString("RabbitMQ"))
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _destination,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                foreach (var message in messages)
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                         routingKey: _destination,
                                         basicProperties: null,
                                         body: body);

                    _logger.Information($"RabbitMQ - " +
                        $"Queue: {_destination} - Mensagem enviada: {message}");
                }
            }
        }
    }
}
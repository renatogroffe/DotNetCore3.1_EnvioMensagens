using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using Serilog.Core;

namespace EnvioMensagens.Messaging
{
    public class KafkaTopicClient : IClientMessageBroker
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
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration.GetConnectionString("ApacheKafka")
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                foreach (var message in messages)
                {
                    var result = producer.ProduceAsync(_destination,
                        new Message<Null, string>
                        { Value = message }).Result;

                    _logger.Information($"Apache Kafka - " +
                        $"Topic: {_destination} - Mensagem enviada: {message} - " +
                        $"Status: {result.Status.ToString()}");
                }
            }
        }
    }
}
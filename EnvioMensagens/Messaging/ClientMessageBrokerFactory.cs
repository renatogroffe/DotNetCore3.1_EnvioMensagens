using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog.Core;

namespace EnvioMensagens.Messaging
{
    public static class ClientMessageBrokerFactory
    {
        public static IClientMessageBroker CreateClient(
            Logger logger, string tecnologia, string destination)
        {
            IClientMessageBroker client = null;
            tecnologia = tecnologia.Trim().ToLower();
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json").Build();

            switch (tecnologia)
            {
                case "queuestorage":
                    client = new AzureQueueStorageClient();
                    break;
                case "rabbitmq":
                    client = new RabbitMQClient();
                    break;
                case "servicebus-queue":
                    client = new AzureServiceBusQueueClient();
                    break;
                case "servicebus-topic":
                    client = new AzureServiceBusTopicClient();
                    break;
                case "kafka":
                    client = new KafkaTopicClient();
                    break;
                default:
                    logger.Error("Tecnologia inválida!");
                    break;
            }

            if (client != null)
                client.Initialize(config, logger, destination);

            return client;
        }
    }
}
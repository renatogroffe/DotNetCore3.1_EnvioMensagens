using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Configuration;
using Serilog.Core;

namespace EnvioMensagens.Messaging
{
    public class AzureQueueStorageClient : IClientMessageBroker
    {
        private IConfiguration _configuration;
        private Logger _logger;
        private string _destination;

        public void Initialize (IConfiguration configuration, Logger logger, string destination)
        {
            _configuration = configuration;
            _logger = logger;
            _destination = destination;
        }

        public void SendMessages(string[] messages)
        {
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(_configuration.GetConnectionString("AzureStorage"));
            CloudQueueClient queueClient =
                storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(_destination);

            if (queue.CreateIfNotExists())
                _logger.Information($"Criada a Queue {_destination} no Azure Storage");
            
            foreach (var message in messages)
            {
                queue.AddMessage(new CloudQueueMessage(message));
                _logger.Information($"Azure Queue Storage - " +
                    $"Queue: {_destination} - Mensagem enviada: {message}");
            }               
        }
    }
}
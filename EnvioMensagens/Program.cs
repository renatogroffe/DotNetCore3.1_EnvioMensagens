using System;
using Serilog;
using EnvioMensagens.Messaging;

namespace EnvioMensagens
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            logger.Information("Testes com mensageria...");

            if (args.Length < 3)
            {
                logger.Error(
                    "Informe ao menos 3 parâmetros: " +
                    "no primeiro a tecnologia " +
                    "(queuestorage, rabbitmq, servicebus-queue, servicebus-topic, kakfa), " +
                    "no segundo a Queue ou Topic que receberá a mensagem, " +
                    "já no terceito em diante as mensagens a serem enviadas!");
                return;
            }

            if (String.IsNullOrWhiteSpace(args[1]))
            {
                logger.Error(
                    "Informe a Queue ou Topic que receberá a mensagem!");
                return;
            }

            var client = ClientMessageBrokerFactory.CreateClient(
                logger, args[0], args[1]);
            if (client != null)
            {
                client.SendMessages(args[2..]);
                logger.Information("Envio de mensagens concluído!");
            }
        }
    }
}
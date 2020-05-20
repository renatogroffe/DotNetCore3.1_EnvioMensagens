using Microsoft.Extensions.Configuration;
using Serilog.Core;

namespace EnvioMensagens.Messaging
{
    public interface IClientMessageBroker
    {
        void Initialize (IConfiguration configuration, Logger logger, string destination);
        void SendMessages(string[] messages);
    }
}
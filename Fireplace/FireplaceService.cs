using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Fireplace
{
    public class FireplaceService : IDisposable
    {
        TcpClient client = new TcpClient();
        private NetworkStream networkStream;
        const string on = "0233303330333033303830314103";
        const string off = "0233303330333033303830313003";
        private readonly IPAddress fireplaceIP;
        private readonly ILogger<FireplaceService> logger;
        byte[] onBytes = StringToByteArray(on);
        byte[] offBytes = StringToByteArray(off);
        public FireplaceService(IConfiguration config, ILogger<FireplaceService> logger)
        {
            fireplaceIP = IPAddress.Parse(config.GetValue<string>("FireplaceIP"));
            this.logger = logger;
            logger.LogInformation("Connecting to Fireplace");
            client.Connect(fireplaceIP, 2000);
            networkStream = client.GetStream(); 
        }

        public void TurnOn()
        {
            logger.LogInformation("Turning Fireplace On");
            writeToSocket(onBytes);
        }

        public void TurnOff()
        {
            logger.LogInformation("Turning Fireplace Off");
            writeToSocket(offBytes);
        }

        private void writeToSocket(byte[] data)
        {
            for (int i = 0; i < 5; i++)
            {
                networkStream.Write(data, 0, data.Length);
                Thread.Sleep(200);
            }
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public void Dispose()
        {
            client?.Close();
            ((IDisposable)networkStream).Dispose();
            ((IDisposable)client).Dispose();
            logger.LogInformation("Disposed Fireplace Service");
        }
    }
}

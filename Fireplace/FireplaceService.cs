using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Fireplace
{
    public class FireplaceService
    {
        TcpClient client;
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
            connect();
            this.logger = logger;
        }

        private void connect()
        {
            logger.LogInformation("Connecting to Fireplace");
            client?.Close();
            client = new TcpClient();
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
            if (!client.Connected)
                connect();

            try
            {
                networkStream.Write(data, 0, data.Length);
            }
            catch (System.IO.IOException)
            {
                logger.LogWarning("Unable to communicate to Fireplace");
                writeToSocket(data);
            }
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}

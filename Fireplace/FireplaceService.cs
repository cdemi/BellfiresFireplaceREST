using Microsoft.Extensions.Configuration;
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
        TcpClient client = new TcpClient();
        private NetworkStream networkStream;
        const string on = "0233303330333033303830314103";
        const string off = "0233303330333033303830313003";
        private readonly IPAddress fireplaceIP;
        byte[] onBytes = StringToByteArray(on);
        byte[] offBytes = StringToByteArray(off);
        public FireplaceService(IConfiguration config)
        {
            fireplaceIP = IPAddress.Parse(config.GetValue<string>("FireplaceIP"));
            connect();
        }

        private void connect()
        {
            client.Connect(fireplaceIP, 2000);
            networkStream = client.GetStream();
        }

        public void TurnOn()
        {
            if (!client.Connected)
                connect();
            networkStream.Write(onBytes, 0, onBytes.Length);
        }

        public void TurnOff()
        {
            if (!client.Connected)
                connect();
            networkStream.Write(offBytes, 0, offBytes.Length);
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

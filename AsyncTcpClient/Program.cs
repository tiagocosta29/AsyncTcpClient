using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("<server> <message> [<port>]");
                return;
            }

            string server = args[0];
            string message = args[1];
            int port = args.Length == 3 ? int.Parse(args[2]) : 7;

            StartClient(server, message, port);
            Console.ReadLine();
        }

        private async static void StartClient(string address, string message, int port)
        {
            TcpClient client = new TcpClient();

            try
            {
                await client.ConnectAsync(address, port);
                Console.WriteLine("Connected to server");

                using (var netStream = client.GetStream())
                {
                    using (var writer = new StreamWriter(netStream))
                    {
                        using (var reader = new StreamReader(netStream))
                        {
                            writer.AutoFlush = true;
                            await writer.WriteLineAsync(message);
                            var dataFromSerer = await reader.ReadLineAsync();
                            if (!string.IsNullOrEmpty(dataFromSerer))
                            {
                                Console.WriteLine(dataFromSerer);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred");
            }
            finally
            {
              //  client.Close();
            }
        }
    }
}

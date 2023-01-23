using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsccProxy
{
    public static class Worker
    {
        public static async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TcpListener server = null;
            try
            {
                int port = 6123;
                IPAddress ipAddress = IPAddress.Parse("217.159.185.135");

                server = new TcpListener(ipAddress, port);
                server.Start();

                byte[] bytes = new byte[256];
                string data = null;

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    using TcpClient client = await server.AcceptTcpClientAsync(stoppingToken);
                    Console.WriteLine("Connected!");

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;
                    while ((i = await stream.ReadAsync(bytes, stoppingToken)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        string response = String.Empty;
                        try
                        {
                            response = await GetFsccResponseAsync(data);
                        }
                        catch (UriFormatException ex)
                        {
							// for some reason I actually get the response and right after
							// a UriFormatException for some reason, so lets just ignore it
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            byte[] msg = Encoding.ASCII.GetBytes(response);
                            stream.Write(msg, 0, msg.Length);
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task<string> GetFsccResponseAsync(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

			Console.WriteLine(url);

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Method = "GET";
            request.ContentType = "text/xml; encoding='utf-8'";

            response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
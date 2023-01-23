using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsccProxy
{
    public static class Worker
    {
        //https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-7.0
        public static async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TcpListener server = null;
            try
            {
                int port = 6123;
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

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
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        data = data.ToUpper();

                        //var fsccResponse = await GetFsccResponseAsync(data);
                        var fsccResponse = "Pong";
                        byte[] msg = Encoding.ASCII.GetBytes(fsccResponse);

                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
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
            using HttpClient httpClient = new();
            var msg = new HttpRequestMessage(HttpMethod.Get, url);
            msg.Headers.Add("ContentType", "text/xml; encoding='utf-8'");
            var res = await httpClient.SendAsync(msg);
            return await res.Content.ReadAsStringAsync();


            HttpWebRequest request = null;
            HttpWebResponse response = null;
            request = (HttpWebRequest)HttpWebRequest.Create(url);
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
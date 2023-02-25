using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(8080);
            
            TcpClient client = null;
            serverSocket.Start();

            while (true)
            {
                client = serverSocket.AcceptTcpClient();
                ClientHandler handler = new ClientHandler(client);
            }

            client.Close();
            serverSocket.Stop();
        }
    }

    class ClientHandler
    {
        private readonly TcpClient _client;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            Thread thread = new Thread(Proceed);
            thread.Start();
        }

        private void Proceed()
        {
            StreamWriter writer = new StreamWriter(_client.GetStream());
            StreamReader reader = new StreamReader(_client.GetStream());
            AcceptClient(reader, writer);
            writer.Close();
            reader.Close();
        }

        private void AcceptClient(StreamReader reader, StreamWriter writer)
        {
            string readerStream = reader.ReadToEnd();

            string fileName=readerStream.Split("/")[1];
            File.ReadAllLines(fileName+".txt");
            writer.WriteLine(readerStream);

        }
    }
}
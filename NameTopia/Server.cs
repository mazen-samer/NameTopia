using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Server;
using SharedClasses;
namespace NameTopia
{
    internal class Server
    {
        static List<string> Categories = new List<string>();
        static List<Player> players = new List<Player>();
        static List<Room> rooms = new List<Room>();


        static readonly object lockObj = new object();


        static void ProcessClientRequests(TcpClient tcpClient)
        {
            StreamReader reader = null;
            bool connectionClosed = false;

            try
            {
                reader = new StreamReader(tcpClient.GetStream());
                string command;
                while ((command = reader.ReadLine()) != null)
                {
                    Command currentCommand = JsonConvert.DeserializeObject<Command>(command);
                    switch (currentCommand.CommandType)
                    {
                        case CommandType.CREATE_PLAYER:
                            ClientEventHandler.CreatePlayer(tcpClient, players, currentCommand);
                            break;
                        case CommandType.GET_ROOMS:
                            ClientEventHandler.GetRooms(tcpClient, rooms);
                            break;
                        case CommandType.CREATE_ROOM:
                            ClientEventHandler.CreateRoom(tcpClient, rooms, currentCommand, players);
                            break;
                        case CommandType.GET_CATEGORIES:
                            ClientEventHandler.SendCategories(Categories, tcpClient);
                            break;
                        case CommandType.CLOSE:
                            ClientEventHandler.HandleClientClosure(tcpClient, players);
                            connectionClosed = true;
                            return;
                        default:
                            Console.WriteLine("Unknown command. Available commands: GET_ROOMS, CLOSE");
                            break;
                    }
                }

                // If we exit the loop normally, mark the connection as closed
                connectionClosed = true;
                Console.WriteLine("Connection closed normally.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Connection was forcibly closed: {ex.Message}");
                connectionClosed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing client requests: {ex.Message}");
            }
            finally
            {
                if (!connectionClosed && tcpClient.Connected)
                {
                    tcpClient.Close();
                    Console.WriteLine("Connection closed.");
                }
            }
        }

        static void Main(string[] args)
        {
            TcpListener tcpListener = null;
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5001);
                tcpListener.Start();
                Console.WriteLine("Server has started...");
                GetCategories();
                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Thread t = new Thread(() => ProcessClientRequests(tcpClient));
                    t.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        static void GetCategories()
        {
            string? baseDirectory = Directory.GetCurrentDirectory();
            string solutionDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
            string categoriesPath = Path.Combine(solutionDirectory, "Categories");
            if (baseDirectory == null)
            {
                Console.WriteLine("Path is null / Please make sure you are in the right directory.");
                return;
            }
            try
            {
                Console.WriteLine("Available categories are:");
                foreach (string file in Directory.GetFiles(categoriesPath))
                {
                    string[] File = Path.GetFileName(file).Split('.');
                    Categories.Add(File[0]);
                    Console.Write($"--{File[0]} ");
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
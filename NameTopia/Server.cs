using System.Net;
using System.Net.Sockets;
using Server;
using SharedClasses;
namespace NameTopia
{
    internal class Server
    {
        static int PlayersCount = 0;
        static int RoomsCount = 0;

        static List<string> Categories = new List<string>();
        static List<Player> players = new List<Player>();
        static List<Room> rooms = new List<Room>();

        /// <summary>
        /// This was for testing purposes ==> the constructor was edited --  check SharedClasses for more
        /// </summary> 
        //static Player mazen = new Player(1, "Mazen", null);
        //static Player ramadan = new Player(2, "Ramadan", null);
        //static Player gohamy = new Player(3, "Gohamy", null);
        //static Player player4 = new Player(4, "Player4", null);
        //static Player player5 = new Player(5, "Player5", null);

        //static List<Room> rooms = new List<Room>
        //{
        //    new Room { PlayerOne = mazen, PlayerTwo = ramadan, IsAvailable = false },
        //    new Room { PlayerOne = gohamy, PlayerTwo = player4, IsAvailable = true },
        //    new Room { PlayerOne = player5, PlayerTwo = null, IsAvailable = true }
        //};
        static readonly object lockObj = new object();


        static void ProcessClientRequests(TcpClient tcpClient)
        {
            StreamReader reader = null;
            StreamWriter writer = null;
            Player player = null;
            bool connectionClosed = false;

            try
            {
                NetworkStream stream = tcpClient.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };

                string playerName = reader.ReadLine();
                if (string.IsNullOrEmpty(playerName))
                {
                    Console.WriteLine("Client disconnected before sending player name");
                    return;
                }

                lock (lockObj)
                {
                    player = new Player(++PlayersCount, playerName, tcpClient);
                    players.Add(player);
                }
                Console.WriteLine($"Welcome to the game {playerName}!");
                string command;
                while ((command = reader.ReadLine()) != null)
                {
                    switch (command)
                    {
                        case "REQUEST_PLAYER_DATA":
                            ClientEventHandler.RequestPlayerData(writer, player);
                            break;
                        case "GET_ROOMS":
                            ClientEventHandler.GetRooms(player, rooms);
                            break;
                        case "CREATE_ROOM":
                            ClientEventHandler.CreateRoom(rooms);
                            break;
                        case "CLOSE":
                            ClientEventHandler.HandleClientClosure(player, players);
                            connectionClosed = true;
                            return;
                        default:
                            writer.WriteLine("Unknown command. Available commands: GET_ROOMS, CLOSE");
                            break;
                    }
                }

                // If we exit the loop normally, mark the connection as closed
                connectionClosed = true;
                Console.WriteLine($"Connection closed normally with {playerName}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Connection with player was forcibly closed: {ex.Message}");
                connectionClosed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing client requests: {ex.Message}");
            }
            finally
            {
                if (!connectionClosed)
                {
                    if (player != null)
                    {
                        lock (lockObj)
                        {
                            players.Remove(player);
                        }
                        Console.WriteLine($"Connection closed with {player.Name}");
                    }

                    if (tcpClient.Connected)
                    {
                        tcpClient.Close();
                    }
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
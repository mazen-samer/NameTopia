using System.Net.Sockets;
using Newtonsoft.Json;
using SharedClasses;

namespace Server
{
    abstract internal class ClientEventHandler
    {
        static int PlayersCount = 0;
        static int RoomsCount = 0;


        static readonly object lockObj = new object();
        public static void GetRooms(TcpClient client, List<Room> rooms)
        {
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            Command command = new Command();
            command.CommandType = CommandType.RECIEVE_ROOMS;
            command.Rooms = rooms;
            string jsonString = JsonConvert.SerializeObject(command);
            writer.WriteLine(jsonString);
        }

        public static void HandleClientClosure(TcpClient client, List<Player> players)
        {
            Console.WriteLine($"Closing connection for player");
            client.Close();
            Console.WriteLine($"Connection closed");
        }
        public static void CreatePlayer(TcpClient client, List<Player> players, Command command)
        {
            Player player = new Player(++PlayersCount, command.PlayerName, client);

            Console.WriteLine($"Welcome to the game {player.Name}!");
            player.Client = client;

            lock (lockObj)
            {
                players.Add(player);
            }

            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            Command newCommand = new Command();
            newCommand.CommandType = CommandType.SEND_PLAYER;
            newCommand.Player = player;
            writer.WriteLine(JsonConvert.SerializeObject(newCommand));
        }


        public static void SendCategories(List<string> categories, TcpClient client)
        {
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            Command command = new Command();
            command.CommandType = CommandType.SEND_CATEGORIES;
            command.Categories = categories;
            string jsonString = JsonConvert.SerializeObject(command);
            writer.WriteLine(jsonString);
        }
        public static void CreateRoom(TcpClient client, List<Room> rooms, Command command, List<Player> players)
        {
            Room room = command.Room;
            lock (lockObj)
            {
                room.RoomID = ++RoomsCount;
                room.PlayerOne.Client = client;
                rooms.Add(room);
            }
            Console.WriteLine($"A room was created with ID: {room.RoomID} from player ID: " +
                              $"{room.PlayerOne.ID} and name: {room.PlayerOne.Name} with a category of {room.Category}");
            Console.WriteLine($"Number of current rooms is {rooms.Count}");

            Command command2 = new Command();
            command2.CommandType = CommandType.RECIEVE_ROOMS;
            command2.Rooms = rooms;

            foreach (Player player in players)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(command2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending update to player {player.ID}: {ex.Message}");
                }
            }
        }
    }
}
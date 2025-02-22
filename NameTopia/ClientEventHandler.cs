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
            StreamWriter writer;
            Room room = command.Room;
            room.RoomID = ++RoomsCount;
            room.PlayerOne.Client = client;


            string? baseDirectory = Directory.GetCurrentDirectory();
            string solutionDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
            string categoriesPath = Path.Combine(solutionDirectory, "Categories");
            string selectedCategoryFile = Path.Combine(categoriesPath, room.Category + ".txt");

            List<string> words = new List<string>(File.ReadAllLines(selectedCategoryFile));
            Random random = new Random();
            string randomWord = words[random.Next(words.Count)];

            room.GuessedWord = randomWord;

            Command command1 = new Command();
            command1.CommandType = CommandType.START_GAME;
            command1.Room = room;
            writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            writer.WriteLine(JsonConvert.SerializeObject(command1));

            lock (lockObj)
            {
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
                    if (player.Client.Connected)
                    {
                        writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                        writer.WriteLine(JsonConvert.SerializeObject(command2));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending update to player {player.ID}: {ex.Message}");
                }
            }
        }
        public static void JoinRoom(TcpClient client, Command command, List<Room> rooms, List<Player> players)
        {
            StreamWriter writer;
            lock (lockObj)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (rooms[i].RoomID == command.Room.RoomID)
                    {
                        rooms[i] = command.Room;
                    }
                }
            }
            Console.WriteLine(command.Room.ToString());
            Command command2 = new Command();
            command2.CommandType = CommandType.RECIEVE_ROOMS;
            command2.Rooms = rooms;

            foreach (Player player in players)
            {
                try
                {
                    writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(command2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending update to player {player.ID}: {ex.Message}");
                }
            }
            Console.WriteLine(command.Room.PlayerTwo.ID);
            Command command3 = new Command();
            command3.CommandType = CommandType.START_GAME_FOR_GUEST;
            command3.Room = command.Room;
            foreach (Player player in players)
            {
                if (command.Room.PlayerTwo.ID == player.ID)
                {
                    writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(command3));
                }
            }
            command3.CommandType = CommandType.UPDATE_ROOM;
            foreach (Player player in players)
            {
                if (command.Room.PlayerOne.ID == player.ID)
                {
                    writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(command3));
                }
            }
        }
        public static void UpdateGameStatusForRoom(TcpClient client, Command command, List<Player> players)
        {
            Console.WriteLine(command.Room);
            foreach (Player player in players)
            {
                Console.WriteLine($"this is {command.Room.TurnToPlay.ID.ToString()} and this is " + player.ID.ToString());
                if (command.Room.TurnToPlay.ID == player.ID)
                {
                    StreamWriter writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(command));
                    break;
                }
            }
        }
    }
}
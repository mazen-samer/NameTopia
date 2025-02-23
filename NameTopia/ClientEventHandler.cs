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

        public static void StartSpectate(TcpClient client, Command command, List<Player> players, List<Room> rooms)
        {
            StreamWriter writer;
            Console.WriteLine($"I am player {command.Player.Name} and i want to join {command.Room}");
            command.Room.Spectators.Add(command.Player);
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
            Console.WriteLine("from start spectate function " + command.Room.ToString());
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
            Command command3 = new Command();
            command3.CommandType = CommandType.START_SPECTATE;
            command3.Room = command.Room;
            command3.Player = command.Player;
            writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            writer.WriteLine(JsonConvert.SerializeObject(command3));

            Command command4 = new Command();
            command4.CommandType = CommandType.ADD_SPECTATOR_TO_ROOM;
            command4.Player = command.Player;

            foreach (Player player in players)
            {
                if (player.ID == command.Room.PlayerOne.ID)
                {
                    writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(command4));
                }
                if (player.ID == command.Room.PlayerTwo.ID)
                {
                    writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(command4));
                }
            }

        }
        // el room elly hna mosh bya5odha updated ////// START HEREEEEEE TMRWWWWWWW
        public static void UpdateGameStatusForRoom(TcpClient client, Command command, List<Player> players, List<Room> rooms)
        {
            Console.WriteLine("test1 " + command.Room);
            // First, update the turn so that the current player whose turn it is receives an update.
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
            // Update the room in the rooms list.
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
            // Prepare a command to update all players with the new room list.
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

            // Now send updates to the spectators in the room.
            // Change the command type to the spectator-specific update.
            command2.CommandType = CommandType.UPDATE_SPECTATOR_STATUS;
            command2.Room = command.Room;

            // For each spectator in the room's Spectator list, find their corresponding Player object in the players list
            // (since the spectator objects don't hold the TCP connection).
            if (command.Room.Spectators != null)
            {
                for (int i = 0; i < command.Room.Spectators.Count; i++)
                {
                    Console.WriteLine("There are spectators to be updated");
                    // Get spectator's ID from the room's spectator list.
                    var spectatorId = command.Room.Spectators[i].ID;
                    // Now iterate through the players list to find the matching player.
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (players[j].ID == spectatorId)
                        {
                            try
                            {
                                StreamWriter writer = new StreamWriter(players[j].Client.GetStream()) { AutoFlush = true };
                                writer.WriteLine(JsonConvert.SerializeObject(command2));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error sending spectator update to player {players[j].ID}: {ex.Message}");
                            }
                            break; // Found the matching spectator, no need to check further.
                        }
                    }
                }
            }
        }

        public static void NotifyWinner(Command command, List<Player> players, List<Room> rooms)
        {
            // Find the other player in the room
            Player otherPlayer = null;

            if (command.Player.ID == command.Room.PlayerOne.ID)
            {
                otherPlayer = command.Room.PlayerTwo;
            }
            else if (command.Player.ID == command.Room.PlayerTwo.ID)
            {
                otherPlayer = command.Room.PlayerOne;
            }

            // If the other player is found, send the message to their TCP client
            if (otherPlayer != null)
            {
                // Iterate through the players list to find the other player's TCP client
                foreach (Player player in players)
                {
                    if (player.ID == otherPlayer.ID && player.Client != null)
                    {
                        StreamWriter writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                        writer.WriteLine(JsonConvert.SerializeObject(command));
                        break; // Exit the loop once the other player is found
                    }
                }
            }
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
                    StreamWriter writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(command2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending update to player {player.ID}: {ex.Message}");
                }
            }


            command2.CommandType = CommandType.GAME_OVER_SPECTATOR;
            command2.Room = command.Room;
            command2.Winner = command.Winner;
            if (command.Room.Spectators != null)
            {
                for (int i = 0; i < command.Room.Spectators.Count; i++)
                {
                    Console.WriteLine("There are spectators to be updated");
                    // Get spectator's ID from the room's spectator list.
                    var spectatorId = command.Room.Spectators[i].ID;
                    // Now iterate through the players list to find the matching player.
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (players[j].ID == spectatorId)
                        {
                            try
                            {
                                StreamWriter writer = new StreamWriter(players[j].Client.GetStream()) { AutoFlush = true };
                                writer.WriteLine(JsonConvert.SerializeObject(command2));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error sending spectator update to player {players[j].ID}: {ex.Message}");
                            }
                            break; // Found the matching spectator, no need to check further.
                        }
                    }
                }
            }
        }
    }
}
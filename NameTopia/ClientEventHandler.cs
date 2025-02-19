using Newtonsoft.Json;
using SharedClasses;

namespace Server
{
    abstract internal class ClientEventHandler
    {
        static int RoomsCount = 0;

        static readonly object lockObj = new object();
        public static void GetRooms(Player player, List<Room> rooms)
        {
            StreamWriter writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
            string jsonString = JsonConvert.SerializeObject(rooms);
            writer.WriteLine(jsonString);
        }

        public static void HandleClientClosure(Player player, List<Player> players)
        {
            Console.WriteLine($"Closing connection for player: {player.Name}");
            lock (lockObj)
            {
                players.Remove(player);
            }
            player.Client.Close();
            Console.WriteLine($"Connection closed with {player.Name}");
        }
        public static void RequestPlayerData(StreamWriter writer, Player player)
        {
            string playerJson = JsonConvert.SerializeObject(player);
            writer.WriteLine(playerJson);
        }
        public static void SendCategories(List<string> categories, Player player)
        {
            StreamWriter writer = new StreamWriter(player.Client.GetStream()) { AutoFlush = true };
            string categoriesString = JsonConvert.SerializeObject(categories);
            writer.WriteLine(categoriesString);
        }
        public static void CreateRoom(Player player, List<Room> rooms)
        {
            StreamReader reader = new StreamReader(player.Client.GetStream());
            string category = reader.ReadLine();
            Room room = new Room(++RoomsCount, player, category);
            rooms.Add(room);
            Console.WriteLine($"A room was created with ID: {room.RoomID} from player ID: " +
                              $"{room.PlayerOne.ID} with a category of {room.Category}");
            Console.WriteLine($"Number of current rooms is {rooms.Count}");
        }
    }
}
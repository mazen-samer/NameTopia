using Newtonsoft.Json;
using SharedClasses;

namespace Server
{
    abstract internal class ClientEventHandler
    {
        static readonly object lockObj = new object();
        public static void SendAllRooms(Player player, List<Room> rooms)
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
    }
}
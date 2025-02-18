using Newtonsoft.Json;
using SharedClasses;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    abstract public class ClientEventHandler
    {
        static readonly object lockObj = new object();


        public static List<Room> RequestRooms(TcpClient tcpClient)
        {
            List<Room> rooms = new List<Room>();

            NetworkStream stream = tcpClient.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            writer.WriteLine("GET_ROOMS");
            string jsonString = reader.ReadLine();
            rooms = JsonConvert.DeserializeObject<List<Room>>(jsonString);


            writer.Close();
            reader.Close();
            stream.Close();

            return rooms;
        }

        public static void CreateRoom(TcpClient tcpClient, Room newRoom)
        {
            NetworkStream stream = tcpClient.GetStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            writer.WriteLine("CREATE_ROOM");
            string jsonString = JsonConvert.SerializeObject(newRoom);
            writer.WriteLine(jsonString);


            writer.Close();
            stream.Close();
        }

        public static void JoinRoom(TcpClient tcpClient, string roomId)
        {
            NetworkStream stream = tcpClient.GetStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            writer.WriteLine("JOIN_ROOM");
            writer.WriteLine(roomId);


            writer.Close();
            stream.Close();
        }


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
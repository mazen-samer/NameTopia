using System.Net.Sockets;

namespace SharedClasses
{
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public TcpClient Client { get; set; }

        public Player(int id, string name, TcpClient client)
        {
            ID = id;
            Name = name;
            Client = client;
        }
    }
}

using System.Net.Sockets;

namespace SharedClasses
{

    public class Player
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public TcpClient Client { get; private set; }

        public Player(int id, string name, TcpClient client)
        {
            ID = id;
            Name = name;
            Client = client;
        }
    }
}

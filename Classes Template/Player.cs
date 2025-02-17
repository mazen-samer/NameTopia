using System.Net.Sockets;

namespace Classes_Template
{
    internal class Player
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

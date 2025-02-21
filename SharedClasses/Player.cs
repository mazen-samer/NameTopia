using System.Net.Sockets;
using Newtonsoft.Json;


namespace SharedClasses
{
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [JsonIgnore] // This prevents serialization of TcpClient which causes circular reference
        public TcpClient Client { get; set; }
        public bool isSpectator { get; set; }

        public Player(int id, string name, TcpClient client)
        {
            ID = id;
            Name = name;
            Client = client;
            isSpectator = false;
        }
    }
}

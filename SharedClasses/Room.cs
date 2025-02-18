namespace SharedClasses
{
    [Serializable]
    public class Room
    {

        public string RoomID { get; private set; }
        public string Category { get; private set; }
        public List<Player> Players { get; private set; }
        public bool IsAvailable => Players.Count < 2;

        public Room(string roomID, Player host, string category)
        {
            RoomID = roomID;
            Category = category;
            Players = new List<Player> { host };
        }

        public void JoinRoom(Player player)
        {
            if (IsAvailable)
            {
                Players.Add(player);
            }
        }
    }
}
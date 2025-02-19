namespace SharedClasses
{
    public class Room
    {
        public int RoomID { get; private set; }
        public Player PlayerOne { get; private set; }
        public Player PlayerTwo { get; private set; }
        public bool IsAvailable { get; private set; }
        public string Category { get; set; }
        List<Player> Spectators { get; }

        public Room(int id, Player player, string category)
        {
            RoomID = id;
            PlayerOne = player;
            Category = category;
            IsAvailable = true;
        }
        public override string ToString()
        {
            return $"Room: PlayerOne={PlayerOne?.Name}, PlayerTwo={PlayerTwo?.Name}, IsAvailable={IsAvailable}";
        }
    }
}
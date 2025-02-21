namespace SharedClasses
{
    public class Room
    {
        public int RoomID { get; set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public bool IsAvailable { get; set; }
        public string Category { get; set; }

        public GameManager GameManager { get; set; }
        public List<Player> spectators { get; }
        public int SpectatorCount = 0;

        public Room()
        {

        }
        public Room(Player player, string category)
        {
            PlayerOne = player;
            Category = category;
            Player? playerTwo = PlayerTwo;
            IsAvailable = true;
        }
        public override string ToString()
        {
            return $"Room: PlayerOne={PlayerOne?.Name}, PlayerTwo={PlayerTwo?.Name}, IsAvailable={IsAvailable}";
        }
    }
}
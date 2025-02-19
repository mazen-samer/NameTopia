namespace SharedClasses
{
    public class Room
    {
        public int RoomID { get;  set; }
        public Player PlayerOne { get;  set; }
        public Player PlayerTwo { get;  set; }
        public bool IsAvailable { get;  set; }
        public string Category { get; set; }
        
        public int SpectatorCount = 0;

        public Room()
        {

        }
        public Room(int id, Player player, string category)
        {
            RoomID = id;
            PlayerOne = player;
            Category = category;
            Player? playerTwo = PlayerTwo;
            
        }
        public override string ToString()
        {
            return $"Room: PlayerOne={PlayerOne?.Name}, PlayerTwo={PlayerTwo?.Name}, IsAvailable={IsAvailable}";
        }
    }
}
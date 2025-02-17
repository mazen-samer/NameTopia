namespace SharedClasses
{
    public class Room
    {
        public string RoomID { get; private set; }
        public Player PlayerOne { get; private set; }
        public Player PlayerTwo { get; private set; }
        public bool IsAvailable { get; private set; }

        public Room(string id, Player player)
        {
            RoomID = id;
            PlayerOne = player;
        }
        public override string ToString()
        {
            return $"Room: PlayerOne={PlayerOne?.Name}, PlayerTwo={PlayerTwo?.Name}, IsAvailable={IsAvailable}";
        }
    }
}
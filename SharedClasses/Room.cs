namespace SharedClasses
{
    public class Room
    {
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public bool IsAvailable { get; set; }

        public override string ToString()
        {
            return $"Room: PlayerOne={PlayerOne?.Name}, PlayerTwo={PlayerTwo?.Name}, IsAvailable={IsAvailable}";
        }
    }
}
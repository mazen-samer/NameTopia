namespace SharedClasses
{
    public enum PlayerTurn
    {
        OWNER, GUEST
    }
    public class Room
    {

        public int RoomID { get; set; }
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public Player TurnToPlay { get; set; }
        public string Category { get; set; }
        public string GuessedWord { get; set; }
        public PlayerTurn PlayerTurn { get; set; }
        public List<Player> Spectator { get; set; }
        public bool isJoinable { get; set; }
        public string GameText { get; set; }
        public Room()
        {
            PlayerTurn = PlayerTurn.OWNER;

        }
        public Room(Player player, string category)
        {
            PlayerOne = player;
            Category = category;
            isJoinable = true;
        }
        public override string ToString()
        {
            return $"RoomID={RoomID}, Guessed Word={GuessedWord}, Gametext={GameText} PlayerOne={PlayerOne?.Name}, PlayerTwo={PlayerTwo?.Name}, Category={Category}";
        }
    }
}
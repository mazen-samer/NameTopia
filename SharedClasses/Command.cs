namespace SharedClasses
{
    public enum CommandType
    {
        //server commands
        CREATE_PLAYER,
        GET_ROOMS,
        CREATE_ROOM,
        GET_CATEGORIES,
        CLOSE,

        // client commands
        SEND_PLAYER,
        RECIEVE_ROOMS,
        SEND_CATEGORIES,
        START_GAME,
    }
    public class Command
    {
        public CommandType CommandType { get; set; }
        public Player? Player { get; set; }
        public Room? Room { get; set; }
        public List<Room>? Rooms { get; set; }
        public List<Player>? Players { get; set; }
        public List<string> Categories { get; set; }
        public string PlayerName { get; set; }

        public Command() { }

        // Constructor to initialize CommandType
        public Command(CommandType type)
        {
            CommandType = type;
        }
    }
}

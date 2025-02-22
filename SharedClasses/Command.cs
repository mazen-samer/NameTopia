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
        JOIN_ROOM,

        // client commands
        SEND_PLAYER,
        RECIEVE_ROOMS,
        SEND_CATEGORIES,
        START_GAME,
        START_GAME_FOR_GUEST,
        UPDATE_ROOM,

        // both
        UPDATE_GAME_STATUS,
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
        public string PressedLetter { get; set; }
        public string GameText { get; set; }

        public Command() { }

        // Constructor to initialize CommandType
        public Command(CommandType type)
        {
            CommandType = type;
        }

        public override string ToString()
        {
            return $"CommandType: {CommandType}, " +
                   $"Player: {Player?.ToString() ?? "null"}, " +
                   $"Room: {Room?.ToString() ?? "null"}, " +
                   $"Rooms: {RoomsToString()}, " +
                   $"Players: {PlayersToString()}, " +
                   $"Categories: {string.Join(", ", Categories ?? new List<string>())}, " +
                   $"PlayerName: {PlayerName}, " +
                   $"PressedLetter: {PressedLetter}, " +
                   $"GameText: {GameText}";
        }

        private string RoomsToString()
        {
            if (Rooms == null) return "null";
            return string.Join("; ", Rooms.Select(r => r.ToString()));
        }

        private string PlayersToString()
        {
            if (Players == null) return "null";
            return string.Join("; ", Players.Select(p => p.ToString()));
        }
    }
}

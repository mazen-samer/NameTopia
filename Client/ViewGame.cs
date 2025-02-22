using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using SharedClasses;

namespace Client
{
    public partial class ViewGame : Form
    {
        StringBuilder dashedWordBuilder;
        TcpClient client;
        Room room;
        public ViewGame(Room room, string PlayerName, TcpClient tcpClient)
        {
            InitializeComponent();
            CreateKeyboardButtons();
            this.room = room;
            pagePlayerName.Text = PlayerName;
            client = tcpClient;
            OwnerName.Text = playerTurn.Text = room.PlayerOne.Name;
            GuestName.Text = room.PlayerTwo?.Name ?? "Waiting for another player to join...";

            dashedWordBuilder = new StringBuilder(new string('_', room.GuessedWord.Length));
            guessedWord.Text = dashedWordBuilder.ToString();

            this.Enabled = false;
        }
        private void CreateKeyboardButtons()
        {
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                Button btn = new Button
                {
                    Text = letter.ToString(),
                    Width = 40,
                    Height = 40,
                    Tag = letter  // Store the letter in Tag
                };

                btn.Click += KeyboardButton_Click;
                keyboardPanel.Controls.Add(btn);
            }
        }
        //This function is just for the Owner of the Game updating that someone joined!!!
        public void UpdateRoom(Room room)
        {
            GuestName.Text = room.PlayerTwo?.Name ?? "";
            this.room.PlayerTwo = room.PlayerTwo;
            this.Enabled = room.PlayerTurn == PlayerTurn.OWNER;
        }

        private void KeyboardButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Enabled = false;
                string letter = btn.Tag.ToString();

                // Update the displayed guessed word by revealing the pressed letter.
                // We assume room.GuessedWord contains the complete word and 
                // guessedWord.Text contains the current state (e.g., "_ _ _ _").
                char[] displayedChars = guessedWord.Text.ToCharArray();
                for (int i = 0; i < room.GuessedWord.Length; i++)
                {
                    // Compare letters ignoring case
                    if (room.GuessedWord[i].ToString().Equals(letter, StringComparison.OrdinalIgnoreCase))
                    {
                        displayedChars[i] = room.GuessedWord[i];
                    }
                }
                string updatedGameText = new string(displayedChars);
                guessedWord.Text = updatedGameText;

                // Toggle player's turn.
                this.Enabled = false;
                if (room.PlayerTurn == PlayerTurn.OWNER)
                {
                    room.PlayerTurn = PlayerTurn.GUEST;
                    this.room.TurnToPlay = this.room.PlayerTwo;
                    playerTurn.Text = this.room.PlayerTwo.Name;  // Show next player's turn
                }
                else
                {
                    room.PlayerTurn = PlayerTurn.OWNER;
                    this.room.TurnToPlay = this.room.PlayerOne;
                    playerTurn.Text = this.room.PlayerOne.Name;  // Show next player's turn
                }

                // Create a command with the updated game text.
                Command command = new Command();
                command.CommandType = CommandType.UPDATE_GAME_STATUS;
                command.PressedLetter = letter;
                command.Room = this.room;
                command.GameText = updatedGameText;
                StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                writer.WriteLine(JsonConvert.SerializeObject(command));
            }
        }
        public void UpdateGameStatus(Command command)
        {
            this.Enabled = true;
            guessedWord.Text = command.GameText;
            this.room = command.Room;  // Update the entire room state

            // Update the turn label based on who should play next
            playerTurn.Text = (command.Room.PlayerTurn == PlayerTurn.OWNER)
                ? command.Room.PlayerOne.Name
                : command.Room.PlayerTwo.Name;
            DisableKey(command.PressedLetter);
        }
        private void DisableKey(string letter)
        {
            foreach (Control ctrl in keyboardPanel.Controls)
            {
                if (ctrl is Button btn && btn.Tag.ToString().Equals(letter, StringComparison.OrdinalIgnoreCase))
                {
                    btn.Enabled = false;
                    break; // Exit after finding and disabling the matching button.
                }
            }
        }

    }
}

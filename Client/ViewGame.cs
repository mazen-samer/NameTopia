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
        Player player;
        public ViewGame(Room room, Player PlayerName, Player player)
        {
            InitializeComponent();
            CreateKeyboardButtons();
            this.room = room;
            pagePlayerName.Text = PlayerName.Name;
            this.player = player;
            client = player.Client;
            OwnerName.Text = playerTurn.Text = room.PlayerOne.Name;
            GuestName.Text = room.PlayerTwo?.Name ?? "Waiting for another player to join...";

            dashedWordBuilder = new StringBuilder(new string('_', room.GuessedWord.Length));
            guessedWord.Text = PlayerName.isSpectator ? room.GameText : dashedWordBuilder.ToString();
            this.keyboardPanel.Enabled = false;
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
            this.keyboardPanel.Enabled = room.PlayerTurn == PlayerTurn.OWNER;
        }

        private void KeyboardButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Enabled = false;
                string letter = btn.Tag.ToString();

                // Update the displayed guessed word
                char[] displayedChars = guessedWord.Text.ToCharArray();
                bool foundLetter = false;

                for (int i = 0; i < room.GuessedWord.Length; i++)
                {
                    if (room.GuessedWord[i].ToString().Equals(letter, StringComparison.OrdinalIgnoreCase))
                    {
                        displayedChars[i] = room.GuessedWord[i];
                        foundLetter = true;
                    }
                }

                string updatedGameText = new string(displayedChars);
                guessedWord.Text = updatedGameText;


                Player loserPlayer = (room.PlayerTurn == PlayerTurn.OWNER) ? room.PlayerOne : room.PlayerTwo;

                // Check if the word is fully guessed (win condition)
                if (!updatedGameText.Contains('_'))
                {
                    string winner = playerTurn.Text;  // The last player who guessed correctly

                    MessageBox.Show($"Congratulations {winner}, you won!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.room.isJoinable = false;
                    // Send win status to the server
                    Command winCommand = new Command
                    {
                        CommandType = CommandType.GAME_OVER,
                        Room = this.room,
                        GameText = updatedGameText,
                        Winner = winner,
                        Player = this.player,
                    };

                    StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(JsonConvert.SerializeObject(winCommand));

                    this.Hide();
                }

                // Toggle player's turn
                this.keyboardPanel.Enabled = false;
                if (room.PlayerTurn == PlayerTurn.OWNER)
                {
                    room.PlayerTurn = PlayerTurn.GUEST;
                    this.room.TurnToPlay = this.room.PlayerTwo;
                    playerTurn.Text = this.room.PlayerTwo.Name;
                }
                else
                {
                    room.PlayerTurn = PlayerTurn.OWNER;
                    this.room.TurnToPlay = this.room.PlayerOne;
                    playerTurn.Text = this.room.PlayerOne.Name;
                }

                // Send the updated game status to the server
                this.room.GameText = updatedGameText;
                Command command = new Command
                {
                    CommandType = CommandType.UPDATE_GAME_STATUS,
                    PressedLetter = letter,
                    Room = this.room,
                    GameText = updatedGameText,
                    Loser = loserPlayer
                };

                StreamWriter updateWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
                updateWriter.WriteLine(JsonConvert.SerializeObject(command));
            }
        }

        public void UpdateGameStatus(Command command)
        {
            this.keyboardPanel.Enabled = true;
            guessedWord.Text = command.GameText;
            this.room = command.Room;

            playerTurn.Text = (command.Room.PlayerTurn == PlayerTurn.OWNER)
                ? command.Room.PlayerOne.Name
                : command.Room.PlayerTwo.Name;

            DisableKey(command.PressedLetter);
            if (command.CommandType == CommandType.GAME_OVER)
            {
                MessageBox.Show($"Congratulations {command.Winner}, you won!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();  // Disable further interaction
            }
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
        public void UpdateSpectatorStatus(Command command)
        {
            MessageBox.Show("test for spectator");
            MessageBox.Show(command.Room?.ToString() ?? "Empty room");
            // Spectators only see updates without interaction.
            guessedWord.Text = command.GameText;
            this.room = command.Room;

            // Update the turn display for information.
            playerTurn.Text = (command.Room.PlayerTurn == PlayerTurn.OWNER)
                                ? command.Room.PlayerOne.Name
                                : command.Room.PlayerTwo?.Name ?? "Waiting...";

            // Ensure that interactive controls remain disabled.
            keyboardPanel.Enabled = false;

            // If the command signals a game-over for spectators, show a message and close the view.
            if (command.CommandType == CommandType.GAME_OVER_SPECTATOR)
            {
                MessageBox.Show($"Game Over! Winner is: {command.Winner}", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
        }

        public void AddSpectatorToRoom(Player player)
        {
            this.room.Spectators.Add(player);
        }

    }
}

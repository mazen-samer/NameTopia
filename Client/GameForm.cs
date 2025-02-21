using SharedClasses;

namespace Client
{
    public partial class GameForm : Form
    {
        Player GameOwner { get; set; }
        Player playerTwo { get; set; }
        string GuessedWord { get; set; }

        public int player1Score = 0, player2Score = 0;
        private HashSet<char> guessedLetters = new HashSet<char>();
        private char[] displayedWord;
        private int numOfWatchers = 0;

        public GameForm(Room room)
        {
            InitializeComponent();
            GameOwner = room.PlayerOne;
            playerTwo = room.PlayerTwo;
            //GameOwner = gameManager.playerOne;
            //GuessedWord = gameManager.GuessedWord;
            lblPlayer.Text = $"Current Player: {GameOwner?.Name}";
            lblStatus.Text = $"{GameOwner?.Name}'s Turn";
            lblPlayer1Score.Text = $"{GameOwner?.Name} Score: {player1Score}";
            lblPlayer2Score.Text = $"{playerTwo?.Name} Score: {player2Score}";
            lblNumOfWatchers.Text = $"Watchers: {room.spectators.Count}";
        }
        //private void btnQuit_Click(object sender, EventArgs e)
        //{
        //    if (GameOwner.isSpectator || playerTwo.isSpectator)
        //    {
        //        this.Close();
        //        QuitWatching();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Players cannot quit the game!");
        //    }
        //}
        private void LetterButton_Click(object sender, EventArgs e)
        {
            //Button letterButton = sender as Button;
            //if (letterButton != null)
            //{
            //    char guessedLetter = letterButton.Text[0];
            //    letterButton.Enabled = false;

            //    if (guessedLetters.Contains(guessedLetter))
            //        return;

            //    guessedLetters.Add(guessedLetter);
            //    if (hiddenWord.Contains(guessedLetter))
            //    {
            //        for (int i = 0; i < hiddenWord.Length; i++)
            //        {
            //            if (hiddenWord[i] == guessedLetter)
            //                displayedWord[i] = guessedLetter;
            //        }

            //        lblWord.Text = string.Join(" ", displayedWord);

            //        if (!displayedWord.Contains('_'))
            //        {
            //            gameManager.EndGame(currentPlayer == 1 ? player1Name : player2Name);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        gameManager.SwitchTurn();
            //    }

            //    // Broadcast the guessed letter to all clients
            //    gameManager.BroadcastMessage($"GUESS:{guessedLetter}");
            //}
        }
        //public void EnableKeyboard(bool enable)
        //{
        //    IsKeyboardEnabled = enable;
        //    foreach (Control control in pnlKeyboard.Controls)
        //    {
        //        if (control is Button btn)
        //            btn.Enabled = enable;
        //    }
        //}
        //private void SwitchTurn()
        //{
        //    currentPlayer = (currentPlayer == 1) ? 2 : 1;
        //    string currentPlayerName = (currentPlayer == 1) ? player1Name : player2Name;
        //    lblStatus.Text = $"{currentPlayerName}'s Turn";
        //    lblPlayer.Text = $"Current Player: {currentPlayerName}";
        //}
        //public void EndGame(string winner, int player1Score, int player2Score)
        //{
        //    // Update the scores
        //    this.player1Score = player1Score;
        //    this.player2Score = player2Score;

        //    // Update the UI
        //    lblPlayer1Score.Text = $"{player1Name} Score: {player1Score}";
        //    lblPlayer2Score.Text = $"{player2Name} Score: {player2Score}";

        //    MessageBox.Show($"{winner} wins! The word was: {hiddenWord}");

        //    // Only show the "play again" dialog for players, not watchers
        //    if (!isWatcher)
        //    {
        //        DialogResult result = MessageBox.Show("Do you want to play again?", "Replay",
        //            MessageBoxButtons.YesNo);
        //        if (result == DialogResult.Yes)
        //            RestartGame();
        //        else
        //        {
        //            SaveGameResult(winner, hiddenWord);
        //            this.Close();
        //        }
        //    }
        //}
        //private void SaveGameResult(string winner, string word)
        //{
        //    try
        //    {
        //        string filePath = Path.Combine(Application.StartupPath, "GameResults.txt");
        //        string resultLine = $"{DateTime.Now}: {winner} won by guessing '{word}'\n";
        //        File.AppendAllText(filePath, resultLine);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error writing game result: " + ex.Message);
        //    }
        //    ReadGameResults();
        //}
        //private void RestartGame()
        //{
        //    guessedLetters.Clear();
        //    displayedWord = new string('_', hiddenWord.Length).ToCharArray();
        //    lblWord.Text = string.Join(" ", displayedWord);
        //    currentPlayer = 1;
        //    lblStatus.Text = $"{player1Name}'s Turn";
        //    lblPlayer.Text = $"Current Player: {player1Name}";

        //    foreach (Control control in pnlKeyboard.Controls)
        //    {
        //        if (control is Button btn)
        //            btn.Enabled = true;
        //    }

        //    gameManager.BroadcastMessage("RESTART");
        //}
        private void CreateKeyboard()
        {
            int x = 10, y = 10;
            for (char c = 'A'; c <= 'Z'; c++)
            {
                Button btn = new Button();
                btn.Text = c.ToString();
                btn.Size = new Size(30, 30);
                btn.Location = new Point(x, y);
                btn.Click += LetterButton_Click;
                pnlKeyboard.Controls.Add(btn);

                x += 35;
                if (x + 40 > pnlKeyboard.Width)
                {
                    x = 10;
                    y += 40;
                }
            }
        }
        //private void AddWatcher()
        //{
        //    numOfWatchers++;
        //    lblNumOfWatchers.Text = $"Watchers: {numOfWatchers}";
        //}
        //private void QuitWatching()
        //{
        //    lblNumOfWatchers.Text = $"Watchers: {numOfWatchers - 1}";
        //}

        //private void ReadGameResults()
        //{
        //    try
        //    {
        //        string filePath = Path.Combine(Application.StartupPath, "GameResults.txt");
        //        if (File.Exists(filePath))
        //        {
        //            string results = File.ReadAllText(filePath);
        //            MessageBox.Show(results, "Game Results");
        //        }
        //        else
        //        {
        //            MessageBox.Show("GameResults.txt not found.", "File Not Found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error reading game results: " + ex.Message);
        //    }
        //}
        //protected override void OnFormClosed(FormClosedEventArgs e)
        //{
        //    base.OnFormClosed(e);
        //    stream.Close();
        //    client.Close();
        //}
    }
}

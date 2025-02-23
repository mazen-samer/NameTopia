using System.Net.Sockets;
using Newtonsoft.Json;
using SharedClasses;

namespace Client
{
    public partial class EntryPoint : Form
    {
        private TcpClient tcpClient;
        Player player;
        private StreamReader reader;
        private Thread listeningThread;
        ViewRooms playerForm;
        ViewGame viewGame;


        public EntryPoint()
        {
            InitializeComponent();
        }

        void StartListening()
        {
            try
            {
                while (true)
                {
                    string message = reader.ReadLine();
                    if (string.IsNullOrEmpty(message))
                        continue;

                    Command receivedCommand = JsonConvert.DeserializeObject<Command>(message);
                    HandleIncomingCommand(receivedCommand);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void HandleIncomingCommand(Command command)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => HandleIncomingCommand(command)));
                return;
            }

            switch (command.CommandType)
            {
                case CommandType.SEND_PLAYER:
                    player = command.Player;
                    player.Client = tcpClient;
                    playerForm = new ViewRooms(player);
                    playerForm.Owner = this;
                    playerForm.Show();
                    this.Hide();
                    break;

                case CommandType.RECIEVE_ROOMS:
                    playerForm?.DisplayRooms(command.Rooms);
                    break;

                case CommandType.SEND_CATEGORIES:
                    // command contains the categories list
                    playerForm?.assignCategoriesAndOpenDialog(command.Categories);
                    break;

                case CommandType.START_GAME:
                    viewGame = new ViewGame(command.Room, command.Room.PlayerOne, player);
                    viewGame.Show();
                    break;
                //This function is just for the Owner of the Game updating that someone joined!!!
                case CommandType.UPDATE_ROOM:
                    viewGame?.UpdateRoom(command.Room);
                    break;
                //Guest Player
                case CommandType.START_GAME_FOR_GUEST:
                    viewGame = new ViewGame(command.Room, command.Room.PlayerTwo, player);
                    viewGame.Show();
                    break;
                case CommandType.UPDATE_GAME_STATUS:
                    viewGame.UpdateGameStatus(command);
                    break;
                case CommandType.UPDATE_SPECTATOR_STATUS:
                    viewGame.UpdateSpectatorStatus(command);
                    break;
                case CommandType.START_SPECTATE:
                    viewGame = new ViewGame(command.Room, command.Player, player);
                    viewGame?.Show();
                    break;
                case CommandType.GAME_OVER:
                    viewGame.UpdateGameStatus(command);
                    break;
                case CommandType.ADD_SPECTATOR_TO_ROOM:
                    viewGame.AddSpectatorToRoom(command.Player);
                    break;
                case CommandType.GAME_OVER_SPECTATOR:
                    viewGame.UpdateSpectatorStatus(command);
                    break;
            }
        }
        private void PlayerForm_CloseConnectionRequested(object sender, EventArgs e)
        {
            // Close the TCP connection.
            tcpClient?.Close();

            // Optionally, close the owner form (ensure thread-safety if needed).
            this.Invoke(new Action(() => this.Close()));
        }

        private void connectButton_click(object sender, EventArgs e)
        {
            string playerName = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(playerName))
            {
                MessageBox.Show("Please enter a valid name");
                return;
            }

            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect("127.0.0.1", 5001);

                StreamWriter writer = new StreamWriter(tcpClient.GetStream()) { AutoFlush = true };
                reader = new StreamReader(tcpClient.GetStream());

                Command newCommand = new Command
                {
                    CommandType = CommandType.CREATE_PLAYER,
                    PlayerName = playerName
                };

                string commandJson = JsonConvert.SerializeObject(newCommand);
                writer.WriteLine(commandJson);

                listeningThread = new Thread(StartListening);
                listeningThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection to the server is lost. Error: {ex.Message}");
            }
        }
    }
}

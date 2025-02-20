using System.Net.Sockets;
using Newtonsoft.Json;
using SharedClasses;

namespace Client
{
    public partial class EntryPoint : Form
    {
        private TcpClient tcpClient;
        private StreamReader reader;
        private Thread listeningThread;
        ViewRooms playerForm;

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
                MessageBox.Show($"Error while listening for messages: {ex.Message}");
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
                    Player player = command.Player;
                    player.Client = tcpClient;
                    playerForm = new ViewRooms(player);
                    playerForm.Show();
                    this.Hide();
                    break;
                case CommandType.RECIEVE_ROOMS:
                    playerForm.DisplayRooms(command.Rooms);
                    break;
                case CommandType.SEND_CATEGORIES:
                    // command contains the categories list
                    playerForm.assignCategoriesAndOpenDialog(command.Categories);
                    break;
            }
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

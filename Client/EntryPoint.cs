using System.Net.Sockets;
using Newtonsoft.Json;
using SharedClasses;

namespace Client
{
    public partial class EntryPoint : Form
    {
        public EntryPoint()
        {
            InitializeComponent();
        }

        private void connectButton_click(object sender, EventArgs e)
        {
            string playerName = textBox1.Text.Trim();

            if (!string.IsNullOrWhiteSpace(playerName))
            {
                TcpClient tcpClient = new TcpClient();
                try
                {
                    tcpClient.Connect("127.0.0.1", 5001);
                    StreamWriter writer = new StreamWriter(tcpClient.GetStream()) { AutoFlush = true };
                    StreamReader reader = new StreamReader(tcpClient.GetStream());

                    writer.WriteLine(playerName);
                    // Request for the player data
                    writer.WriteLine("REQUEST_PLAYER_DATA");

                    string playerJson = reader.ReadLine();

                    if (string.IsNullOrEmpty(playerJson))
                    {
                        throw new Exception("Received null or empty player data from the server.");
                    }

                    Player player = JsonConvert.DeserializeObject<Player>(playerJson);

                    if (player != null)
                    {
                        player.Client = tcpClient; // Assign the connection

                        this.Invoke(new Action(() =>
                        {
                            ViewRooms playerForm = new ViewRooms(player);
                            playerForm.Show();
                        }));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection to the server is lost. Error: {ex.Message}");

                    if (tcpClient.Connected)
                        tcpClient.Close();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid name");
            }
        }

    }
}
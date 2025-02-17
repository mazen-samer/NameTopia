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
            if (textBox1.Text != "")
            {
                Thread newClientThread = new Thread(() =>
                {
                    TcpClient tcpClient = new TcpClient();
                    try
                    {
                        tcpClient.Connect("127.0.0.1", 5001);
                        StreamWriter writer = new StreamWriter(tcpClient.GetStream()) { AutoFlush = true };
                        StreamReader reader = new StreamReader(tcpClient.GetStream());

                        writer.WriteLine(textBox1.Text);
                        writer.WriteLine("GET_ROOMS");
                        string recievedRooms = reader.ReadLine();
                        List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(recievedRooms);
                        ViewRooms viewRooms = new ViewRooms(tcpClient, rooms);
                        viewRooms.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Connection to the server is lost. Error: {ex.Message}");
                    }
                    finally
                    {
                        tcpClient.Close();
                    }
                });
                newClientThread.Start();
            }
        }
    }
}
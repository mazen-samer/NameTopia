using System.Net.Sockets;
using SharedClasses;

namespace Client
{
    public partial class ViewRooms : Form
    {
        TcpClient tcpClient;
        List<Room> rooms;

        public ViewRooms(TcpClient tcpClient, List<Room> rooms)
        {
            this.tcpClient = tcpClient;
            this.rooms = rooms;
            InitializeComponent();
            PopulateRoomsList();
        }

        private void PopulateRoomsList()
        {
            foreach (var room in rooms)
            {
                listBoxRooms.Items.Add(room.ToString());
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(tcpClient.GetStream()))
            {
                sw.WriteLine("CLOSE");
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
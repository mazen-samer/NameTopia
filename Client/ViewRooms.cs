using Server;
using SharedClasses;
using System.Net.Sockets;

namespace Client
{
    public partial class ViewRooms : Form
    {


        int roomCounter = 1;
        TcpClient tcpClient;
        private List<Room> roomList = new List<Room>();
        public ViewRooms(TcpClient tcpClient, List<Room> rooms)
        {
            this.tcpClient = tcpClient;
            //this.roomList = rooms;
            InitializeComponent();
            roomList = ClientEventHandler.RequestRooms(tcpClient);


            PopulateRoomsList();
        }

        private void PopulateRoomsList() ////??
        {
            foreach (var room in roomList)
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



        private void listBoxRooms_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            string category = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(category))
            {
                Status.Text = "Please select a category";
                return;
            }

            Player player = new Player(1, "Host", tcpClient);
            Room newRoom = new Room($"Room_{roomCounter++}", player, category);
            //===
            ClientEventHandler.CreateRoom(tcpClient, newRoom);

            roomList.Add(newRoom);
            listBoxRooms.Items.Add($"{newRoom.RoomID} - {newRoom.Category}");
            Status.Text = $"Room Created!";
            comboBox1.SelectedIndex = -1;
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            if (listBoxRooms.SelectedItem == null)
            {
                Status.Text = "Please select a room to join";
                return;
            }

            string selectedRoomID = listBoxRooms.SelectedItem.ToString().Split('-')[0].Trim();
            Room selectedRoom = null;
            foreach (Room room in roomList)
            {
                if (room.RoomID.Trim() == selectedRoomID)
                {
                    selectedRoom = room;
                    break;
                }
            }
            if (selectedRoom == null)
            {
                Status.Text = "Room not found";
                return;
            }
            if (!selectedRoom.IsAvailable)
            {
                Status.Text = "Room is full!";
                return;
            }
            //==

            ClientEventHandler.JoinRoom(tcpClient, selectedRoomID);
            Player player = new Player(1, "guest", tcpClient);
            selectedRoom.JoinRoom(player);

            listBoxRooms.Items[listBoxRooms.SelectedIndex] = $"{selectedRoom.RoomID} - {selectedRoom.Category} [Full]";
            Status.Text = $"Joined Room {selectedRoom.RoomID}!";
        }
    }
}
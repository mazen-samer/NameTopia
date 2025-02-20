using Newtonsoft.Json;
using SharedClasses;

namespace Client
{
    public partial class ViewRooms : Form
    {
        Player currentPlayer;
        List<Room> rooms;
        StreamWriter writer;
        List<string> Categories;
        public ViewRooms(Player player)
        {
            InitializeComponent();
            currentPlayer = player;
            writer = new StreamWriter(currentPlayer.Client.GetStream()) { AutoFlush = true };
            label2.Text = $"Welcome to the game {currentPlayer.Name}\n You are assigned to ID: {currentPlayer.ID}";
            getAllRooms();
        }

        public void getAllRooms()
        {
            try
            {
                Command newCommand = new Command();
                newCommand.CommandType = CommandType.GET_ROOMS;
                string command = JsonConvert.SerializeObject(newCommand);
                writer.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching rooms: " + ex.Message);
            }
        }

        public void DisplayRooms(List<Room> rooms)
        {
            flowLayoutPanelRooms.Controls.Clear();

            foreach (Room room in rooms)
            {
                Panel roomPanel = new Panel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Width = 330,
                    Height = 120,
                    Margin = new Padding(10)
                };

                // Label for Room ID
                Label lblRoomId = new Label
                {
                    Text = $"Room ID: {room.RoomID}",
                    Location = new Point(10, 10),
                    AutoSize = true
                };
                roomPanel.Controls.Add(lblRoomId);
                //PlayerOne name
                Label lblPlayerOnename = new Label
                {
                    Text = $"Player 1: {room.PlayerOne.Name}",
                    Location = new Point(10, 30),
                    AutoSize = true
                };
                roomPanel.Controls.Add(lblPlayerOnename);
                //PlayerTwo name
                Label lblPlayerTwoname = new Label
                {
                    Text = $"Player 2: {room.PlayerTwo?.Name ?? "Waiting for player..."}",
                    Location = new Point(10, 50),
                    AutoSize = true
                };
                roomPanel.Controls.Add(lblPlayerTwoname);
                // Label for Spectator Count
                Label lblSpectators = new Label
                {
                    Text = $"Spectators: {room.SpectatorCount}",
                    Location = new Point(10, 70),
                    AutoSize = true
                };
                roomPanel.Controls.Add(lblSpectators);

                // Label for Availability
                Label lblAvailability = new Label
                {
                    Text = room.IsAvailable ? "Available" : "Full",
                    Location = new Point(10, 90),
                    AutoSize = true,
                    ForeColor = room.IsAvailable ? Color.Green : Color.Red
                };
                roomPanel.Controls.Add(lblAvailability);

                // Button to Join the room
                Button btnJoin = new Button
                {
                    Text = "Join",
                    Location = new Point(210, 10),
                    Size = new Size(90, 30)
                };
                btnJoin.Click += (sender, e) => { JoinRoom(room); };
                roomPanel.Controls.Add(btnJoin);

                // Button to Spectate the room with an icon
                Button btnSpectate = new Button
                {
                    Text = "Spectate",
                    Location = new Point(210, 50),
                    Size = new Size(90, 30)
                };

                // Optionally, assign a spy icon if you have one in your resources:
                // btnSpectate.Image = YourProjectNamespace.Properties.Resources.spyIcon;
                // btnSpectate.ImageAlign = ContentAlignment.MiddleLeft;
                // btnSpectate.TextAlign = ContentAlignment.MiddleRight
                //btnSpectate.Image = Client.Properties.Resources.spyIcon;

                roomPanel.Controls.Add(btnSpectate);

                // Add the room panel to the FlowLayoutPanel
                flowLayoutPanelRooms.Controls.Add(roomPanel);
            }
        }


        private void JoinRoom(Room room)
        {
            MessageBox.Show(room.RoomID.ToString());
        }

        public void closeButton_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(currentPlayer.Client.GetStream()))
            {
                Command command = new Command { CommandType = CommandType.CLOSE };
                string commandJson = JsonConvert.SerializeObject(command);
                sw.WriteLine(commandJson);
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void createRoomButton_Click(object sender, EventArgs e)
        {
            Command newCommand = new Command();
            newCommand.CommandType = CommandType.GET_CATEGORIES;
            string commandjson = JsonConvert.SerializeObject(newCommand);
            writer.WriteLine(commandjson);
        }
        public void assignCategoriesAndOpenDialog(List<string> cats)
        {
            Command newCommand = new Command();
            string chosenCategory = string.Empty;
            CategorySelection categorySelection = new CategorySelection(cats);
            categorySelection.ShowDialog();

            if (categorySelection.DialogResult == DialogResult.OK)
            {
                chosenCategory = categorySelection.selectedCategory;
                Room room = new Room(currentPlayer, chosenCategory);
                string roomString = JsonConvert.SerializeObject(room);
                newCommand.CommandType = CommandType.CREATE_ROOM;
                newCommand.Room = room;
                string commandjson = JsonConvert.SerializeObject(newCommand);
                writer.WriteLine(commandjson);
            }
            else
            {
                MessageBox.Show("Category selection was canceled.");
            }
        }

    }
}
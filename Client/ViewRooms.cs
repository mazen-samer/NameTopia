using Newtonsoft.Json;
using SharedClasses;

namespace Client
{
    public partial class ViewRooms : Form
    {
        Player currentPlayer;
        List<Room> rooms;

        public ViewRooms(Player player)
        {
            InitializeComponent();
            currentPlayer = player;
            label2.Text = $"Welcome to the game {currentPlayer.Name}\n You are assigned to ID: {currentPlayer.ID}";
            getAllRooms();
        }

        void getAllRooms()
        {
            // Sending the "GET_ROOMS" command to the server:
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(currentPlayer.Client.GetStream(), leaveOpen: true);
                writer.AutoFlush = true;
                writer.WriteLine("GET_ROOMS");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending GET_ROOMS command: " + ex.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }

            // Reading the server's response:
            StreamReader reader = null;
            try
            {
                // Create a StreamReader (again, without disposing if the stream remains in use).
                reader = new StreamReader(currentPlayer.Client.GetStream(), leaveOpen: true);
                string jsonResponse = reader.ReadLine();
                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    Console.WriteLine("Received JSON: " + jsonResponse);
                    // Deserialize the JSON response into a List of Room objects.
                    rooms = JsonConvert.DeserializeObject<List<Room>>(jsonResponse);

                    // Now update the UI with the new room data.
                    DisplayRooms(rooms);
                }


                // Here you can update the UI control that lists the rooms.
                // For example, if you have a ListBox named listBoxRooms:
                // listBoxRooms.DataSource = rooms.Select(r => r.RoomName).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading server response: " + ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }

        }
        private void DisplayRooms(List<Room> rooms)
        {
            // Clear existing room panels (if any)
            flowLayoutPanelRooms.Controls.Clear();

            // Iterate through the list of rooms
            foreach (Room room in rooms)
            {
                // Create a new Panel for the room
                Panel roomPanel = new Panel
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Width = 300,
                    Height = 100,
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

                // Label for Spectator Count
                Label lblSpectators = new Label
                {
                    Text = $"Spectators: {room.SpectatorCount}",
                    Location = new Point(10, 30),
                    AutoSize = true
                };
                roomPanel.Controls.Add(lblSpectators);

                // Label for Availability
                Label lblAvailability = new Label
                {
                    Text = room.IsAvailable ? "Available" : "Full",
                    Location = new Point(10, 50),
                    AutoSize = true,
                    ForeColor = room.IsAvailable ? Color.Green : Color.Red
                };
                roomPanel.Controls.Add(lblAvailability);

                // Button to Join the room
                Button btnJoin = new Button
                {
                    Text = "Join",
                    Location = new Point(180, 10),
                    Size = new Size(90, 30)
                };
                btnJoin.Click += (sender, e) => { JoinRoom(room); };
                roomPanel.Controls.Add(btnJoin);

                // Button to Spectate the room with an icon
                Button btnSpectate = new Button
                {
                    Text = "Spectate",
                    Location = new Point(180, 50),
                    Size = new Size(90, 30)
                };

                // Optionally, assign a spy icon if you have one in your resources:
                // btnSpectate.Image = YourProjectNamespace.Properties.Resources.spyIcon;
                // btnSpectate.ImageAlign = ContentAlignment.MiddleLeft;
                // btnSpectate.TextAlign = ContentAlignment.MiddleRight
                //btnSpectate.Image = Client.Properties.Resources.spyIcon;
            
                btnSpectate.Click += (sender, e) => { SpectateRoom(room); };
                roomPanel.Controls.Add(btnSpectate);

                // Add the room panel to the FlowLayoutPanel
                flowLayoutPanelRooms.Controls.Add(roomPanel);
            }
        }
       

        private void JoinRoom(Room room)
        {
            MessageBox.Show(room.RoomID.ToString());
        }

        private void SpectateRoom(Room room)
        {
            room.SpectatorCount++;
        }



        private void closeButton_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(currentPlayer.Client.GetStream()))
            {
                sw.WriteLine("CLOSE");
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void createRoomButton_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            string chosenCategory = string.Empty;
            StreamWriter writer = new StreamWriter(currentPlayer.Client.GetStream()) { AutoFlush = true };
            StreamReader reader = new StreamReader(currentPlayer.Client.GetStream());

            writer.WriteLine("GET_CATEGORIES");
            string categoriesjson = reader.ReadLine();
            List<string> categories = JsonConvert.DeserializeObject<List<string>>(categoriesjson);


            CategorySelection categorySelection = new CategorySelection(categories);
            categorySelection.ShowDialog();

            if (categorySelection.DialogResult == DialogResult.OK)
            {
                chosenCategory = categorySelection.selectedCategory;
                writer.WriteLine("CREATE_ROOM");
                writer.WriteLine(chosenCategory);
            }
            else
            {
                MessageBox.Show("Category selection was canceled.");
            }
            this.Enabled = true;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
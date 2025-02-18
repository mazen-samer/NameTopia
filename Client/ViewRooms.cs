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

        }
    }
}
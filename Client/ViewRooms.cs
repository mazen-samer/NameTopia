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
    }
}
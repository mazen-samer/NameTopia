namespace SharedClasses
{
    public class GameManager
    {
        Random random = new Random();
        public Player playerOne { get; set; }
        public Player playerTwo { get; set; }
        public string GuessedWord { get; set; }
        public string category { get; set; }

        public GameManager(Player playerOne, string category)
        {
            this.playerOne = playerOne;
            LoadRandomWord();
        }
        private void LoadRandomWord()
        {
            string? baseDirectory = Directory.GetCurrentDirectory();
            string solutionDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
            string categoriesPath = Path.Combine(solutionDirectory, "Categories");
            string folderPath = Path.Combine(categoriesPath, category);

            try
            {
                string[] words;
                if (category == "Animal")
                {
                    words = File.ReadAllLines("Animal.txt");
                }
                else if (category == "Devices")
                {
                    words = File.ReadAllLines("Devices.txt");
                }
                else if (category == "Fruits")
                {
                    words = File.ReadAllLines("Fruits.txt");
                }
                else
                {
                    words = File.ReadAllLines("words.txt");
                }

                if (words.Length > 0)
                {
                    GuessedWord = words[random.Next(words.Length)].ToUpper();
                }
                else
                {
                    GuessedWord = "DEFAULT";
                }
            }
            catch (Exception ex)
            {
                GuessedWord = "DEFAULT";
            }
        }
    }
}

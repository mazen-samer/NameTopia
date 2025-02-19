namespace Client
{
    public partial class CategorySelection : Form
    {
        public string selectedCategory { get; private set; }

        public CategorySelection(List<string> categories)
        {
            InitializeComponent();
            selectedCategory = string.Empty;

            // Create radio buttons dynamically
            CreateRadioButtons(categories);
        }

        private void CreateRadioButtons(List<string> categories)
        {
            int yOffset = 10; // Start position for radio buttons

            foreach (var category in categories)
            {
                RadioButton radioButton = new RadioButton
                {
                    Text = category,
                    AutoSize = true,
                    Location = new Point(10, yOffset)
                };

                radioButton.CheckedChanged += RadioButton_CheckedChanged;
                this.Controls.Add(radioButton);
                yOffset += 25; // Move down for the next button
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                selectedCategory = radioButton.Text;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a category before proceeding.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            selectedCategory = string.Empty;
            this.Close();
        }
    }
}

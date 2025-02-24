namespace Client
{
    partial class ViewGame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            OwnerName = new Label();
            GuestName = new Label();
            playerTurn = new Label();
            guessedWord = new Label();
            keyboardPanel = new FlowLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            pagePlayerName = new Label();
            SuspendLayout();
            // 
            // OwnerName
            // 
            OwnerName.AutoSize = true;
            OwnerName.Location = new Point(125, 34);
            OwnerName.Name = "OwnerName";
            OwnerName.Size = new Size(50, 20);
            OwnerName.TabIndex = 0;
            OwnerName.Text = "label1";
            // 
            // GuestName
            // 
            GuestName.AutoSize = true;
            GuestName.Location = new Point(125, 98);
            GuestName.Name = "GuestName";
            GuestName.Size = new Size(50, 20);
            GuestName.TabIndex = 1;
            GuestName.Text = "label1";
            // 
            // playerTurn
            // 
            playerTurn.AutoSize = true;
            playerTurn.Location = new Point(687, 34);
            playerTurn.Name = "playerTurn";
            playerTurn.Size = new Size(50, 20);
            playerTurn.TabIndex = 2;
            playerTurn.Text = "label1";
            // 
            // guessedWord
            // 
            guessedWord.AutoSize = true;
            guessedWord.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guessedWord.Location = new Point(310, 151);
            guessedWord.Name = "guessedWord";
            guessedWord.Size = new Size(196, 54);
            guessedWord.TabIndex = 3;
            guessedWord.Text = "Guess Me";
            // 
            // keyboardPanel
            // 
            keyboardPanel.Location = new Point(12, 235);
            keyboardPanel.Name = "keyboardPanel";
            keyboardPanel.Size = new Size(776, 159);
            keyboardPanel.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 34);
            label1.Name = "label1";
            label1.Size = new Size(99, 20);
            label1.TabIndex = 5;
            label1.Text = "Owner Player:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 98);
            label2.Name = "label2";
            label2.Size = new Size(93, 20);
            label2.TabIndex = 6;
            label2.Text = "Guest Player:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(544, 34);
            label3.Name = "label3";
            label3.Size = new Size(137, 20);
            label3.TabIndex = 7;
            label3.Text = "Current Player Turn:";
            // 
            // pagePlayerName
            // 
            pagePlayerName.AutoSize = true;
            pagePlayerName.Location = new Point(375, 33);
            pagePlayerName.Name = "pagePlayerName";
            pagePlayerName.Size = new Size(50, 20);
            pagePlayerName.TabIndex = 8;
            pagePlayerName.Text = "label4";
            // 
            // ViewGame
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pagePlayerName);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(keyboardPanel);
            Controls.Add(guessedWord);
            Controls.Add(playerTurn);
            Controls.Add(GuestName);
            Controls.Add(OwnerName);
            Name = "ViewGame";
            Text = "ViewGame";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label OwnerName;
        private Label GuestName;
        private Label playerTurn;
        private Label guessedWord;
        private FlowLayoutPanel keyboardPanel;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label pagePlayerName;
    }
}
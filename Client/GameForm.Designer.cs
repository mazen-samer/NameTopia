namespace Client
{
    partial class GameForm
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
            pnlGame = new Panel();
            btnQuit = new Button();
            lblWord = new Label();
            lblNumOfWatchers = new Label();
            lblPlayer2Score = new Label();
            lblPlayer1Score = new Label();
            lblStatus = new Label();
            lblPlayer = new Label();
            pnlKeyboard = new Panel();
            pnlGame.SuspendLayout();
            SuspendLayout();
            // 
            // pnlGame
            // 
            pnlGame.Controls.Add(btnQuit);
            pnlGame.Controls.Add(lblWord);
            pnlGame.Controls.Add(lblNumOfWatchers);
            pnlGame.Controls.Add(lblPlayer2Score);
            pnlGame.Controls.Add(lblPlayer1Score);
            pnlGame.Controls.Add(lblStatus);
            pnlGame.Controls.Add(lblPlayer);
            pnlGame.Controls.Add(pnlKeyboard);
            pnlGame.Location = new Point(12, 12);
            pnlGame.Name = "pnlGame";
            pnlGame.Size = new Size(776, 426);
            pnlGame.TabIndex = 0;
            // 
            // btnQuit
            // 
            btnQuit.Location = new Point(656, 384);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(94, 29);
            btnQuit.TabIndex = 7;
            btnQuit.Text = "Quit";
            btnQuit.UseVisualStyleBackColor = true;
            // 
            // lblWord
            // 
            lblWord.AutoSize = true;
            lblWord.Location = new Point(128, 114);
            lblWord.Name = "lblWord";
            lblWord.Size = new Size(50, 20);
            lblWord.TabIndex = 6;
            lblWord.Text = "label1";
            // 
            // lblNumOfWatchers
            // 
            lblNumOfWatchers.AutoSize = true;
            lblNumOfWatchers.Location = new Point(503, 67);
            lblNumOfWatchers.Name = "lblNumOfWatchers";
            lblNumOfWatchers.Size = new Size(50, 20);
            lblNumOfWatchers.TabIndex = 5;
            lblNumOfWatchers.Text = "label1";
            // 
            // lblPlayer2Score
            // 
            lblPlayer2Score.AutoSize = true;
            lblPlayer2Score.Location = new Point(503, 23);
            lblPlayer2Score.Name = "lblPlayer2Score";
            lblPlayer2Score.Size = new Size(50, 20);
            lblPlayer2Score.TabIndex = 4;
            lblPlayer2Score.Text = "label1";
            // 
            // lblPlayer1Score
            // 
            lblPlayer1Score.AutoSize = true;
            lblPlayer1Score.Location = new Point(604, 114);
            lblPlayer1Score.Name = "lblPlayer1Score";
            lblPlayer1Score.Size = new Size(50, 20);
            lblPlayer1Score.TabIndex = 3;
            lblPlayer1Score.Text = "label3";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(604, 67);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 20);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "label2";
            // 
            // lblPlayer
            // 
            lblPlayer.AutoSize = true;
            lblPlayer.Location = new Point(604, 23);
            lblPlayer.Name = "lblPlayer";
            lblPlayer.Size = new Size(50, 20);
            lblPlayer.TabIndex = 1;
            lblPlayer.Text = "label1";
            // 
            // pnlKeyboard
            // 
            pnlKeyboard.Location = new Point(26, 172);
            pnlKeyboard.Name = "pnlKeyboard";
            pnlKeyboard.Size = new Size(724, 194);
            pnlKeyboard.TabIndex = 0;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlGame);
            Name = "GameForm";
            Text = "GameForm";
            pnlGame.ResumeLayout(false);
            pnlGame.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlGame;
        private Panel pnlKeyboard;
        private Label lblPlayer1Score;
        private Label lblStatus;
        private Label lblPlayer;
        private Label lblPlayer2Score;
        private Label lblNumOfWatchers;
        private Label lblWord;
        private Button btnQuit;
    }
}
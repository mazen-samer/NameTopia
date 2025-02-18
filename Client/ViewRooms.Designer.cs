namespace Client
{
    partial class ViewRooms
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
            label1 = new Label();
            closeButton = new Button();
            listBoxRooms = new ListBox();
            btnCreateRoom = new Button();
            btnJoinRoom = new Button();
            Status = new Label();
            comboBox1 = new ComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(400, 9);
            label1.Name = "label1";
            label1.Size = new Size(178, 30);
            label1.TabIndex = 0;
            label1.Text = "Available Rooms ";
            // 
            // closeButton
            // 
            closeButton.Location = new Point(607, 307);
            closeButton.Margin = new Padding(3, 2, 3, 2);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(82, 22);
            closeButton.TabIndex = 1;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += closeButton_Click;
            // 
            // listBoxRooms
            // 
            listBoxRooms.FormattingEnabled = true;
            listBoxRooms.ItemHeight = 15;
            listBoxRooms.Location = new Point(346, 41);
            listBoxRooms.Margin = new Padding(3, 2, 3, 2);
            listBoxRooms.Name = "listBoxRooms";
            listBoxRooms.Size = new Size(328, 109);
            listBoxRooms.TabIndex = 2;
            listBoxRooms.SelectedIndexChanged += listBoxRooms_SelectedIndexChanged;
            // 
            // btnCreateRoom
            // 
            btnCreateRoom.Location = new Point(209, 174);
            btnCreateRoom.Name = "btnCreateRoom";
            btnCreateRoom.Size = new Size(120, 32);
            btnCreateRoom.TabIndex = 3;
            btnCreateRoom.Text = "Create Room";
            btnCreateRoom.UseVisualStyleBackColor = true;
            btnCreateRoom.Click += btnCreateRoom_Click;
            // 
            // btnJoinRoom
            // 
            btnJoinRoom.Location = new Point(384, 172);
            btnJoinRoom.Name = "btnJoinRoom";
            btnJoinRoom.Size = new Size(104, 34);
            btnJoinRoom.TabIndex = 5;
            btnJoinRoom.Text = "Join Room";
            btnJoinRoom.UseVisualStyleBackColor = true;
            btnJoinRoom.Click += btnJoinRoom_Click;
            // 
            // Status
            // 
            Status.AutoSize = true;
            Status.Location = new Point(24, 61);
            Status.Name = "Status";
            Status.Size = new Size(39, 15);
            Status.TabIndex = 6;
            Status.Text = "Status";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "animals", "movies" });
            comboBox1.Location = new Point(176, 61);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 7;
            // 
            // ViewRooms
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(comboBox1);
            Controls.Add(Status);
            Controls.Add(btnJoinRoom);
            Controls.Add(btnCreateRoom);
            Controls.Add(listBoxRooms);
            Controls.Add(closeButton);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "ViewRooms";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button closeButton;
        private ListBox listBoxRooms;
        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Label Status;
        private ComboBox comboBox1;
    }
}
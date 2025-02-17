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
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(327, 9);
            label1.Name = "label1";
            label1.Size = new Size(147, 38);
            label1.TabIndex = 0;
            label1.Text = "All Rooms:";
            // 
            // closeButton
            // 
            closeButton.Location = new Point(694, 409);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(94, 29);
            closeButton.TabIndex = 1;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += closeButton_Click;
            // 
            // listBoxRooms
            // 
            listBoxRooms.FormattingEnabled = true;
            listBoxRooms.Location = new Point(64, 98);
            listBoxRooms.Name = "listBoxRooms";
            listBoxRooms.Size = new Size(695, 104);
            listBoxRooms.TabIndex = 2;
            // 
            // ViewRooms
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listBoxRooms);
            Controls.Add(closeButton);
            Controls.Add(label1);
            Name = "ViewRooms";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button closeButton;
        private ListBox listBoxRooms;
    }
}
namespace Client
{
    partial class CategorySelection
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
            cancelButton = new Button();
            confirmButton = new Button();
            SuspendLayout();
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(202, 216);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(94, 29);
            cancelButton.TabIndex = 0;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // confirmButton
            // 
            confirmButton.Location = new Point(12, 216);
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new Size(94, 29);
            confirmButton.TabIndex = 1;
            confirmButton.Text = "Ok";
            confirmButton.UseVisualStyleBackColor = true;
            confirmButton.Click += OkButton_Click;
            // 
            // CategorySelection
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(308, 257);
            Controls.Add(confirmButton);
            Controls.Add(cancelButton);
            Name = "CategorySelection";
            Text = "CategorySelection";
            ResumeLayout(false);
        }

        #endregion

        private Button cancelButton;
        private Button confirmButton;
    }
}
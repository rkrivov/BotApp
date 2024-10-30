namespace BotApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelUsernme = new Label();
            labelId = new Label();
            labelIdentifier = new Label();
            SuspendLayout();
            // 
            // labelUsernme
            // 
            labelUsernme.AutoSize = true;
            labelUsernme.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            labelUsernme.Location = new Point(12, 9);
            labelUsernme.Name = "labelUsernme";
            labelUsernme.Size = new Size(65, 25);
            labelUsernme.TabIndex = 0;
            labelUsernme.Text = "label1";
            // 
            // labelId
            // 
            labelId.AutoSize = true;
            labelId.Location = new Point(12, 34);
            labelId.Name = "labelId";
            labelId.Size = new Size(23, 15);
            labelId.TabIndex = 1;
            labelId.Text = "Id :";
            // 
            // labelIdentifier
            // 
            labelIdentifier.AutoSize = true;
            labelIdentifier.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelIdentifier.Location = new Point(41, 34);
            labelIdentifier.Name = "labelIdentifier";
            labelIdentifier.Size = new Size(77, 15);
            labelIdentifier.TabIndex = 2;
            labelIdentifier.Text = "2312312312";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(963, 450);
            Controls.Add(labelIdentifier);
            Controls.Add(labelId);
            Controls.Add(labelUsernme);
            Name = "MainForm";
            Text = "Form1";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelUsernme;
        private Label labelId;
        private Label labelIdentifier;
    }
}

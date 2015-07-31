namespace DatenbankBackupTool
{
    partial class Hauptformular
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.StarteSicherungButton = new System.Windows.Forms.Button();
            this.AdresseTextBox = new System.Windows.Forms.TextBox();
            this.BenutzernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswortTextBox = new System.Windows.Forms.TextBox();
            this.DatenbankTextBox = new System.Windows.Forms.TextBox();
            this.AdresseLabel = new System.Windows.Forms.Label();
            this.BenutzernameLabel = new System.Windows.Forms.Label();
            this.PasswortLabel = new System.Windows.Forms.Label();
            this.PortLabel = new System.Windows.Forms.Label();
            this.DatenbankLabel = new System.Windows.Forms.Label();
            this.StarteWiederherstellungButton = new System.Windows.Forms.Button();
            this.PortNum = new System.Windows.Forms.NumericUpDown();
            this.DatenbanksystemLabel = new System.Windows.Forms.Label();
            this.DumpersComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PortNum)).BeginInit();
            this.SuspendLayout();
            // 
            // StarteSicherungButton
            // 
            this.StarteSicherungButton.Location = new System.Drawing.Point(15, 217);
            this.StarteSicherungButton.Name = "StarteSicherungButton";
            this.StarteSicherungButton.Size = new System.Drawing.Size(134, 23);
            this.StarteSicherungButton.TabIndex = 0;
            this.StarteSicherungButton.Text = "Sicherung Erstellen";
            this.StarteSicherungButton.UseVisualStyleBackColor = true;
            this.StarteSicherungButton.Click += new System.EventHandler(this.StarteBackup);
            // 
            // AdresseTextBox
            // 
            this.AdresseTextBox.Location = new System.Drawing.Point(135, 46);
            this.AdresseTextBox.Name = "AdresseTextBox";
            this.AdresseTextBox.Size = new System.Drawing.Size(177, 20);
            this.AdresseTextBox.TabIndex = 1;
            // 
            // BenutzernameTextBox
            // 
            this.BenutzernameTextBox.Location = new System.Drawing.Point(135, 72);
            this.BenutzernameTextBox.Name = "BenutzernameTextBox";
            this.BenutzernameTextBox.Size = new System.Drawing.Size(177, 20);
            this.BenutzernameTextBox.TabIndex = 2;
            // 
            // PasswortTextBox
            // 
            this.PasswortTextBox.Location = new System.Drawing.Point(135, 98);
            this.PasswortTextBox.Name = "PasswortTextBox";
            this.PasswortTextBox.PasswordChar = '*';
            this.PasswortTextBox.Size = new System.Drawing.Size(177, 20);
            this.PasswortTextBox.TabIndex = 3;
            // 
            // DatenbankTextBox
            // 
            this.DatenbankTextBox.Location = new System.Drawing.Point(135, 150);
            this.DatenbankTextBox.Name = "DatenbankTextBox";
            this.DatenbankTextBox.Size = new System.Drawing.Size(177, 20);
            this.DatenbankTextBox.TabIndex = 5;
            // 
            // AdresseLabel
            // 
            this.AdresseLabel.AutoSize = true;
            this.AdresseLabel.Location = new System.Drawing.Point(12, 49);
            this.AdresseLabel.Name = "AdresseLabel";
            this.AdresseLabel.Size = new System.Drawing.Size(78, 13);
            this.AdresseLabel.TabIndex = 7;
            this.AdresseLabel.Text = "Serveradresse:";
            // 
            // BenutzernameLabel
            // 
            this.BenutzernameLabel.AutoSize = true;
            this.BenutzernameLabel.Location = new System.Drawing.Point(12, 75);
            this.BenutzernameLabel.Name = "BenutzernameLabel";
            this.BenutzernameLabel.Size = new System.Drawing.Size(78, 13);
            this.BenutzernameLabel.TabIndex = 8;
            this.BenutzernameLabel.Text = "Benutzername:";
            // 
            // PasswortLabel
            // 
            this.PasswortLabel.AutoSize = true;
            this.PasswortLabel.Location = new System.Drawing.Point(12, 101);
            this.PasswortLabel.Name = "PasswortLabel";
            this.PasswortLabel.Size = new System.Drawing.Size(53, 13);
            this.PasswortLabel.TabIndex = 9;
            this.PasswortLabel.Text = "Passwort:";
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(12, 127);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(29, 13);
            this.PortLabel.TabIndex = 10;
            this.PortLabel.Text = "Port:";
            // 
            // DatenbankLabel
            // 
            this.DatenbankLabel.AutoSize = true;
            this.DatenbankLabel.Location = new System.Drawing.Point(12, 153);
            this.DatenbankLabel.Name = "DatenbankLabel";
            this.DatenbankLabel.Size = new System.Drawing.Size(63, 13);
            this.DatenbankLabel.TabIndex = 11;
            this.DatenbankLabel.Text = "Datenbank:";
            // 
            // StarteWiederherstellungButton
            // 
            this.StarteWiederherstellungButton.Location = new System.Drawing.Point(189, 217);
            this.StarteWiederherstellungButton.Name = "StarteWiederherstellungButton";
            this.StarteWiederherstellungButton.Size = new System.Drawing.Size(153, 23);
            this.StarteWiederherstellungButton.TabIndex = 13;
            this.StarteWiederherstellungButton.Text = "Sicherung zurückspielen";
            this.StarteWiederherstellungButton.UseVisualStyleBackColor = true;
            this.StarteWiederherstellungButton.Click += new System.EventHandler(this.StarteWiederherstellung);
            // 
            // PortNum
            // 
            this.PortNum.Location = new System.Drawing.Point(135, 124);
            this.PortNum.Maximum = new decimal(new int[] {
            66000,
            0,
            0,
            0});
            this.PortNum.Name = "PortNum";
            this.PortNum.Size = new System.Drawing.Size(177, 20);
            this.PortNum.TabIndex = 15;
            this.PortNum.Value = new decimal(new int[] {
            3306,
            0,
            0,
            0});
            // 
            // DatenbanksystemLabel
            // 
            this.DatenbanksystemLabel.AutoSize = true;
            this.DatenbanksystemLabel.Location = new System.Drawing.Point(12, 20);
            this.DatenbanksystemLabel.Name = "DatenbanksystemLabel";
            this.DatenbanksystemLabel.Size = new System.Drawing.Size(95, 13);
            this.DatenbanksystemLabel.TabIndex = 16;
            this.DatenbanksystemLabel.Text = "Datenbanksystem:";
            // 
            // DumpersComboBox
            // 
            this.DumpersComboBox.FormattingEnabled = true;
            this.DumpersComboBox.Location = new System.Drawing.Point(135, 17);
            this.DumpersComboBox.Name = "DumpersComboBox";
            this.DumpersComboBox.Size = new System.Drawing.Size(177, 21);
            this.DumpersComboBox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(207, 263);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 9);
            this.label1.TabIndex = 18;
            this.label1.Text = "(C) 2015 tadora - business & it-solutions";
            // 
            // Hauptformular
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 277);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DumpersComboBox);
            this.Controls.Add(this.DatenbanksystemLabel);
            this.Controls.Add(this.PortNum);
            this.Controls.Add(this.StarteWiederherstellungButton);
            this.Controls.Add(this.DatenbankLabel);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.PasswortLabel);
            this.Controls.Add(this.BenutzernameLabel);
            this.Controls.Add(this.AdresseLabel);
            this.Controls.Add(this.DatenbankTextBox);
            this.Controls.Add(this.PasswortTextBox);
            this.Controls.Add(this.BenutzernameTextBox);
            this.Controls.Add(this.AdresseTextBox);
            this.Controls.Add(this.StarteSicherungButton);
            this.Name = "Hauptformular";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Datenbank Backup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PortNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StarteSicherungButton;
        private System.Windows.Forms.TextBox AdresseTextBox;
        private System.Windows.Forms.TextBox BenutzernameTextBox;
        private System.Windows.Forms.TextBox PasswortTextBox;
        private System.Windows.Forms.TextBox DatenbankTextBox;
        private System.Windows.Forms.Label AdresseLabel;
        private System.Windows.Forms.Label BenutzernameLabel;
        private System.Windows.Forms.Label PasswortLabel;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.Label DatenbankLabel;
        private System.Windows.Forms.Button StarteWiederherstellungButton;
        private System.Windows.Forms.NumericUpDown PortNum;
        private System.Windows.Forms.Label DatenbanksystemLabel;
        private System.Windows.Forms.ComboBox DumpersComboBox;
        private System.Windows.Forms.Label label1;
    }
}


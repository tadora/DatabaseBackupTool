using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatenbankBackupTool
{
    public partial class Hauptformular : Form
    {
        public Hauptformular()
        {
            InitializeComponent();

            IList<IDatabasedumper> dumpers =
                typeof(IDatabasedumper).Assembly
                .GetTypes()
                .Where<System.Type>(x => x.GetInterface("IDatabasedumper") != null)
                .Select<System.Type, IDatabasedumper>(x => (Activator.CreateInstance(x) as IDatabasedumper))
                .ToList<IDatabasedumper>();

            DumpersComboBox.DisplayMember = "DisplayName";
            DumpersComboBox.ValueMember = "DisplayName";
            DumpersComboBox.DataSource = dumpers;
            
            AdresseTextBox.Text = Properties.Settings.Default.Server;
            BenutzernameTextBox.Text = Properties.Settings.Default.Benutzer;
            PasswortTextBox.Text = Properties.Settings.Default.Passwort;
            PortNum.Value = Properties.Settings.Default.Port;
            DatenbankTextBox.Text = Properties.Settings.Default.Datenbank;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Server = AdresseTextBox.Text;
            Properties.Settings.Default.Benutzer = BenutzernameTextBox.Text;
            Properties.Settings.Default.Passwort = PasswortTextBox.Text;
            Properties.Settings.Default.Port = (uint)PortNum.Value;
            Properties.Settings.Default.Datenbank = DatenbankTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private IDatabasedumper instanciateDumper()
        {
            IDatabasedumper dumper = (DumpersComboBox.SelectedItem as IDatabasedumper);
            if (dumper == null)
            {
                return null;
            }
            dumper.setAddress(AdresseTextBox.Text);
            dumper.setDatabase(DatenbankTextBox.Text);
            dumper.setPassword(PasswortTextBox.Text);
            dumper.setPort((uint)PortNum.Value);
            dumper.setUsername(BenutzernameTextBox.Text);

            return dumper;
        }

        private void StarteBackup(object sender, EventArgs e)
        {
            String filename = WähleDateiAus(new SaveFileDialog());
            if (filename == null)
            {
                return;
            }

            IDatabasedumper dumper = instanciateDumper();
            if(dumper != null) {
                try
                {
                    dumper.export(filename);
                    MessageBox.Show("Backup wurde erstellt");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Backup: " + ex.Message);
                }
            }
        }
    
        private void StarteWiederherstellung(object sender, EventArgs e)
        {
            String filename = WähleDateiAus(new OpenFileDialog());
            if (filename == null)
            {
                return;
            }

            IDatabasedumper dumper = instanciateDumper();
            if (dumper != null)
            {
                try
                {
                    dumper.import(filename);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler bei Wiederherstellung: " + ex.Message);
                }
            }
        }

        private String WähleDateiAus(FileDialog dialogToUse)
        {
            DialogResult dialogErgebnis = dialogToUse.ShowDialog();
            if (dialogErgebnis != DialogResult.OK)
            {
                return null;
            }
            
            return dialogToUse.FileName;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

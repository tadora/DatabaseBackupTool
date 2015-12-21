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
    /// <summary>
    /// Das Hauptformular
    /// </summary>
    public partial class Hauptformular : Form
    {
        /// <summary>
        /// Der Konstruktor des Hauptformulars befüllt die Liste
        /// der unterstützten Datenbankserver und lädt die zuletzt verwendeten
        /// Servereinstellungen aus den Benutzereinstellungen in die Textboxen
        /// </summary>
        public Hauptformular()
        {
            InitializeComponent();

            //Liste der Klassen, die IDatabasedumper implementieren
            IList<IDatabasedumper> dumpers =
                typeof(IDatabasedumper).Assembly //Von der Assembly, in der IDatabasedumper ist
                .GetTypes() //Hole alle Datentypen
                .Where<System.Type>(x => x.GetInterface("IDatabasedumper") != null) //die IDatabasedumper implementieren
                .Select<System.Type, IDatabasedumper>(x => (Activator.CreateInstance(x) as IDatabasedumper)) //erstelle eine Instance des des Datentyps
                .ToList<IDatabasedumper>(); //füge alle gefundenen zu einer Liste zusammen.

            //Die Eigenschaft DisplayName ist der anzuzeigende Text in der Combobox für unterstützte Datenbankserver
            DumpersComboBox.DisplayMember = "DisplayName";
            //DisplayName ist gleichzeitig der eindeutige Erkennung
            DumpersComboBox.ValueMember = "DisplayName";
            //Weise die Liste der IDatabasedumper als Datenquelle der Combobox zu
            DumpersComboBox.DataSource = dumpers;
            
            //Lade die Einstellungen in die Textboxen
            AdresseTextBox.Text = Properties.Settings.Default.Server;
            BenutzernameTextBox.Text = Properties.Settings.Default.Benutzer;
            PasswortTextBox.Text = Properties.Settings.Default.Passwort;
            PortNum.Value = Properties.Settings.Default.Port;
            DatenbankTextBox.Text = Properties.Settings.Default.Datenbank;
        }

        /// <summary>
        /// Beim Schließen des Formular werden die Werte aus den Textboxen in die Einstellungen geschrieben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Server = AdresseTextBox.Text;
            Properties.Settings.Default.Benutzer = BenutzernameTextBox.Text;
            Properties.Settings.Default.Passwort = PasswortTextBox.Text;
            Properties.Settings.Default.Port = (uint)PortNum.Value;
            Properties.Settings.Default.Datenbank = DatenbankTextBox.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Weist dem gerade ausgewählten IDatabasedumper aus DumpersComboBox die
        /// Servereinstellungen zu und gibt den IDatabasedumper zurück
        /// </summary>
        /// <returns></returns>
        private IDatabasedumper ErstelleEingestelltenIDatabasedumper()
        {
            //Hole den ausgewählten IDatabasedumper
            IDatabasedumper dumper = (DumpersComboBox.SelectedItem as IDatabasedumper);
            //Wenn keiner ausgewählt wurde gibt einfach NULL zurück.
            if (dumper == null)
            {
                return null;
            }
            //Stelle den IDatabasedumper ein
            dumper.setAddress(AdresseTextBox.Text);
            dumper.setDatabase(DatenbankTextBox.Text);
            dumper.setPassword(PasswortTextBox.Text);
            dumper.setPort((uint)PortNum.Value);
            dumper.setUsername(BenutzernameTextBox.Text);

            //Gibt den IDatabasedumper zurück
            return dumper;
        }

        /// <summary>
        /// Erstellt ein Dump der Datenbank mit den in den Textboxen angegebenen Serverdaten
        /// und dem IDatabasedumper aus der DumpersComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StarteBackup(object sender, EventArgs e)
        {
            //Fordere den Nutzer auf, auszuwählen wo die Datei abgespeichert werden soll.
            String filename = WähleDateiAus(new SaveFileDialog());
            if (filename == null)
            {
                return;
            }

            IDatabasedumper dumper = ErstelleEingestelltenIDatabasedumper();
            //Prüfe ob wir einen gültigen IDatabasedumper gekriegt haben
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

            IDatabasedumper dumper = ErstelleEingestelltenIDatabasedumper();
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

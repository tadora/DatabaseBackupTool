using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatenbankBackupTool
{
    /// <summary>
    /// Die Startklasse für das Programm.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Wenn keine Programmparameter angegeben wurden soll regulär die Grafische OPberfläche starten
            if (args.Length == 0)
            {
                //Standardaufrufe für das starten des grafischen Programms
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Hauptformular());
            }
            else
            {
                
            }
        }
    }
}

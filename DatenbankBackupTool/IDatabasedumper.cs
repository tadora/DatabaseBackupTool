using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenbankBackupTool
{
    /// <summary>
    /// Die Schnittstelle IDatabasedumper stellt den Zugriff auf die Datenbank-Implementationen
    /// zur Verfügung. Alle Datenbank-Systemimplementationen müssen diese Schnittstelle implementieren.
    /// </summary>
    public interface IDatabasedumper
    {
        /// <summary>
        /// Legt die Netzwerkadresse des zu verwendenden Datenbankservers fest (ohne Portangabe).
        /// </summary>
        /// <param name="address"></param>
        void setAddress(string address);

        /// <summary>
        /// Legt den für den Login zu verwendende Benutzernamen fest.
        /// </summary>
        /// <param name="username"></param>
        void setUsername(string username);

        /// <summary>
        /// Legt den Port für den Datenbankserver fest.
        /// </summary>
        /// <param name="port"></param>
        void setPort(uint port);

        /// <summary>
        /// Legt das für den Login zu verwendende Passwort fest.
        /// </summary>
        /// <param name="password"></param>
        void setPassword(string password);

        /// <summary>
        /// Legt fest, mit welcher Datenbank auf dem Server gearbeitet werden soll.
        /// </summary>
        /// <param name="database"></param>
        void setDatabase(string database);

        /// <summary>
        /// Liest einen SQL-Dump vom angegebenen Dateinamen und führt diesen gegen die
        /// angegebene Datenbank aus.
        /// </summary>
        /// <param name="filename">Kompletter Pfad zur Datei</param>
        /// <returns>true bei Erfolg, fals beim Auftritt eines Fehlers</returns>
        bool import(string filename);

        /// <summary>
        /// Liest einen SQL-Dump vom angegebenen Stream und führt diesen gegen die 
        /// angegebene Datenbank aus
        /// </summary>
        /// <param name="stream">Der Stream, welcher den Dump zur Verfügung stellt.</param>
        /// <returns>true bei Erfolg, fals beim Auftritt eines Fehlers</returns>
        bool import(Stream stream);

        /// <summary>
        /// Schreibt den Inhalt der Datenbank in die angegebenen Datei als SQL-Dump.
        /// Wenn die Datei nicht existiert wird sie angelegt.
        /// Existiert sie wird sie überschrieben!
        /// </summary>
        /// <param name="filename">Kompletter Pfad zur Datei</param>
        /// <returns>true bei Erfolg, fals beim Auftritt eines Fehlers</returns>
        bool export(string filename);

        /// <summary>
        /// Schreibt den Inhalt der Datenbank in den angegebenen Stream als SQL-Dump.
        /// </summary>
        /// <param name="stream">Der Stream, in den die Daten geschrieben werden sollen</param>
        /// <returns>true bei Erfolg, fals beim Auftritt eines Fehlers</returns>
        bool export(Stream stream);

        /// <summary>
        /// Gibt den Anzeigenamen der Implementierenden Klasse zurück.
        /// </summary>
        String DisplayName { get; }
    }
}

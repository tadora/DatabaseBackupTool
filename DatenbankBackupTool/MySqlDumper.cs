using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DatenbankBackupTool
{
    /// <summary>
    /// Der MySqlDumper implementiert die Dump- und Importfunktionen für MySql-Datenbankserver Version 5
    /// </summary>
    public class MySqlDumper : IDatabasedumper
    {
        private MySqlConnectionStringBuilder connectionStringBuilder;

        /// <summary>
        /// Erstellt eine neue MySqlDumper-Instanz
        /// </summary>
        public MySqlDumper()
        {
            connectionStringBuilder = new MySqlConnectionStringBuilder();
        }

        /// <inheritdoc/>
        public void setAddress(string address)
        {
            connectionStringBuilder.Server = address;
        }

        /// <inheritdoc/>
        public void setUsername(string username)
        {
            connectionStringBuilder.UserID = username;
        }

        /// <inheritdoc/>
        public void setPort(uint port)
        {
            connectionStringBuilder.Port = port;
        }

        /// <inheritdoc/>
        public void setPassword(string password)
        {
            connectionStringBuilder.Password = password;
        }

        /// <inheritdoc/>
        public void setDatabase(string database)
        {
            connectionStringBuilder.Database = database;
        }

        /// <inheritdoc/>
        public bool export(string filename)
        {
            Stream fileStream = File.OpenWrite(filename);
            return export(fileStream);
        }

        /// <inheritdoc/>
        public bool export(Stream stream)
        {
            //Erstelle eine neue MySqlVerbindung
            using (MySqlConnection conn = new MySqlConnection(connectionStringBuilder.ToString()))
            {
                //Erstelle ein neues MySql-Kommando
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open(); //Öffnet die Verbindung zum Server
                    cmd.Connection = conn; //Weist dem Kommando die eben geöffnete MySql-Verbindung zu
                    cmd.CommandText = "SHOW TABLES"; //Definiere den auszuführenden Sql-Text
                    MySqlDataReader reader = cmd.ExecuteReader(); //Führt das Sql aus und gibt einen Datenleser zurück

                    List<String> tableNames = new List<String>(); //Definiert eine Liste, worin alle Tabellennamen der Datenbank gespeichert werden

                    //Befülle die Liste der Tabellennamen
                    while (reader.Read())
                    {
                        tableNames.Add(reader.GetString(0));
                    }
                    reader.Close(); //Datenleser nach getaner Arbeit schließen
                    List<String> Constraints = new List<String>(); //Eine Liste der Abhängigkeiten zwischen den Tabellen
                    /*
                     * Dieser Reguläre Ausdruck dient dazu die Constraints aus den CREATE TABLE Anweisungen herauszufiltern.
                     * Die Constraints müssen nämlich definiert werden, nachdem alle Tabellen angelegt wurden, um ein sicheres Dumpen
                     * und späteres Wiederherstellen zu gewähren.
                     * Beispiel:
                     * 1 CREATE TABLE `ARTIKEL_ZU_SHOPKATEGORIE` (
                     * 2   `ARTIKEL_ID` int(11) NOT NULL,
                     * 3   `SHOPKATEGORIE_ID` int(11) NOT NULL,
                     * 4   PRIMARY KEY (`SHOPKATEGORIE_ID`,`ARTIKEL_ID`),
                     * 5   KEY `SHOPKATEGORIE_ID` (`SHOPKATEGORIE_ID`),
                     * 6   KEY `ARTIKEL_ID` (`ARTIKEL_ID`),
                     * 7   CONSTRAINT `ARTIKEL_ZU_SHOPKATEGORIE_ibfk_2` FOREIGN KEY (`SHOPKATEGORIE_ID`) REFERENCES `SHOPKATEGORIEN` (`REC_ID`) ON DELETE CASCADE,
                     * 8   CONSTRAINT `ARTIKEL_ZU_SHOPKATEGORIE_ibfk_1` FOREIGN KEY (`ARTIKEL_ID`) REFERENCES `ARTIKEL` (`REC_ID`) ON DELETE CASCADE
                     * 9 ) ENGINE=InnoDB DEFAULT CHARSET=utf8;
                     * 
                     * Merke:   Die doppelten Backslashes müssen als einzelner gelesen werden.
                     *          Da der String selber Escaped wird müssen Backslashes, die zum escapen
                     *          verwendet für den RegEx verwendet werden sollen, selber auch escaped werden.
                     * 
                     * 1. ",\\n\\s*"            Ein Constraint fängt immer mit einem Komma (,) aus der Vorherigen Zeile und einem Zeilenumbruch (\\n)
                     *                          gefolgt von einer beliebigen Anzahl Leerzeichen (\\s*) an. (Ende Zeile 6 bis Anfang Zeile 7)
                     * 2. "CONSTRAINT "         Dann kommt das Schlüsselwort CONSTRAINT mit einem Leerzeichen dahinter (Anfang Zeile 7)
                     * 3. "`([^`]*)`"           Nach dem Schlüsselwort folgt der Name des Constraint, welches in Accent Grave (`) eingeschlossen ist.
                     *                          Der Name wird durch "[^`]" lies - NICHT Accent Grave, "*" lies - beliebig oft erfasst. 
                     *                          Da der Ausdruck in runden Klammern gefasst ist wird das erfasste als erste Gruppe gespeichert. (Zeile 7)
                     * 4. " FOREIGN KEY "       Nun kommt wieder eine normale Zeichenfolge (Zeile 7)
                     * 5. \\(`([^`]*)`\\)       Der Name des Fremdschlüssels. Da hier die runde Klammer nicht als RegEx-Erfassungsgruppe dient, sondern
                     *                          einfach als das Zeichen runde Klammer muss diese mit "\\" escaped werden.
                     *                          Ansonsten gilt das gleiche wie bei 3. (Zeile 7)
                     * 6. " REFERENCES "        Wieder eine normale Zeichenfolge (Zeile 7)
                     * 7. "`([^`]*)`"           Der Name der referenzierten Tabelle (siehe 3.) (Zeile 7)
                     * 8. " \\(`([^`]*)`\\) "   Der Name des Primärschlüssel in der Referenzierten Tabelle wie bei 5. (Zeile 7)
                     * 9. "([^,\\n]*)"          [^,\n] lies - nicht Komma und nicht Zeilenumbruch beliebig oft. Dadurch werden alle restlichen Zeichen
                     *                          in der Zeile erfasst. Es muss auf Komma und Zeilenumbruch geprüft werden. Vergleiche Zeilenende 7 und 8
                     */
                    Regex ausdruck = new Regex(",\\n\\s*CONSTRAINT `([^`]*)` FOREIGN KEY \\(`([^`]*)`\\) REFERENCES `([^`]*)` \\(`([^`]*)`\\)([^,\\n]*)", RegexOptions.Multiline);
                    String setNames = "/*!40101 SET NAMES utf8 */;\n\n";    //Encoding der Tabellen und Spaltennamen
                    byte[] setNamesBytes = UTF8Encoding.UTF8.GetBytes(setNames);    //Wandler die Encoding-Angabe in UTF8-Bytes um
                    stream.Write(setNamesBytes, 0, setNamesBytes.Length);   //Und schreibe das ganze in den Stream

                    //Nun werden alle Tabellennamen durchlaufen
                    foreach (String tableName in tableNames)
                    {
                        //MySql-Befehl der das komplette CREATE TABLE-Kommando für eine Tabelle zurückgibt
                        cmd.CommandText = "SHOW CREATE TABLE " + tableName; 
                        //Sql ausführen
                        reader = cmd.ExecuteReader();   
                        //Daten vom Server einlesen
                        reader.Read();  
                        //Übernimmt das CREATE TABLE-Kommando
                        String createCommandString = reader.GetString(1) + ";\n\n\n"; 
                        //Hier wird der RegEx auf das CREATE TABLE angewandt und 
                        //übereinstimmungen (also die Constraints) in matches abgespeichert.
                        MatchCollection matches = ausdruck.Matches(createCommandString);
                        //Nun werden mit dem RegEx die Constraints aus dem CREATE TABLE Befehl entfernt.
                        ausdruck.Replace(createCommandString, "");

                        //Nun durchlaufe alle gefundenen Constraints im Create Table Befehl
                        foreach (Match match in matches)
                        {
                            //In die Constraints-Liste werden jetzt neue ADD CONSTRAINT-Befehle gespeichert,
                            //die denen aus dem CREATE TABLE entsprechen.
                            //Dabei werden die Originalnamen aus der Datenbank wiederverwendet. Denn in match.Groups sind alle erfassten Zeichen
                            //die von runden Klammern im RegEx umgeben waren enthalten. Dabei ist das erste Klammernpaar match.Groups[1],
                            //das zweite match.Groups[2] usw.
                            Constraints.Add(
                                "ALTER TABLE " + tableName + 
                                " ADD CONSTRAINT " + match.Groups[1].Value +
                                " FOREIGN KEY (" + match.Groups[2].Value + ") REFERENCES " + match.Groups[3] + " (" + match.Groups[4] + ")" +
                                match.Groups[5].Value + ";\n"
                            );
                        }
                        //Nun werden mit dem RegEx die Constraints aus dem CREATE TABLE Befehl entfernt.
                        createCommandString = ausdruck.Replace(createCommandString, "");
                        //Der komplette SQL-Befehl wird nun wieder in UTF8-Bytes umgewandelt
                        byte[] createCommand = UTF8Encoding.UTF8.GetBytes(createCommandString);
                        //und in den Stream geschrieben
                        stream.Write(createCommand,0, createCommand.Length);
                        //Nach getaner Arbeit dem MySqlDatareader wieder schließen.
                        reader.Close();

                        //Als nächstes sind die Daten in den Tabellen dran.

                        //Dafür müssen wie die Anzahl der Datensätze in der Tabelle wissen.
                        cmd.CommandText = "SELECT count(*) FROM " + tableName;
                        reader = cmd.ExecuteReader();
                        reader.Read();
                        UInt64 rows = reader.GetUInt64(0); //Anzahl Datensätze wird in rows gespeichert.
                        reader.Close();

                        //Nur was tun, wenn auch Datensätze vorhanden.
                        if (rows > 0)
                        {
                            //Hier wird der Anfang des INSERT INTO-Befehls in UTF8-Bytes umgewandelt
                            byte[] insertCommand = UTF8Encoding.UTF8.GetBytes("INSERT INTO " + tableName + " VALUES \n");
                            //Zwischenspeicher, der angibt wieviele Datensätze bereits verarbeitet wurden.
                            UInt64 processedRows = 0;

                            //Eine Liste der Datensätze
                            List<String> rowList = new List<String>();
                            //Jetzt arbeite, solange die Anzahl der verarbeiteten Datensätze kleiner den gezählten ist. 
                            while (processedRows < rows)
                            {
                                //Schreibe den INSERT INTO Befehl schon mal in den Stream.
                                stream.Write(insertCommand, 0, insertCommand.Length);
                                //Hole die Datensätze von der Tabelle, angefangen bei Datensatznummer processedRows, maximal 1000 Stück.
                                cmd.CommandText = "SELECT * FROM " + tableName + " LIMIT " + processedRows + ", 1000";
                                //Sql Ausführen
                                reader = cmd.ExecuteReader();
                                
                                //Arbeite, solange wir Daten vom Server auslesen können
                                while (reader.Read())
                                {
                                    //Liste der Werte des aktuellen Datensatzes
                                    List<String> rowValues = new List<String>();
                                    //Durchlaufe alle Spalten im Datensatz
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        //Erst mal schauen, obs ein NULL-Wert ist.
                                        if (reader.IsDBNull(i))
                                        {
                                            rowValues.Add("NULL");
                                        }
                                        else
                                        {
                                            //Entscheide was zu tun ist anhand des MySql-Datentypennamen
                                            switch (reader.GetDataTypeName(i))
                                            {   
                                                case "SMALLINT":
                                                    rowValues.Add(reader.GetInt16(i).ToString());
                                                    break;
                                                //INT und TINYINT werden als 32 bit Integer ausgelesen.
                                                case "INT":
                                                case "TINYINT":
                                                    rowValues.Add(reader.GetInt32(i).ToString());
                                                    break;
                                                //BIGINT hingegen als 64 but Integer
                                                case "BIGINT":
                                                    rowValues.Add(reader.GetInt64(i).ToString());
                                                    break;
                                                //Die folgenden Datentypen werden als C# String ausgelesen
                                                //Dabei werden Anführungszeichen durch escapedte Anführungszeichen ersetzt.
                                                // \" (Anführungszeichen in den Daten) -> \\\" (ersetzt durch ein escapedtes Anführungszeichen
                                                //Und der ganze String wiederum in Anführungszeichen umfasst.
                                                case "VARCHAR":
                                                case "CHAR(36)":
                                                    rowValues.Add("\"" + reader.GetString(i).Replace("\"","\\\"") + "\"");
                                                    break;
                                                //DECIMAL wird als float ausgelesen und dann als String in die Liste der Spaltenwerte geschrieben.
                                                case "DECIMAL":
                                                    rowValues.Add(reader.GetFloat(i).ToString(System.Globalization.CultureInfo.InvariantCulture));
                                                    break;
                                                case "DATETIME":
                                                case "DATE":
                                                    rowValues.Add("\"" + reader.GetDateTime(i).ToString("yyyy-MM-dd HH:mm:ss").Replace("\"", "\\\"") + "\"");
                                                    break;
                                                //BLOBs werden Byte für Byte ausgelesen.
                                                //Dann werden die Bytes mit ".ToHex()" in eine Hexadezimale Darstellung
                                                //umgewandelt und als Hexstring in die Spaltenwerte aufgenommen
                                                case "BLOB":
                                                    long bufferSize = reader.GetBytes(i, 0, null, 0, 0);
                                                    byte[] daten = new byte[bufferSize];
                                                    reader.GetBytes(i, 0, daten, 0, (int)bufferSize);
                                                    rowValues.Add("0x"+daten.ToHex());
                                                    break;
                                                //Der default ist nur dafür da, dass das Programm nicht abstürzt, wenn
                                                //auf eine MySQL-Datentyp getroffen wird, der noch nicht bekannt ist.
                                                default:
                                                    String datatypeName = reader.GetDataTypeName(i);
                                                    throw new ArgumentOutOfRangeException("MySql Datentyp '" + datatypeName + "' ist diesem Tool nicht bekannt");
                                            }
                                        }
                                    }
                                    //Nun werden alle Spaltenwerte mit String.Join mit Kommas verbunden, der
                                    //gesamte Werte-String von Klammern umfasst und in die Datensatzliste aufgenommen.
                                    rowList.Add("("+String.Join(",", rowValues)+")");
                                }
                                //Nach getaner Arbeit reader schließen
                                reader.Close();
                                //Umwandlung des Wertestrings in UTF8-bytes mit etwas Formatierung (Zeilenumbrüche {\n} und Tabs {\t}
                                byte[] insertCommandValuePart = UTF8Encoding.UTF8.GetBytes(String.Join(",\n\t", rowList) + ";\n\n\n");
                                //Schreibe die ausgelesenen Datensätze in den Stream
                                stream.Write(insertCommandValuePart, 0, insertCommandValuePart.Length);
                                //Zähler für verarbeitete Datensätze um 1000 erhöhen.
                                processedRows += 1000;
                                //Die Datensatzliste mit den 1000 Datensatzstrings kann nun geleert werden, um Ram zu sparen und
                                //damit keine Datensätze doppelt in den Stream geschrieben werden.
                                rowList.Clear();
                            }
                        }
                    }
                    //Nachdem alle CREATE TABLE und INSERT INTOs in den Stream geschrieben wurden
                    //Werden jetzt die CONSTRAINTs am Ende nachgefügt.
                    foreach (String constraint in Constraints)
                    {
                        byte[] byteConstraint = UTF8Encoding.UTF8.GetBytes(constraint);
                        stream.Write(byteConstraint, 0, byteConstraint.Length);
                    }
                }
            }
            stream.Close();
            return true;
        }

        /// <inheritdoc/>
        public bool import(string filename)
        {
            //Filestream mit dem übergebenen Dateinamen erstellen
            Stream fileStream = File.OpenRead(filename);
            //und den erstellten Stream an die Stream-Version der import-Methode weitergeben
            return import(fileStream);
        }

        /// <inheritdoc/>
        public bool import(System.IO.Stream stream)
        {
            //Encoding definieren
            connectionStringBuilder.CharacterSet = "utf8";
            //MySqlVerbindung erstellen
            using (MySqlConnection conn = new MySqlConnection(connectionStringBuilder.ToString()))
            {
                //MySqlKommando erstellen
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //Verbindung öffnen
                    conn.Open();
                    string query = string.Empty;
                    //neues MySqlScript erstellen
                    MySqlScript script = new MySqlScript(conn);
                    //einen Streamreader aus dem erhaltenen Stream erstellen
                    StreamReader streamReader = new StreamReader(stream);
                    //Sql aus dem StreamReader auslesen und dem
                    //Script übergeben
                    script.Query = streamReader.ReadToEnd();                
                    
                    //Script ausführen
                    script.Execute();
                }
            }
            return true;
        }

        ///<inheritdoc />
        public string DisplayName
        {
            get { return "MySql 5"; }
        }
    }
}

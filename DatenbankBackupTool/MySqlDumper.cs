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
    public class MySqlDumper : IDatabasedumper
    {
        private MySqlConnectionStringBuilder connectionStringBuilder;

        public MySqlDumper()
        {
            connectionStringBuilder = new MySqlConnectionStringBuilder();
        }

        public void setAddress(string address)
        {
            connectionStringBuilder.Server = address;
        }

        public void setUsername(string username)
        {
            connectionStringBuilder.UserID = username;
        }

        public void setPort(uint port)
        {
            connectionStringBuilder.Port = port;
        }

        public void setPassword(string password)
        {
            connectionStringBuilder.Password = password;
        }

        public void setDatabase(string database)
        {
            connectionStringBuilder.Database = database;
        }

        public bool export(string filename)
        {
            Stream fileStream = File.OpenWrite(filename);
            return export(fileStream);
        }

        public bool export(Stream stream)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStringBuilder.ToString()))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "SHOW TABLES";
                    MySqlDataReader reader = cmd.ExecuteReader();

                    List<String> tableNames = new List<String>();

                    while (reader.Read())
                    {
                        tableNames.Add(reader.GetString(0));
                    }
                    reader.Close();
                    List<String> Constraints = new List<String>();
                    Regex ausdruck = new Regex(",\\n\\s*CONSTRAINT `([^`]*)` FOREIGN KEY \\(`([^`]*)`\\) REFERENCES `([^`]*)` \\(`([^`]*)`\\)([^,\n]*)", RegexOptions.Multiline);
                    String setNames = "/*!40101 SET NAMES utf8 */;\n\n";
                    byte[] setNamesBytes = UTF8Encoding.UTF8.GetBytes(setNames);
                    stream.Write(setNamesBytes, 0, setNamesBytes.Length);

                    foreach (String tableName in tableNames)
                    {
                        cmd.CommandText = "SHOW CREATE TABLE " + tableName;
                        reader = cmd.ExecuteReader();
                        reader.Read();
                        String createCommandString = reader.GetString(1) + ";\n\n\n";
                        MatchCollection matches = ausdruck.Matches(createCommandString);
                        ausdruck.Replace(createCommandString, "");
                        foreach (Match match in matches)
                        {
                            Constraints.Add(
                                "ALTER TABLE " + tableName + 
                                " ADD CONSTRAINT " + match.Groups[1].Value +
                                " FOREIGN KEY (" + match.Groups[2].Value + ") REFERENCES " + match.Groups[3] + " (" + match.Groups[4] + ")" +
                                match.Groups[5].Value + ";\n"
                            );
                        }
                        createCommandString = ausdruck.Replace(createCommandString, "");
                        byte[] createCommand = UTF8Encoding.UTF8.GetBytes(createCommandString);
                        stream.Write(createCommand,0, createCommand.Length);
                        reader.Close();

                        cmd.CommandText = "SELECT count(*) FROM " + tableName;
                        reader = cmd.ExecuteReader();
                        reader.Read();
                        UInt64 rows = reader.GetUInt64(0);
                        reader.Close();


                        if (rows > 0)
                        {
                            byte[] insertCommand = UTF8Encoding.UTF8.GetBytes("INSERT INTO " + tableName + " VALUES \n");
                            UInt64 processedRows = 0;

                            List<String> rowList = new List<String>();
                            while (processedRows < rows)
                            {
                                stream.Write(insertCommand, 0, insertCommand.Length);
                                cmd.CommandText = "SELECT * FROM " + tableName + " LIMIT " + processedRows + ", 1000";
                                reader = cmd.ExecuteReader();
                                
                                while (reader.Read())
                                {
                                    List<String> rowValues = new List<String>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        if (reader.IsDBNull(i))
                                        {
                                            rowValues.Add("NULL");
                                        }
                                        else
                                        {
                                            switch (reader.GetDataTypeName(i))
                                            {
                                                case "INT":
                                                case "TINYINT":
                                                    rowValues.Add(reader.GetInt32(i).ToString());
                                                    break;
                                                case "BIGINT":
                                                    rowValues.Add(reader.GetInt64(i).ToString());
                                                    break;
                                                case "VARCHAR":
                                                case "CHAR(36)":
                                                case "DATETIME":
                                                case "DATE":
                                                    rowValues.Add("\"" + reader.GetString(i).Replace("\"","\\\"") + "\"");
                                                    break;
                                                case "DECIMAL":
                                                    rowValues.Add(reader.GetFloat(i).ToString(System.Globalization.CultureInfo.InvariantCulture));
                                                    break;
                                                case "BLOB":
                                                    long bufferSize = reader.GetBytes(i, 0, null, 0, 0);
                                                    byte[] daten = new byte[bufferSize];
                                                    reader.GetBytes(i, 0, daten, 0, (int)bufferSize);
                                                    rowValues.Add("0x"+daten.ToHex());
                                                    break;
                                                default:
                                                    String datatypeName = reader.GetDataTypeName(i);
                                                    break;
                                            }
                                        }
                                    }
                                    rowList.Add("("+String.Join(",", rowValues)+")");
                                }
                                reader.Close();
                                byte[] insertCommandValuePart = UTF8Encoding.UTF8.GetBytes(String.Join(",\n\t", rowList) + ";\n\n\n");
                                stream.Write(insertCommandValuePart, 0, insertCommandValuePart.Length);
                                processedRows += 1000;
                                rowList.Clear();
                            }
                        }
                    }
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

        public bool import(string filename)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStringBuilder.ToString()))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    Stream fileStream = File.OpenRead(filename);
                    return import(fileStream);
                }
            }
        }

        public bool import(System.IO.Stream stream)
        {
            connectionStringBuilder.CharacterSet = "utf8";
            using (MySqlConnection conn = new MySqlConnection(connectionStringBuilder.ToString()))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();
                    string query = string.Empty;
                    MySqlScript script = new MySqlScript(conn);
                    StreamReader streamReader = new StreamReader(stream);
                    script.Query = streamReader.ReadToEnd();                
                    
                    script.Execute();
                }
            }
            return true;
        }

        public string DisplayName
        {
            get { return "MySql 5"; }
        }
    }
}

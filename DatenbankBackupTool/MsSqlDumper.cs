using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DatenbankBackupTool
{
    class MsSqlDumper : IDatabasedumper
    {
        private SqlConnectionStringBuilder connectionStringBuilder;

        public MsSqlDumper()
        {
            connectionStringBuilder = new SqlConnectionStringBuilder();
        }

        public void setAddress(string address)
        {
            connectionStringBuilder.DataSource = address;
        }

        public void setUsername(string username)
        {
            connectionStringBuilder.UserID = username;
        }

        public void setPort(uint port)
        {
            connectionStringBuilder.DataSource = connectionStringBuilder.DataSource.Split(new Char[] { ',' })[0] + "," + port.ToString();
        }

        public void setPassword(string password)
        {
            connectionStringBuilder.Password = password;
        }

        public void setDatabase(string database)
        {
            connectionStringBuilder.InitialCatalog = database;
        }

        public bool import(string filename)
        {
            Server srv = new Server(connectionStringBuilder.ToString());
            Database db = srv.Databases[connectionStringBuilder.InitialCatalog];
            Restore restore = new Restore();

            restore.Action = RestoreActionType.Database;
            restore.Database = connectionStringBuilder.InitialCatalog;

            BackupDeviceItem bdi = new BackupDeviceItem(filename, DeviceType.File);
            restore.Devices.Add(bdi);

            restore.SqlRestore(srv);

            return true;
        }

        public bool import(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public bool export(string filename)
        {
            SqlConnection sqlConn = new SqlConnection(connectionStringBuilder.ToString());
            ServerConnection srvConn = new ServerConnection(sqlConn);

            Server srv = new Server(srvConn);
            Database db = srv.Databases[connectionStringBuilder.InitialCatalog];
            Backup bak = new Backup();

            bak.Action = BackupActionType.Database;
            bak.BackupSetDescription = "Datenbankbackup";
            bak.BackupSetName = "Datenbankbackup";
            bak.Database = connectionStringBuilder.InitialCatalog;

            BackupDeviceItem bdi = new BackupDeviceItem(filename, DeviceType.File);
            bak.Devices.Add(bdi);
            bak.Incremental = false;

            bak.SqlBackup(srv);

            return true;
        }

        public bool export(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public string DisplayName
        {
            get { return "Sql Server 2008 und neuer"; }
        }
    }
}

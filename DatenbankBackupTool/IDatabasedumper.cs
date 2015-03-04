using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenbankBackupTool
{
    public interface IDatabasedumper
    {
        void setAddress(string address);
        void setUsername(string username);
        void setPort(uint port);
        void setPassword(string password);
        void setDatabase(string database);

        bool import(string filename);
        bool import(Stream stream);

        bool export(string filename);
        bool export(Stream stream);

        String DisplayName { get; }
    }
}

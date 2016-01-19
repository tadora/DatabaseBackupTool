using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenbankBackupTool
{
    public class IDatabasedumperFactory
    {
        private IDatabasedumper dumperType;

        public IDatabasedumperFactory(IDatabasedumper dumperType)
        {
            this.dumperType = dumperType;
        }

        public IDatabasedumper getInstance()
        {
            return dumperType;
        }

        public String DisplayName
        {
            get
            {
                return dumperType.DisplayName;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ESD_Testjig
{
   public class DBAccess
    {
       public static string ConnectionString;
        public static string ConnectionStringMaster;

        public static string getconstring()
        {
            return ConnectionString;
           // return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        }
        public static string getconstringMaster()
        {
            return ConnectionStringMaster;
           // return ConfigurationManager.ConnectionStrings["ConnectionStringMaster"].ToString();
        }
    }
}

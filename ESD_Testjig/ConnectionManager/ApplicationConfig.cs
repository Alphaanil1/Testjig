using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace ESD_Testjig
{   
    public class ApplicationConfig : IDisposable
    {
        private readonly Encrypt _encrypt = new Encrypt("alphaict2020");
        private List<string> ConnectionString;
        readonly LogFile log = new LogFile();
        //private const string RES_RootName = "Configuration";

        private XmlDocument data;
        private readonly string fileName;
        public ApplicationConfig(string fileName)
        {          
            if (!String.IsNullOrEmpty(fileName))                //throw new ArgumentNullException(RES_RootName);
                    this.fileName = fileName;
        }

        public ApplicationConfig()
        { }
                
        public List<string> Open()
        {
            ConnectionString = new List<string>();
            data = new XmlDocument() { XmlResolver = null };           
                try
                {
                    if (!File.Exists(fileName))
                    {
                    string ServerName = "ALPHA052\\SQLEXPRESS";       // "ALPHA025\\SQLEXPRESS";
                    string DatabaseName = "Testjig_DB";
                    string UserID = "sa";
                    string Password = "Alpha%03";
                    bool IsWindowAuth = true;                    
                    CreateXML(ServerName, DatabaseName, UserID, Password, IsWindowAuth);
                    }                 

                    data.Load(fileName);

                    XmlNodeList elemList = data.GetElementsByTagName("add");

                string[] ConnectionDetails = _encrypt.DecryptString(elemList[0].Attributes["connectionString"].Value).Split(';'); //elemList[0].Attributes["connectionString"].Value.Split(';');  // 

                GlobalInformation.ServerName = ConnectionDetails[0].Split('=')[1];
                    GlobalInformation.DatabaseName = ConnectionDetails[1].Split('=')[1];

                    if (ConnectionDetails[2].Split('=')[1].Trim() != "SSPI")
                    {
                        GlobalInformation.UserID = ConnectionDetails[2].Split('=')[1];
                        GlobalInformation.Password = ConnectionDetails[3].Split('=')[1];
                    }
                    else
                    {
                        GlobalInformation.UserID = string.Empty;
                        GlobalInformation.Password = string.Empty;
                    }

                //ConnectionString.Add(elemList[0].Attributes["connectionString"].Value);
                //ConnectionString.Add(elemList[1].Attributes["connectionString"].Value);

                ConnectionString.Add(_encrypt.DecryptString(elemList[0].Attributes["connectionString"].Value));
                ConnectionString.Add(_encrypt.DecryptString(elemList[1].Attributes["connectionString"].Value));
                ConnectionString.Add(elemList[2].Attributes["IsCentralisedSystem"].Value);
                return ConnectionString;
                }
                catch
                {
                    throw;
                }           
        }

        public bool WriteXML(string ServerName, string DatabaseName, string UserID, string Password, bool IsWindowAuth)
        {
            data = new XmlDocument() { XmlResolver = null };
            try
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "", "configuration.xml");
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    CreateXML(ServerName, DatabaseName, UserID, Password, IsWindowAuth,Convert.ToInt16(GlobalInformation.IsCentralisedSystem));
                }
                else
                {
                    CreateXML(ServerName, DatabaseName, UserID, Password, IsWindowAuth);
                }
            }
            catch
            {
                throw;
            }
            return true;
        }

        public bool CheckConnectionString(string ServerName, string DatabaseName, string UserID, string Password, bool IsWindowAuth)
        {
            StringBuilder _connectionString = new StringBuilder();
            _connectionString.Append("SERVER=" + ServerName + "; ");
            _connectionString.Append("Initial Catalog=" + DatabaseName + ";");
            if (IsWindowAuth)
            {
                _connectionString.Append("User Id=" + UserID + ";");
                _connectionString.Append("Password=" + Password + ";");
                _connectionString.Append("Persist Security Info=false;Integrated Security = false;");
            }
            else
                _connectionString.Append("Integrated Security = SSPI;");

            using (SqlConnection conn = new SqlConnection(_connectionString.ToString()))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool CreateXML(string ServerName,string DatabaseName,string UserID,string Password,bool IsWindowAuth,int IsCentralisedSystem = 0)
        {
            try
            {
                StringBuilder _connectionString = new StringBuilder();
                _connectionString.Append("SERVER=" + ServerName + "; ");
                _connectionString.Append("Initial Catalog=" + DatabaseName + ";");
                if (IsWindowAuth)
                {
                    _connectionString.Append("User Id=" + UserID + ";");
                    _connectionString.Append("Password=" + Password + ";");
                    _connectionString.Append("Persist Security Info=false;Integrated Security = false;");
                }
                else
                    _connectionString.Append("Integrated Security = SSPI;");


                StringBuilder _connectionStringMaster = new StringBuilder();
                _connectionStringMaster.Append("SERVER=" + ServerName + "; ");
                _connectionStringMaster.Append("Initial Catalog=" + "master" + ";");
                _connectionStringMaster.Append("Integrated Security = SSPI;");

                XDocument doc = new XDocument(
               //new XDeclaration("1.0", "utf-8", string.Empty),
               new XElement("configuration", 
               new XElement("connectionStrings",
                                   new XElement("add",
                                       new XAttribute("name", "ConnectionString"),
                                       new XAttribute("connectionString", _encrypt.EncryptString(_connectionString.ToString())),
                                       new XAttribute("providerName", "System.Data.SqlClient")
                                               ),
                                    new XElement("add",
                                       new XAttribute("name", "ConnectionStringMaster"),
                                       new XAttribute("connectionString", _encrypt.EncryptString(_connectionStringMaster.ToString())),
                                            new XAttribute("providerName", "System.Data.SqlClient")
                                               ),
                                    new XElement("add",
                                        new XAttribute("IsCentralisedSystem", IsCentralisedSystem)
                                    )
                            )                              
                            )
                         );
                string ApplicationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "", "configuration.xml");
                doc.Save(ApplicationPath);
                doc = null;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _encrypt.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public bool TestConnection(string ServerName, string DatabaseName, string UserID, string Password, bool IsWindowAuth)
        {

            StringBuilder _connectionString = new StringBuilder();
            _connectionString.Append("SERVER=" + ServerName + "; ");
            _connectionString.Append("Initial Catalog=" + DatabaseName + ";");
            if (IsWindowAuth)
            {
                _connectionString.Append("User Id=" + UserID + ";");
                _connectionString.Append("Password=" + Password + ";");
                _connectionString.Append("Persist Security Info=false;Integrated Security = false;");
            }
            else
                _connectionString.Append("Integrated Security = SSPI;");
            GlobalInformation.ConnectionString = _connectionString.ToString();

            StringBuilder _connectionStringMaster = new StringBuilder();
            _connectionStringMaster.Append("SERVER=" + ServerName + "; ");
            _connectionStringMaster.Append("Initial Catalog=" + "master" + ";");
            _connectionStringMaster.Append("Integrated Security = SSPI;");
            GlobalInformation.ConnectionStringMaster = _connectionStringMaster.ToString();

            using (SqlConnection con = new SqlConnection(GlobalInformation.ConnectionString))
            {
                try
                {
                    con.Open();
                    return true;
                }
                catch
                {                    
                    return false;
                }
            }
        }

        public bool TestServerConnection(string ServerName, string UserID, string Password, bool IsWindowAuth)
        {

            StringBuilder _connectionString = new StringBuilder();
            _connectionString.Append("SERVER=" + ServerName + "; ");
          //  _connectionString.Append("Initial Catalog=" + DatabaseName + ";");
            if (IsWindowAuth)
            {
                _connectionString.Append("User Id=" + UserID + ";");
                _connectionString.Append("Password=" + Password + ";");
                _connectionString.Append("Persist Security Info=false;Integrated Security = false;");
            }
            else
                _connectionString.Append("Integrated Security = SSPI;");
            GlobalInformation.ConnectionString = _connectionString.ToString();

            StringBuilder _connectionStringMaster = new StringBuilder();
            _connectionStringMaster.Append("SERVER=" + ServerName + "; ");
            _connectionStringMaster.Append("Initial Catalog=" + "master" + ";");
            _connectionStringMaster.Append("Integrated Security = SSPI;");
            GlobalInformation.ConnectionStringMaster = _connectionStringMaster.ToString();

            using (SqlConnection con = new SqlConnection(GlobalInformation.ConnectionString))
            {
                try
                {
                    con.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool CheckDatabaseExist(string ConnectionStringMaster, string DatabaseName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringMaster))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    string query = @"IF EXISTS (SELECT name FROM master.sys.databases WHERE name = @DatabaseName)
                            SELECT 1
                            ELSE
                            SELECT 0";

                    command.Parameters.AddWithValue("@DatabaseName", DatabaseName);
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;
                    try
                    {
                        connection.Open();
                        int result = Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture);
                        return result == 1;
                    }
                    catch (Exception ex)
                    {
                        // Handle exception or log error
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                }
            }
                // SqlCommand command = new SqlCommand();
                //string strCheckDB = @"IF EXISTS (SELECT name FROM master.sys.databases WHERE name = @DatabaseName)
                //                    select 1
                //                    ELSE
                //                    SELECT 0";

                //SqlConnection Con = new SqlConnection(ConnectionStringMaster);
                //command.Parameters.AddWithValue("@DatabaseName", DatabaseName);
                //command.Connection = Con;
                //command.CommandType = CommandType.Text;
                //command.Connection.Open();
                //command.CommandText = strCheckDB;
                //bool checkDB = Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture) == 1 ? true : false;
                //return checkDB;   
        }

        public void GenerateDatabase(String ConnectionStringMaster)
        {
            List<string> cmds = new List<string>();
            if (File.Exists(Application.StartupPath + "\\script0.1.sql"))
            {
                TextReader tr = new StreamReader(Application.StartupPath + "\\script0.1.sql");
                string line;
                string cmd = "";
                while ((line = tr.ReadLine()) != null)
                {
                    if (line.Trim().ToUpper(CultureInfo.InvariantCulture) == "GO")
                    {
                        cmds.Add(cmd);
                        cmd = string.Empty;
                    }
                    else
                    {
                        line = line.Replace("Testjig_DB", GlobalInformation.DatabaseName);
                        cmd += line + "\r\n";
                    }
                }
                if (cmd.Length > 0)
                {
                    cmds.Add(cmd); ;
                }
                tr.Close();
            }
            if (cmds.Count > 0)
            {
                SqlCommand command = new SqlCommand();
                try
                {                    
                    SqlConnection Con = new SqlConnection(ConnectionStringMaster);
                    command.Connection = Con;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();

                    for (int i = 0; i < cmds.Count; i++)
                    {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                        command.CommandText = cmds[i];
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities                        
                        command.ExecuteNonQuery();
                        //PleaseWaitMsg.ShowDatabaseInstallingMSg();
                    }
                }
                catch (Exception ex)
                {
                    log.CreateLog(ex.Message, ex.ToString(),"ApplicationConfig.cs");
                   // MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                    command.Dispose();
                }
            }
        }

    }
}

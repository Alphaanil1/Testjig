using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ESD_Testjig
{
    public static class ConnectionManager
    {
        public static string currentConfigFileName { get; set; }
        private static ApplicationConfig Config;
        static LogFile log = new LogFile();

        public static List<string> GetApplicationConfig(string ExeFileName)
        {
            if (String.IsNullOrEmpty(ExeFileName))
            {
                if (currentConfigFileName != null)
                    ExeFileName = currentConfigFileName;
                else
                {
                    ExeFileName = "Configuration.xml";
                    ExeFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "", ExeFileName);
                }
            }
            Config = new ApplicationConfig(ExeFileName);
            try
            {
                return Config.Open();
            }
            catch
            {
                throw;
            }
        }


        public static void ChangeConfigFile(string ServerName, string DatabaseName, string UserID, string Password, bool IsWindowAuth)
        {
            Config = new ApplicationConfig();        
            try
            {
                Config.WriteXML(ServerName, DatabaseName, UserID, Password, IsWindowAuth);
            }
            catch
            {
                throw;
            }
        }

        public static bool CheckConnectionString(string ServerName, string DatabaseName, string UserID, string Password, bool IsWindowAuth)
        {
            Config = new ApplicationConfig();
            try
            {
                return Config.CheckConnectionString(ServerName, DatabaseName, UserID, Password, IsWindowAuth);
            }
            catch
            {
                throw;
            }
        }


        public static SqlConnection SetConnection()
        {
            SqlConnection conn = new SqlConnection(GlobalInformation.ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                return conn;
            }
            catch
            {
                throw;
            }
        }

        public static string Getconstring()
        {
            GetConnectionString();
            return GlobalInformation.ConnectionString;
        }

        //public static List<string> SelectServerName()
        //{
        //    List<string> ServernameList = new List<string>();
        //    //return CommandConfiguration.SelectServerName();
        //    try
        //    {
        //        if (!GlobalInformation.IsCentralisedSystem)
        //            ServernameList = GetLocalServerInstances();
        //        else
        //        {
        //            DataTable InstanceDT = SqlDataSourceEnumerator.Instance.GetDataSources();
        //            foreach (DataRow dr in InstanceDT.Rows)
        //            {
        //                ServernameList.Add(dr["ServerName"].ToString() + "\\" + dr["InstanceName"].ToString());
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return ServernameList;
        //}


        public class DatabaseList
        {
            public Int64 DatabaseID { get; set; }
            public string DatabaseName { get; set; }
            public string DatabasePath { get; set; }
        }

        public static IEnumerable<DatabaseList> GetDatabaseList()
        {
            //get all room types
            List<DatabaseList> lstDatabaseList = new List<DatabaseList>();

            string strSelectRoomType = @"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) as DatabaseID, 
                d.name as 'DatabaseName',  mdf.physical_name as 'FileLocation',  ldf.physical_name as 'log_file'
                from sys.databases d inner join sys.master_files mdf on d.database_id = mdf.database_id and mdf.[type] = 0
                inner join sys.master_files ldf on d.database_id = ldf.database_id and ldf.[type] = 1
	            WHERE CASE WHEN d.state_desc = 'ONLINE' THEN OBJECT_ID(QUOTENAME(d.name) + '.[dbo].[HotelLockConfiguration]', 'U')
			   END IS NOT NULL AND d.name NOT IN ('master', 'model', 'tempdb', 'msdb')";

            DataSet ds = CommonFunction.GetList(strSelectRoomType);
            try
            {
                lstDatabaseList = ds.Tables[0].AsEnumerable().Select(
                   dataRow => new DatabaseList
                   {
                       DatabaseID = dataRow.Field<Int64>("DatabaseID"),
                       DatabaseName = dataRow.Field<string>("DatabaseName"),
                       DatabasePath = dataRow.Field<string>("FileLocation"),
                   }).ToList();
                return lstDatabaseList;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds.Dispose();
            }
        }

        //public static IEnumerable<DatabaseList> GetDatabaseList()
        //{
        //    return GetDatabaseList();
        //}

        public static void GetConnectionString()
        {
            try
            {
                List<string> ConnectionString = ConnectionManager.GetApplicationConfig("Configuration.xml");
                GlobalInformation.ConnectionString = ConnectionString[0];
                GlobalInformation.ConnectionStringMaster = ConnectionString[1];
                GlobalInformation.IsCentralisedSystem = Convert.ToBoolean(Convert.ToInt16(ConnectionString[2], CultureInfo.InvariantCulture));
            }
            catch (SqlException SQLex)
            {
                if (SQLex.Number == 26)
                {
                    CommonFunction.DisplayMessage(GlobalInformation.ResourceManager.GetString("MSG_NETWORK_SERVERFAILURE", GlobalInformation.MachinCulture));
                }
                else
                {
                    CommonFunction.DisplayMessage(SQLex.Message);
                }
                log.CreateLog(SQLex.Message, SQLex.ToString(), "ConnectionManager.cs");
            }
            catch
            {
                throw;
            }
        }

        //public static DataSet Configuration()
        //{
        //    return GetConfiguration();
        //}

        //public static DataSet GetConfiguration()
        //{
        //    string strCongigurationList = @"SELECT * FROM HotelLockConfiguration";   //WHERE BranchID = " + GlobalInformation.BranchID;
        //    DataSet ds = CommonFunction.GetList(strCongigurationList);
        //    List<Configurations> _listConfiguration = new List<Configurations>();
        //    try
        //    {
        //        _listConfiguration = ds.Tables[0].AsEnumerable().Select(
        //           dataRow => new Configurations
        //           {
        //               ConfigurationID = dataRow.Field<int>("ConfigurationID"),
        //               SelectLanguage = dataRow.Field<int>("ApplicationLanguage"),
        //               BackupDbBeforeExit = dataRow.Field<bool>("BackupDbBeforeExit"),
        //               AllowLiftControl = dataRow.Field<bool>("AllowLiftFunction"),
        //               LoginProtection = dataRow.Field<bool>("LoginProtection"),
        //               WaitingTime = Convert.ToInt32(dataRow.Field<int>("WaitingTime")),
        //               DefaultCheckOutTimeSetting = dataRow.Field<bool>("DefaultCheckOutTimeSetting"),
        //               DefaultCheckOutTime = dataRow.Field<DateTime>("DefaultCheckOutTime"),
        //               ApplyGuestCardExpirationWarning = dataRow.Field<bool>("ApplyGuestCardExpirationWarning"),
        //               ExpirationWarningBeforeTime = Convert.ToInt32(dataRow.Field<int>("ExpirationWarningBeforeTime")),
        //               AllowChangeFontSize = Convert.ToBoolean(dataRow.Field<bool>("AllowChangeFontSize")),
        //               ChangedFontsize = Convert.ToInt16(dataRow.Field<int>("ChangedFontsize")),
        //           }).ToList();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return ds;
        //}
    }
}

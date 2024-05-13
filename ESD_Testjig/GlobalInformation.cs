using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;



namespace ESD_Testjig
{
    public static class GlobalInformation
    {
        public static int LoginUserID { set; get; }
       // public static int BranchID { set; get; }
        public static bool IsCentralisedSystem { set; get; }

        public static string UserName { set; get; }
       // public static string BranchName { set; get; }

        public static bool IsAdmin { set; get; }
        public static bool AllowConfiguration { set; get; }
        public static bool AllowRoomCreation { set; get; }
        public static bool AllowRoomOperation { set; get; }
        public static bool AllowUserLiftControl { set; get; }

        public static bool MemberAccessToolTip { set; get; }
        public static bool OperatorAccessToolTip { set; get; }
        public static bool OperatorLockBattery { set; get; }

        public static string SelectedLanguage { set; get; }
        public static string SelectedDatabase { set; get; }

        //CONFIGURATION 
        public static bool BackupDbBeforeExit { set; get; }
        public static bool AllowLiftControl { set; get; }
        public static bool LoginProtection { set; get; }
        public static int LoginWaitingTime { set; get; }
        public static bool DefaultCheckOutTimeSetting { set; get; }
        public static DateTime DefaultCheckOutTime { set; get; }
        public static bool ApplyGuestCardExpirationWarning { set; get; }
        public static int ExpirationWarningBeforeTime { set; get; }
        public static bool AllowChangeFontSize { set; get; }
        public static int ChangedFontsize { set; get; }

        public static int ApplicationLanguage { set; get; }

        //***DATABASE DETAILS
        public static string DatabaseName { set; get; }
        public static string ServerName { set; get; }
        public static string UserID { set; get; }
        public static string Password { set; get; }
        public static bool IsServerAuth { set; get; }

        public static string ConnectionString { set; get; }
        public static string ConnectionStringMaster { set; get; }

        public static int ProductTypeID { set; get; }

        //***

        public static ResourceManager ResourceManager { set; get; }
        public static CultureInfo MachinCulture { set; get; }
        public static CultureInfo MachinUICulture { set; get; }
        //
        public const string ProjectVersion = "8.19";

        public const string PreviousDBVersion = "6";
        public const string NewDBVersion = "7";

        //public static void SetAdminProperties(int _loginUserID, int _branchID)
        //{
        //    LoginUserID = _loginUserID;
        //    BranchID = _branchID;
        //}

        public static void SetCofiguration(DataSet dsconfig)
        {
            if (dsconfig == null) return;

            if (dsconfig.Tables[0].Rows.Count > 0)
            {
                ApplicationLanguage = Convert.ToInt16(dsconfig.Tables[0].Rows[0]["ApplicationLanguage"], CultureInfo.InvariantCulture);
                //ShowDatabaseSelection = Convert.ToBoolean(dsconfig.Tables[0].Rows[0]["ApplicationDatabase"], CultureInfo.InvariantCulture);
                BackupDbBeforeExit = Convert.ToBoolean(dsconfig.Tables[0].Rows[0]["BackupDbBeforeExit"], CultureInfo.InvariantCulture);
                AllowLiftControl = Convert.ToBoolean(dsconfig.Tables[0].Rows[0]["AllowLiftFunction"], CultureInfo.InvariantCulture);
                LoginProtection = Convert.ToBoolean(dsconfig.Tables[0].Rows[0]["LoginProtection"], CultureInfo.InvariantCulture);
                LoginWaitingTime = Convert.ToInt16(dsconfig.Tables[0].Rows[0]["WaitingTime"], CultureInfo.InvariantCulture);
                DefaultCheckOutTimeSetting = Convert.ToBoolean(dsconfig.Tables[0].Rows[0]["DefaultCheckOutTimeSetting"], CultureInfo.InvariantCulture);
                DefaultCheckOutTime = Convert.ToDateTime(dsconfig.Tables[0].Rows[0]["DefaultCheckOutTime"], CultureInfo.InvariantCulture);
                ApplyGuestCardExpirationWarning = Convert.ToBoolean(dsconfig.Tables[0].Rows[0]["ApplyGuestCardExpirationWarning"], CultureInfo.InvariantCulture);
                ExpirationWarningBeforeTime = Convert.ToInt32(dsconfig.Tables[0].Rows[0]["ExpirationWarningBeforeTime"], CultureInfo.InvariantCulture);
                AllowChangeFontSize = Convert.ToBoolean(dsconfig.Tables[0].Rows[0]["AllowChangeFontSize"], CultureInfo.InvariantCulture);
                ChangedFontsize = Convert.ToInt32(dsconfig.Tables[0].Rows[0]["ChangedFontsize"], CultureInfo.InvariantCulture);
            }
            else
            {
                DateTime checkoutTime = new DateTime();
                TimeSpan tS = new TimeSpan(10, 00, 00);
                checkoutTime = DateTime.Now.Date + tS;
                DefaultCheckOutTime = checkoutTime;
            }
        }





    }
}

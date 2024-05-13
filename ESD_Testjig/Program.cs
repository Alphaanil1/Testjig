using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESD_Testjig
{
    static class Program
    {
        static LogFile log = new LogFile();
        static string currentFileName = string.Empty;
       /// <summary>
       /// The main entry point for the application.
       /// </summary>
       /// 
       //[STAThread]
       //static void Main()
       //{
       //    //System.Threading.Mutex singleton = new Mutex(true, "ESD_Testjig");
       //    //if (!singleton.WaitOne(TimeSpan.Zero, true))
       //    //{
       //    //    MessageBox.Show("Instance already running");
       //    //    Application.Exit();
       //    //    return;
       //    //}
       //    //Application.EnableVisualStyles();
       //    //Application.SetCompatibleTextRenderingDefault(false);
       //    //if (!CheckDatabaseExist())
       //    //{
       //    //    GenerateDatabase();
       //    //} 
       //    //Application.Run(new Login());

       //    System.Threading.Mutex singleton = new Mutex(true, "ESD_Testjig");

       //    // Release the mutex when the application is closing
       //    FormClosingEventHandler closingHandler = null;
       //    closingHandler = (sender, e) =>
       //    {
       //        singleton.ReleaseMutex();
       //        singleton.Dispose();
       //        Application.Exit();
       //    };

       //    Application.EnableVisualStyles();
       //    Application.SetCompatibleTextRenderingDefault(false);

       //    Login loginForm = new Login();
       //    loginForm.FormClosing += closingHandler;

       //    if (!singleton.WaitOne(TimeSpan.Zero, true))
       //    {
       //        MessageBox.Show("Instance already running");
       //        Application.Exit();
       //    }
       //    else
       //    {
       //        if (!CheckDatabaseExist())
       //        {
       //            GenerateDatabase();
       //        }
       //        Application.Run(loginForm);
       //    }
       //}
       //private static bool CheckDatabaseExist()
       //{
       //    SqlConnection con = new SqlConnection(DBAccess.getconstring());
       //    try
       //    {
       //        con.Open();
       //        //MessageBox.Show("Connection successfull");
       //        return true;
       //    }
       //    catch (Exception ex)
       //    {
       //        // MessageBox.Show(ex.ToString());             
       //        return false;
       //    }
       //}

       //private static void GenerateDatabase()
       //{
       //    List<string> cmds = new List<string>();
       //    if (File.Exists(Application.StartupPath + "\\Script0.1.sql"))
       //    // if (File.Exists(Application.StartupPath + a))
       //    {
       //        TextReader tr = new StreamReader(Application.StartupPath + "\\Script0.1.sql");
       //        string line = "";
       //        string cmd = "";
       //        while ((line = tr.ReadLine()) != null)
       //        {
       //            if (line.Trim().ToUpper() == "GO")
       //            {
       //                cmds.Add(cmd);
       //                cmd = "";
       //            }
       //            else
       //            {
       //                cmd += line + "\r\n";
       //            }
       //        }
       //        if (cmd.Length > 0)
       //        {
       //            cmds.Add(cmd);
       //            cmd = "";
       //        }
       //        tr.Close();
       //    }
       //    if (cmds.Count > 0)
       //    {
       //        try
       //        {
       //            SqlCommand command = new SqlCommand();
       //            SqlConnection Connection = new SqlConnection(DBAccess.getconstringMaster());
       //            command.Connection = Connection;
       //            command.CommandType = CommandType.Text;
       //            command.Connection.Open();
       //            for (int i = 0; i < cmds.Count; i++)
       //            {
       //                command.CommandText = cmds[i];
       //                command.ExecuteNonQuery();
       //                PleaseWaitMsg.ShowDBMSg();
       //            }
       //            MessageBox.Show("Database created successfully!");
       //        }
       //        catch (Exception ex)
       //        {
       //            //Console.WriteLine(ex.Message);
       //            MessageBox.Show(ex.Message);
       //        }
       //    }
       //}


       // New Configuration code 02042024

       [STAThread]
        static void Main()
        {
            string currentFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // Get the file name with extension
             currentFileName = Path.GetFileName(currentFilePath);

            if (RunningInstance() != null)
            {
                CommonFunction.DisplayMessage("Instance already running");
                return;
            }

            //CHECK CONFIG FILE IS EXIST
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "", "configuration.xml");
            if (!File.Exists(fileName))
            {
                try
                {
                    GlobalInformation.MemberAccessToolTip = false;
                    GlobalInformation.OperatorAccessToolTip = false;
                   // CurrentMachineCulture();
                    using (frmConnectionSettings conConfig = new frmConnectionSettings())
                    {
                        conConfig.ShowDialog();
                    }
                }
                catch (SqlException SQLex)
                {
                    if (SQLex.Number == 26 || SQLex.Number == -1 || SQLex.Number == 10054)
                    {
                        CommonFunction.DisplayMessage(GlobalInformation.ResourceManager.GetString("MSG_NETWORK_SERVERFAILURE", GlobalInformation.MachinCulture));
                        log.CreateLog(SQLex.Message, SQLex.ToString(), currentFileName);
                    }
                    else
                    {
                        log.CreateLog(SQLex.Message, SQLex.ToString(), currentFileName);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    log.CreateLog(ex.Message, ex.ToString(), currentFileName);
                    throw;
                }
                finally
                {
                    GC.Collect();
                }
            }
            else
            {
                ProjectStartUp();
            }
        }

        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //Loop through the running processes in with the same name 
            foreach (Process process in processes)
            {
                //Ignore the current process 
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file. 
                    if (Assembly.GetExecutingAssembly().Location.
                         Replace("/", "\\") == current.MainModule.FileName)

                    {
                        //Return the other process instance.  
                        return process;

                    }
                }
            }
            //No other instance was found, return null.  
            return null;
        }


        public static void DatabaseScriptExecute()
        {
            using (ApplicationConfig config = new ApplicationConfig())
            {
                //Check database exist or not
                if (!CheckDatabaseExist())
                {
                    config.GenerateDatabase(GlobalInformation.ConnectionStringMaster);
                }
                //else if (!GlobalInformation.PreviousDBVersion.Equals(GlobalInformation.NewDBVersion, StringComparison.Ordinal))
                //{
                //    //IF DATABASE HAS CHANGES INCREASE NEWDBVERSION VALUE, 
                //    config.GenerateDatabase(GlobalInformation.ConnectionString);
                //}
                System.Threading.Thread.Sleep(4000);
            }
        }

        public static void ProjectStartUp()
        {
            //if (IsAlreadyRunning())
            //{
            //    Application.Exit();
            //}

            DatabaseScriptExecute();

            //******ONE COLUMN INCREASED IN EACH RESERVATION & DOORS TABLE. IF NOT PRESENT IN EXISTING TABLE , UNCOMMENT BELOW LINE
            //UpdateDatabase();

            //GET CONFIGURATION DETAILS
           // DataSet dsConfig = UserMasterController.Configuration();
           // GlobalInformation.SetCofiguration(dsConfig);
            GlobalInformation.MemberAccessToolTip = false;
            GlobalInformation.OperatorAccessToolTip = false;

           // CurrentMachineCulture();

            try
            {   
                using (Login objlogin = new Login())
                {
                        Application.Run(objlogin);
                }
            }
            catch (SqlException SQLex)
            {
                //10054
                if (SQLex.Number == 26 || SQLex.Number == -1 || SQLex.Number == 10054)
                {
                    CommonFunction.DisplayMessage(GlobalInformation.ResourceManager.GetString("MSG_NETWORK_SERVERFAILURE", GlobalInformation.MachinCulture));
                    log.CreateLog(SQLex.Message, SQLex.ToString(), currentFileName);
                }
                else
                {
                    log.CreateLog(SQLex.Message, SQLex.ToString(), currentFileName);
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), currentFileName);
            }
            finally
            {
               // dsConfig.Dispose();
                GC.Collect();
            }
        }

        //public static void CurrentMachineCulture()
        //{
        //    try
        //    {
        //        LanguageType enumValue = (LanguageType)GlobalInformation.ApplicationLanguage;
        //        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        //        var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        //        if (descriptionAttributes.Length > 0 || descriptionAttributes != null)
        //        {
        //            string language = descriptionAttributes[0].Description.Substring(descriptionAttributes[0].Description.IndexOf('(') + 1, 2);
        //            GlobalInformation.ResourceManager = new ResourceManager("HotelLocks.AppResource.Res", Assembly.GetExecutingAssembly());

        //            GlobalInformation.MachinCulture = CultureInfo.CreateSpecificCulture(language);
        //            GlobalInformation.MachinUICulture = CultureInfo.CreateSpecificCulture(language);

        //            Thread.CurrentThread.CurrentUICulture = GlobalInformation.MachinUICulture;
        //            Thread.CurrentThread.CurrentCulture = GlobalInformation.MachinCulture;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public static bool PriorProcess()
        {
            // Returns a System.Diagnostics.Process pointing to
            // a pre-existing process with the same name as the
            // current one, if any; or null if the current process
            // is unique.
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) && (p.MainModule.FileName == curr.MainModule.FileName))
                    return false;
            }
            return false;
        }

        private static bool CheckDatabaseExist()
        {
            using (SqlConnection con = new SqlConnection(ConnectionManager.Getconstring()))
            {
                try
                {
                    con.Open();
                    return true;
                }
                catch (SqlException SQLex)
                {
                    if (SQLex.Number == 26 || SQLex.Number == -1 || SQLex.Number == 10054)
                    {
                        CommonFunction.DisplayMessage("The server was not found or was not accessible.");
                        //GlobalInformation.ResourceManager.GetString("MSG_NETWORK_SERVERFAILURE", GlobalInformation.MachinCulture));
                        log.CreateLog(SQLex.Message, SQLex.ToString(), currentFileName);
                    }
                    else
                    {
                        log.CreateLog(SQLex.Message, SQLex.ToString(), currentFileName);
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    log.CreateLog(ex.Message, ex.ToString(), currentFileName);
                    return false;
                }
            }
        }        

        private static void UpdateDatabase()
        {
            List<string> cmds = new List<string>();
            if (File.Exists(Application.StartupPath + "\\UpdateDatabase.sql"))
            {
                TextReader tr = new StreamReader(Application.StartupPath + "\\UpdateDatabase.sql");
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
                        cmd += line + "\r\n";
                    }
                }
                if (cmd.Length > 0)
                {
                    cmds.Add(cmd);
                }
                tr.Close();
            }
            if (cmds.Count > 0)
            {
                SqlCommand command = new SqlCommand();
                try
                {

                    SqlConnection Con = new SqlConnection(GlobalInformation.ConnectionString);
                    command.Connection = Con;
                    command.CommandType = CommandType.Text;
                    command.Connection.Open();
                    for (int i = 0; i < cmds.Count; i++)
                    {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                        command.CommandText = cmds[i];
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException SQLex)
                {
                    if (SQLex.Number == 26 || SQLex.Number == -1 || SQLex.Number == 10054)
                    {
                        CommonFunction.DisplayMessage(GlobalInformation.ResourceManager.GetString("MSG_NETWORK_SERVERFAILURE", GlobalInformation.MachinCulture));
                        log.CreateLog(SQLex.Message, SQLex.ToString(), currentFileName);
                    }
                    else
                    {
                        log.CreateLog(SQLex.Message, SQLex.ToString(), currentFileName);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    log.CreateLog(ex.Message, ex.ToString(), currentFileName);
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

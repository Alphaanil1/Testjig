using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ESD_Testjig.ConnectionManager;

namespace ESD_Testjig
{
    public partial class frmConnectionSettings : Form
    {
        //private readonly ConfigurationController _controller = new ConfigurationController();
        private static ApplicationConfig Config;
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_RESTORE = 9;

        private static string DatabaseName = "Testjig_DB";
        
        public frmConnectionSettings()
        {
            InitializeComponent();
           // Application.UseWaitCursor = true;
            btnClose.Click += BtnClose_Click;
            btnSave.Click += BtnSave_Click;
            btnSave.Enabled = false;
            btnTestCon.Click += BtnTestCon_Click;
            Config = new ApplicationConfig();
            TimeSpan ts = new TimeSpan(10, 00, 00);
            //SetLabelText();
            
            rbWindowAuth.CheckedChanged += RbWindowAuth_CheckedChanged;
            rbSQLServerAuth.CheckedChanged += RbSQLServerAuth_CheckedChanged;
            rbStandAlone.CheckedChanged += RbStandAlone_CheckedChanged;
            rbCentralized.CheckedChanged += RbCentralized_CheckedChanged;
            rdbNewDB.CheckedChanged += RdbNewDB_CheckedChanged;
            rdbExistingDB.CheckedChanged += RdbExistingDB_CheckedChanged;
            //rbSQLServerAuth.Checked = true;
            rbStandAlone.Checked = true;
            TxtUserID.Enabled = rbSQLServerAuth.Checked;
            TxtPassword.Enabled = rbSQLServerAuth.Checked;
        }

        private void RdbExistingDB_CheckedChanged(object sender, EventArgs e)
        {
            GetDatabaseList();
            if (rdbExistingDB.Checked)
            {
                cmbDatabases.Visible = rdbExistingDB.Checked;
                txtDatabaseName.Visible = !rdbExistingDB.Checked;
                txtDatabaseName.Text = string.Empty;
            }
            else
            {
                cmbDatabases.Visible = !rdbExistingDB.Checked;
                txtDatabaseName.Visible = rdbExistingDB.Checked;
            }
            lblSelectDB.Text = "Select Database";
        }

        private void RdbNewDB_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNewDB.Checked)
            {
                txtDatabaseName.Visible = rdbNewDB.Checked;
                cmbDatabases.Visible = !rdbNewDB.Checked;
            }
            else
            {
                txtDatabaseName.Visible = !rdbNewDB.Checked;
                cmbDatabases.Visible = rdbNewDB.Checked;
            }
            lblSelectDB.Text = "Enter Name";
        }

        private void RbCentralized_CheckedChanged(object sender, EventArgs e)
        {
           // Cursor.Current = Cursors.WaitCursor;
            cmbServerList.DataSource = null;
            cmbServerList.Items.Clear();
            GetServerList();
           // Cursor.Current = Cursors.Default;
        }

        private void RbStandAlone_CheckedChanged(object sender, EventArgs e)
        {
           // Cursor.Current = Cursors.WaitCursor;
            cmbServerList.DataSource = null;
            cmbServerList.Items.Clear();
            GetServerList();
           // Cursor.Current = Cursors.Default;
        }

        //public override void SetLabelText()
        //{
        //    CommonFunction.ControlSetting(this);

        //    this.Text = GlobalInformation.ResourceManager.GetString("LBL_CONFIGURATION", GlobalInformation.MachinCulture).ToUpper(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture);
        //    lblSelectServer.Text = GlobalInformation.ResourceManager.GetString("LBL_SELECTSERVER", GlobalInformation.MachinCulture);
        //    lblAuthMode.Text = GlobalInformation.ResourceManager.GetString("LBL_AUTHENTIFICATIONMODE", GlobalInformation.MachinCulture);
        //    lblUser.Text = GlobalInformation.ResourceManager.GetString("LBL_USERNAME", GlobalInformation.MachinCulture);
        //    lblPassword.Text = GlobalInformation.ResourceManager.GetString("LBL_PASSWORD", GlobalInformation.MachinCulture);

        //    rbWindowAuth.Text = GlobalInformation.ResourceManager.GetString("LBL_WINAUTHENTIFICATION", GlobalInformation.MachinCulture);
        //    rbSQLServerAuth.Text = GlobalInformation.ResourceManager.GetString("LBL_SERVERAUTHENTIFICATION", GlobalInformation.MachinCulture);
        //     lblPassword.Text = GlobalInformation.ResourceManager.GetString("LBL_PASSWORD", GlobalInformation.MachinCulture);
        //    btnSave.Text = GlobalInformation.ResourceManager.GetString("LBL_SAVE", GlobalInformation.MachinCulture).ToUpper(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture);
        //    btnClose.Text = GlobalInformation.ResourceManager.GetString("LBL_CLOSE", GlobalInformation.MachinCulture).ToUpper(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture);
        //}

        private List<string> GetLocalServerInstances()
        {
            List<string> lstSetverName = new List<string>();
            try
            {
                var registryViewArray = new[] { RegistryView.Registry32, RegistryView.Registry64 };

                foreach (var registryView in registryViewArray)
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
                    using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server"))
                    {
                        var instances = (string[])key?.GetValue("InstalledInstances");
                        if (instances != null)
                        {
                            foreach (var element in instances)
                            {
                                if (element == "MSSQLSERVER")
                                    lstSetverName.Add(System.Environment.MachineName);
                                else
                                    lstSetverName.Add(System.Environment.MachineName + @"\" + element);

                            }
                        }
                    }
                }

                return lstSetverName;
            }
            catch
            {
                throw;
            }
        }


        private void GetServerList()
        {
            try
            {
                DataTable dsServerName = new DataTable();
                    dsServerName.Columns.Add("ServerID");
                    dsServerName.Columns.Add("srvname");

                if (rbStandAlone.Checked)
                {
                    List<string> lstLocalSetverName = new List<string>();
                    lstLocalSetverName = GetLocalServerInstances();
                    for (int j =0; j < lstLocalSetverName.Count; j++)
                    {
                        dsServerName.Rows.Add(j, lstLocalSetverName[j]);
                    }
                }
                else if (rbCentralized.Checked)
                {
                    string servername = System.Environment.MachineName;
                    DataTable InstanceDT = SqlDataSourceEnumerator.Instance.GetDataSources();

                    for (int i = 0; i < InstanceDT.Rows.Count; i++)
                    {
                        if(!string.IsNullOrWhiteSpace(InstanceDT.Rows[i]["InstanceName"].ToString()))
                            dsServerName.Rows.Add(i, InstanceDT.Rows[i]["ServerName"].ToString() + "\\" + InstanceDT.Rows[i]["InstanceName"].ToString());
                    }
                }               
               
                cmbServerList.DataSource = dsServerName;
                cmbServerList.ValueMember = "ServerID";
                cmbServerList.DisplayMember = "srvname";               
            }
            catch
            {
                throw;
            }
        }

        private void RbSQLServerAuth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSQLServerAuth.Checked)
            {
                TxtUserID.Enabled = rbSQLServerAuth.Checked;
                TxtPassword.Enabled = rbSQLServerAuth.Checked;
            }
            else
            {
                TxtUserID.Enabled = !rbWindowAuth.Checked;
                TxtPassword.Enabled = !rbWindowAuth.Checked;
            }
            TxtUserID.Text = GlobalInformation.UserID;
            TxtPassword.Text = GlobalInformation.Password;
        }

        private void RbWindowAuth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSQLServerAuth.Checked)
            {
                TxtUserID.Enabled = rbSQLServerAuth.Checked;
                TxtPassword.Enabled = rbSQLServerAuth.Checked;
            }
            else
            {
                TxtUserID.Enabled = !rbWindowAuth.Checked;
                TxtPassword.Enabled = !rbWindowAuth.Checked;
            }
            TxtUserID.Text = string.Empty;
            TxtPassword.Text = string.Empty;
        }

        private void TxtWarningTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void TxtWaitingTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtWaitingTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private bool SaveConfigFile;

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = ConnectionManager.SetConnection())
            {
                //SqlTransaction trans = conn.BeginTransaction("HotelLocksTransaction");

                try
                {
                    if (!SaveConfigFile)
                    {
                        //ApplicationConfig config = new ApplicationConfig();
                        string ServerName = Convert.ToString(cmbServerList.Text, CultureInfo.InvariantCulture).Trim();
                        string UserID = TxtUserID.Text.Trim();
                        string Password = TxtPassword.Text.Trim();
                        bool IsWindowAuth = rbWindowAuth.Checked;
                        int IsCentralisedSystem = rbCentralized.Checked ? 1 : 0;
                        if (Config.CreateXML(ServerName, DatabaseName, UserID, Password, !IsWindowAuth, IsCentralisedSystem))
                        {
                            SaveConfigFile = true;

                            Program.DatabaseScriptExecute();
                            this.Hide();

                            //// Check if another instance is already running
                            //Process currentProcess = Process.GetCurrentProcess();
                            //Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);

                            //if (processes.Length > 1)
                            //{
                            //    // Another instance is running, bring it to foreground and exit
                            //    IntPtr hWnd = processes[0].MainWindowHandle;
                            //    if (hWnd != IntPtr.Zero)
                            //    {
                            //        // Bring the window to the foreground
                            //        ShowWindowAsync(hWnd, SW_RESTORE);
                            //        SetForegroundWindow(hWnd);
                            //    }
                            //    return;
                            //}

                            //using (Login objlogin = new Login())
                            //{                               
                            //    Application.Run(objlogin);
                            //}
                             RestartApplication();
                        }

                       // trans.Commit();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                   // trans.Dispose();
                }
            }
        }

        private void RestartApplication()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
           // Info.FileName = "cmd.exe";
            Info.UseShellExecute = false;
            Process.Start(Info);
            Application.Exit();
            //Environment.Exit(0);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

       

        private void BtnTestCon_Click(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;

            if (rbSQLServerAuth.Checked && (string.IsNullOrWhiteSpace(TxtUserID.Text) || string.IsNullOrWhiteSpace(TxtPassword.Text)))
            {
                CommonFunction.DisplayMessage("Please enter User ID and password.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtDatabaseName.Text) && rdbNewDB.Checked == true)
                DatabaseName = txtDatabaseName.Text;
            else if (!string.IsNullOrWhiteSpace(cmbDatabases.SelectedItem.ToString()) && rdbExistingDB.Checked == true)
            { DatabaseName = cmbDatabases.SelectedItem.ToString(); }
            else
            { MessageBox.Show("Please enter correct data");
                return;
            }

            GlobalInformation.DatabaseName = DatabaseName;

            string ServerName = Convert.ToString(cmbServerList.Text, CultureInfo.InvariantCulture).Trim();
            string UserID = TxtUserID.Text.Trim();
            string Password = TxtPassword.Text.Trim();
            bool IsWindowAuth = rbWindowAuth.Checked;
            try
            {
                if (!Config.TestConnection(ServerName, DatabaseName, UserID, Password, !IsWindowAuth))
                {
                    //IF DB NOT PRESENT THEN CREATE DB
                    if(!Config.CheckDatabaseExist(GlobalInformation.ConnectionStringMaster, DatabaseName))
                        Config.GenerateDatabase(GlobalInformation.ConnectionStringMaster);

                    System.Threading.Thread.Sleep(4000);
                    if (Config.TestConnection(ServerName, DatabaseName, UserID, Password, !IsWindowAuth))
                    {
                        CommonFunction.DisplayMessage("Connection done successfully.");
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        CommonFunction.DisplayMessage("Connection failed.");
                        btnSave.Enabled = false;
                    }
                }
                else
                {
                    CommonFunction.DisplayMessage("Connection done successfully.");
                    btnSave.Enabled = true;
                }
                
            }
            catch(Exception ex)
            {
                if(((System.Data.SqlClient.SqlException)ex).Number == 258)
                {
                    CommonFunction.DisplayMessage(ex.Message + " " + "The server was not found or was not accessible. Please check network.");
                    return;
                }
                else if (((System.Data.SqlClient.SqlException)ex).Number == 262)
                {
                    CommonFunction.DisplayMessage(ex.Message + " " + "Please check server rights or check the user name and password.");
                    return;
                }
                else
                {
                    CommonFunction.DisplayMessage(ex.Message + " " + "Please check user id and password.");
                    return;
                }
            }
           // Cursor.Current = Cursors.Default;
        }

        private void GetDatabaseList()
        {
            try
            {
                //List<DatabaseList> lstDatabaselist = ConnectionManager.GetDatabaseList().ToList();
                //cmbDatabases.ValueMember = "DatabaseID";
                //cmbDatabases.DisplayMember = "DatabaseName";
                //cmbDatabases.DataSource = lstDatabaselist;
               
                string ServerName = Convert.ToString(cmbServerList.Text, CultureInfo.InvariantCulture).Trim();
                string UserID = TxtUserID.Text.Trim();
                string Password = TxtPassword.Text.Trim();
                bool IsWindowAuth = rbWindowAuth.Checked;
                string connectionString = string.Empty;
                if(IsWindowAuth)
                {
                     connectionString = "Server=" + ServerName + ";Integrated Security=true;";
                }
                else if(!string.IsNullOrEmpty(UserID) && !string.IsNullOrEmpty(Password))
                {
                     connectionString = "Server="+ServerName+";User Id="+UserID+";Password="+Password+";";
                }
                   

                if(string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Please select correct server details");
                    return;
                }
                // List to store database names
                List<string> databaseList = new List<string>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT name FROM sys.databases WHERE database_id > 4", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        databaseList.Add(reader["name"].ToString());
                    }

                    reader.Close();
                }

                List<String> lstDatabaselist = databaseList.ToList();
                cmbDatabases.DataSource = lstDatabaselist;
            }
            catch
            {
                throw;
            }
        }
       
    }
}

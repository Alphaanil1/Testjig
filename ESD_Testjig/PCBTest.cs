using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESDBE;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Management;
using System.Globalization;

namespace ESD_Testjig
{
    public partial class PCBTest : Form
    {      
        readonly ESD_TestjigProperties _objDal = new ESD_TestjigProperties();
        readonly ESDBEProperties _objBE = new ESDBEProperties();
        readonly MyDialogBox _mydialogBox = new MyDialogBox();
        readonly MyMessageBox _msgbox = new MyMessageBox();
        readonly LogFile log = new LogFile();
        public delegate void SimpleDelegate();
        public delegate void SimpleDelegateLabelStatus();
        int pcbtypeid = 0;
       // private frmProductType frmProduct;
        int Autoj = 0;
        readonly int LoginUserId;
        readonly string Username = string.Empty;
        string FrameToSend = string.Empty;
        string ResponseToReceive = string.Empty;
        private String Messages;
        private string ErrorMsg;       
        string PIdVIdDeviceID=string.Empty;
        string USBDescription = string.Empty;
        bool rdbPassClicked;   
        bool rdbFailClicked;   
        System.Threading.Timer sysTimer;
        bool isTestRunning = false;
        public PCBTest(int UserID,string UserName)
        {
            InitializeComponent();                   
            LoginUserId = UserID;
            Username = UserName;
            txtserialNo.KeyUp += TxtserialNo_KeyUp;
            txtserialNo.MouseDown += TxtserialNo_MouseDown;
            DataTable dtUserRole = _objDal.GetUserRole(LoginUserId);
            string Role = dtUserRole.Rows[0]["Role"].ToString();
            //User id admin
            // if (Role == "Admin" || Role == "admin")
            if (Convert.ToInt32(dtUserRole.Rows[0]["RoleID"]) == 1)
                LblUser.Visible = true;
            else
            {   LblUser.Visible = false; }
            //Timer is used to check usb status (connected/disconnected)
            //timer1.Start();

            //sysTimer = new System.Threading.Timer(_ => SysTimerTick(), null, 0, 2000);
        }

        private void TxtserialNo_MouseDown(object sender, MouseEventArgs e)
        {
            // Disable right-click context menu
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();
                txtserialNo.ContextMenu = contextMenu;
            }
        }

        private void TxtserialNo_KeyUp(object sender, KeyEventArgs e)
        {
            if ((sender as TextBox).Text.Count() == 3)  //allow backspace(10-4-19)&& e.KeyChar != (char)Keys.Back
            {
                string thirdChar = txtserialNo.Text.Substring(2, 1);
                if (pcbtypeid == 1)
                {
                    if (thirdChar != "C")
                        txtserialNo.Text = txtserialNo.Text.Substring(0, 2) + "C" + txtserialNo.Text.Substring(3, txtserialNo.Text.Count() - 3);
                }
                else if (pcbtypeid == 2)
                {
                    if (thirdChar != "R")
                        txtserialNo.Text = txtserialNo.Text.Substring(0, 2) + "R" + txtserialNo.Text.Substring(3, txtserialNo.Text.Count() - 3);
                }              
                else
                {
                    e.Handled = true;
                    SerialNoMsgBox.ShowBox();    //MessageBox.Show("Please enter correct character"); 
                }
                txtserialNo.SelectionStart = txtserialNo.Text.Length;
                txtserialNo.SelectionLength = 0;
            }
        }

        private void PCBTest_Load(object sender, EventArgs e)
        {
            try
            {
                grdPcbType.ClearSelection();
                lblPcbTypeId.Visible = false;
                grdPcbType.DataSource = null;
                //For login form,close button and minimize button location on any resolution PC.          
                const int margin = 0;
                Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

                Rectangle rect = new Rectangle(
                workingArea.X + margin,
                workingArea.Y + margin,
                workingArea.Width,
                workingArea.Height);
                this.Bounds = rect;
                int screenWidth = workingArea.Width;

                pikboxClose.Location = new Point(screenWidth - 40, 4);
                pikboxMinimize.Location = new Point(screenWidth - 70, 5);

                lblSignOut.Text = Username + "|Sign Out";
                // lblSignOut.Location = new Point(screenWidth - 120, 51);

                btnUsbStatus.Location = new Point(screenWidth - 90, 12);
                LblUser.Location = new Point(screenWidth - 200, 17);
                lblProdcutSelect.Location = new Point(screenWidth - 380, 17);


                using (Graphics g = lblSignOut.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(lblSignOut.Text, lblSignOut.Font);
                    int newX = Math.Max(screenWidth - (int)textSize.Width - 10, 0);
                    lblSignOut.Location = new Point(newX, lblSignOut.Location.Y);
                    lblSignOut.Width = (int)textSize.Width + 10;
                }

                if (USBCommunication.showProductSelection)
                {
                    //lblProdcutSelect_Click(lblProdcutSelect, EventArgs.Empty); //null

                    frmProductType frmPro = new frmProductType(this);
                    //DialogResult result = frmPro.ShowDialog(this);
                    frmPro.Show(this);
                }

                BindData();
               
            }
            catch (Exception)
            {
            }
        }

        public void BindData()
        {
            if (frmProductType.frmProductStatus)
                BindGridView();
            else
                lblPleaseSelectProduct.Visible = true;

            if (grdPcbType.Rows.Count > 0)
            {
                lblPleaseSelectProduct.Visible = false;
                BindDefaultPCBTestCases();
            }
            else
                lblPleaseSelectProduct.Visible = true;           
        }

        private void BindDefaultPCBTestCases()
        {
            int rowIndex = 0;  // e.RowIndex;
            DataGridViewRow row = grdPcbType.Rows[rowIndex];
            lblPcbType.Text = grdPcbType.Rows[rowIndex].Cells[1].Value.ToString();
            int PcbTypeID = Convert.ToInt32(grdPcbType.Rows[rowIndex].Cells[0].Value.ToString());
            lblPcbTypeId.Text = PcbTypeID.ToString();
            // OpenSerialPort();
            BindPCBAllTestCases(PcbTypeID);
            grdPcbType.Rows[0].Selected = true;
        }

        //Get all connected devices
        private List<string> GetConnectedUSBDevices()
        {
            List<string> _devices = new List<string>();         
            ManagementObjectCollection collection; // Create a object of ManagementObjectCollection class which is hold the list of connected devices
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub")) // This line fetch all the connected device with windows query
            {
                collection = searcher.Get();
            }
            foreach (var device in collection)
            {
                //_devices.Add(new PCBTest1(
                // (string)device.GetPropertyValue("DeviceID"),
                PIdVIdDeviceID = (string)device.GetPropertyValue("PNPDeviceID");
                USBDescription = (string)device.GetPropertyValue("Description");
                // ));
                if(USBDescription== "USB Composite Device")      //if (PIdVIdDeviceID == "USB\\VID_067B&PID_2303") 
                {
                    OpenSerialPort();
                }               
            }
            collection.Dispose();
            return _devices;
        }
        // Gridview Get all PCB Type
        public void BindGridView()
        {
            try
            {
                //grdPcbType.ClearSelection();
                grdPcbType.DataSource = null;
                DataTable dt = _objDal.GetPCBType(GlobalInformation.ProductTypeID);
                if (dt.Rows.Count > 0)
                {                  
                    grdPcbType.DataSource = dt;
                    grdPcbType.Columns[0].Visible = false;
                    grdPcbType.Columns[1].Width = 204;
                    grdPcbType.Columns[2].Visible = false;
                    grdPcbType.RowHeadersVisible = true;
                    this.grdPcbType.DefaultCellStyle.Font = new Font(" Microsoft Sans Serif", 12);
                    grdPcbType.Visible = true;
                    lblPleaseSelectProduct.Visible = false;
                }
                else
                {
                    isVisible(false);
                    lblPleaseSelectProduct.Visible = true;
                    // _msgbox.ShowBox("Records not found");                                      
                }
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }

        private void isVisible(bool visible)
        {
            // ShowSerialNumberPrefix(PcbTypeID);

            lblSerial.Visible = visible;
            lblStatusPCB.Visible = visible;
            lblPcbStatus.Visible = visible;
            lblPcbStatus.Text = "-";
            grpboxMaual.Visible = visible;
            grpboxHybrid.Visible = visible;
            grpAutoTest.Visible = visible;
            grdPcbType.Visible = visible;
            txtserialNo.Visible = visible;
        }

        private void GrdPcbType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!isTestRunning)
                {
                    int rowIndex = e.RowIndex;
                    DataGridViewRow row = grdPcbType.Rows[rowIndex];
                    lblPcbType.Text = grdPcbType.Rows[rowIndex].Cells[1].Value.ToString();
                    int PcbTypeID = Convert.ToInt32(grdPcbType.Rows[rowIndex].Cells[0].Value.ToString());
                    lblPcbTypeId.Text = PcbTypeID.ToString();
                    OpenSerialPort();
                    BindPCBAllTestCases(PcbTypeID);
                }
                else
                    _msgbox.ShowBox(lblPcbType.Text +" "+ "PCB test case is running...");
            }
            catch (Exception ex)
            {
                log.CreateLog(ex
                    .Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        private void BindPCBAllTestCases(int PcbTypeID)
        {
            // Open serial port
            //OpenSerialPort();

            //Serial number validation
            ShowSerialNumberPrefix(PcbTypeID);

            // Bind all test cases
            BindPCBTestCases(PcbTypeID);
            grpAutoTest.Controls.Add(btnAutomaticStart);
            grpboxMaual.Controls.Add(lblStatus);
            //txtserialNo.Text = string.Empty;
            // txtserialNo.Focus();
            ResponseToReceive = string.Empty;
            FrameToSend = string.Empty;
            lblSerial.Visible = true;

            lblStatusPCB.Visible = true;
            lblPcbStatus.Visible = true;
            lblPcbStatus.Text = "-";
            //rdbKeyboardFail.Checked = false;
            //rdbKeyboardPass.Checked = false;
            // ChangeKeyboardButtonsColor();
        }


        // for pcb serial no. sequesnce
        private void ShowSerialNumberPrefix(int PcbTypeId)
        {
            txtserialNo.Text = string.Empty;
            txtserialNo.Visible = true;
            pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
            txtserialNo.Focus();    
        }

        //Bind all PCB test cases
        private void BindPCBTestCases(int PcbTypeID)
        {
            try
            {
                //-----Bind Manual Test Cases-----//
                grpboxMaual.Controls.Clear();
                pnlManualTest.Controls.Clear();
                Font commonFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                Size commonSize = new Size(25, 18);
                DataTable dtmanual = _objDal.GetPCBTestCases(PcbTypeID, "Manual");
                if (dtmanual.Rows.Count > 0)
                {
                    int y = 16;
                    int m = dtmanual.Rows.Count;
                    Label[] labelsManual = new Label[m];
                    Label[] labelsStatus = new Label[m];
                    Label[] lblTestCaseId = new Label[m];
                    Label[] lblSrNo = new Label[m];
                    Label[] lblBatteryVoltRange = new Label[m];

                    Button[] btnStart = new Button[m];
                    Button[] btnSubmit = new Button[m];

                    RadioButton[] rdbPass = new RadioButton[m];
                    RadioButton[] rdbFail = new RadioButton[m];

                    for (int i = 0; i < m; i++)
                    {
                        //Label Serial No
                        lblSrNo[i] = new Label();
                        //lblSrNo[i].Name = "lblSrNo" + i;
                        lblSrNo[i].Text = (i + 1).ToString();
                        lblSrNo[i].Size = commonSize;
                        lblSrNo[i].Location = new Point(5, y);
                        lblSrNo[i].Font =commonFont;
                        pnlManualTest.Controls.Add(lblSrNo[i]);

                       
                        //if (lblPcbType.Text != "Latch")
                        //{
                            //Button for start manual test
                            btnStart[i] = new Button();
                            btnStart[i].Name = "btnStartManual" + i;
                            btnStart[i].Text = "Start";
                            btnStart[i].Font =commonFont;
                            btnStart[i].FlatStyle = FlatStyle.Standard;
                            btnStart[i].Size = new Size(75, 26);
                            btnStart[i].Location = new Point(30, y);
                            pnlManualTest.Controls.Add(btnStart[i]);
                            btnStart[i].Click += new EventHandler(this.BtnStart_Click);
                            toolTip1.SetToolTip(btnStart[i], dtmanual.Rows[i]["Parameter"].ToString());
                        //}
                        // Label TestCase Id
                        lblTestCaseId[i] = new Label();
                        lblTestCaseId[i].Name = "lblTestCaseId" + i;
                        lblTestCaseId[i].Text = "TestCaseID";
                        lblTestCaseId[i].Location = new Point(17, y);
                        pnlManualTest.Controls.Add(lblTestCaseId[i]);
                        lblTestCaseId[i].Visible = false;

                        // Label test Cases name
                        labelsManual[i] = new Label();
                        labelsManual[i].Text = dtmanual.Rows[i]["TestCaseName"].ToString();
                        labelsManual[i].Font =commonFont;
                        labelsManual[i].Location = new Point(115, y);
                        labelsManual[i].Name = "lbl_" + i;
                        labelsManual[i].AutoSize = true;
                        pnlManualTest.Controls.Add(labelsManual[i]);

                        //Radiobutton for Pass
                        rdbPass[i] = new RadioButton();
                        rdbPass[i].Name = "rdbPass" + i;
                        rdbPass[i].Text = "Pass";
                        rdbPass[i].Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular); 
                        rdbPass[i].Location = new Point(320, y);
                        pnlManualTest.Controls.Add(rdbPass[i]);
                        // rdbPass[i].Enabled = false;
                        rdbPass[i].CheckedChanged += RdbPass_CheckedChanged;

                        // Rediobutton for Fail
                        rdbFail[i] = new RadioButton();
                        rdbFail[i].Name = "rdbFail" + i;
                        rdbFail[i].Text = "Fail";
                        rdbFail[i].Font = commonFont;
                        rdbFail[i].Location = new Point(430, y);
                        pnlManualTest.Controls.Add(rdbFail[i]);
                        // rdbFail[i].Enabled = false;
                        rdbFail[i].CheckedChanged += RdbFail_CheckedChanged;

                        // label for status Pass/Fail
                        labelsStatus[i] = new Label();
                        labelsStatus[i].Text = string.Empty;
                        labelsStatus[i].Font =commonFont;
                        labelsStatus[i].Location = new Point(530, y);
                        labelsStatus[i].Name = "lblStatus_" + i;
                        pnlManualTest.Controls.Add(labelsStatus[i]);
                        labelsStatus[i].Enabled = false;

                        labelsManual[i].Enabled = false;
                        rdbPass[i].Enabled = false;
                        rdbFail[i].Enabled = false;

                        y = y + 40;
                        lnkNewTest.Visible = false;
                    }
                    grpboxMaual.Controls.Add(pnlManualTest);
                    grpboxMaual.Visible = true;
                }
                else
                {
                    // label for No Manual Test
                    //Label lblNoTest = new Label();
                    //lblNoTest = new Label();
                    //lblNoTest.Text = "No manual test for " + lblPcbType.Text;
                    //lblNoTest.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    //lblNoTest.BackColor = Color.Transparent;
                    //lblNoTest.ForeColor = Color.Brown;
                    //lblNoTest.AutoSize = true;
                    //lblNoTest.Location = new Point(230, 100);
                    //grpboxMaual.Controls.Add(lblNoTest);
                    //lblNoTest.Enabled = true;
                    grpboxMaual.Visible = false;
                }
                //----- Bind Auto Test Cases-----//
                grpAutoTest.Controls.Clear();
                DataTable dtAuto = _objDal.GetPCBTestCases(PcbTypeID, "Auto");
                int a = dtAuto.Rows.Count;
                int ya = 60;
                Label[] labelsAuto = new Label[a];
                PictureBox[] picbox = new PictureBox[a];
                Label[] labelsAutoStatus = new Label[a];
                Label[] lblBatVoltg = new Label[a];
              
                if (dtAuto.Rows.Count > 0)
                {
                    for (int j = 0; j < dtAuto.Rows.Count; j++)
                    {
                        //Picturebox for arrow image       
                        picbox[j] = new PictureBox();
                        picbox[j].Name = "pictureBox";
                        picbox[j].Size = new Size(20, 20);
                        picbox[j].Location = new Point(25, ya + 1);
                        picbox[j].Image = ESD_Testjig.Properties.Resources.icon_arrow;
                        grpAutoTest.Controls.Add(picbox[j]);

                        //label for testcase name
                        labelsAuto[j] = new Label();
                        labelsAuto[j].Text = dtAuto.Rows[j]["TestCaseName"].ToString();
                        labelsAuto[j].Name = "lblAutoTestCase_" + j;
                        labelsAuto[j].Font =commonFont;
                        labelsAuto[j].Location = new Point(50, ya);
                        labelsAuto[j].AutoSize = true;
                        grpAutoTest.Controls.Add(labelsAuto[j]);
                        labelsAuto[j].Enabled = false;

                        // label for show battery voltage
                        lblBatVoltg[j] = new Label();
                        lblBatVoltg[j].Text = string.Empty;
                        lblBatVoltg[j].Font = commonFont;
                        lblBatVoltg[j].Location = new Point(295, ya);      //280
                        lblBatVoltg[j].Name = "lblBatVoltg_" + j;
                        grpAutoTest.Controls.Add(lblBatVoltg[j]);
                        lblBatVoltg[j].Enabled = false;

                        // label for status pass/fail
                        labelsAutoStatus[j] = new Label();
                        labelsAutoStatus[j].Text = string.Empty;
                        labelsAutoStatus[j].Font = commonFont;
                        labelsAutoStatus[j].Location = new Point(255, ya);      //230
                        labelsAutoStatus[j].Name = "lblAutoStatus_" + j;
                        grpAutoTest.Controls.Add(labelsAutoStatus[j]);
                        labelsAutoStatus[j].Enabled = false;
                        ya = ya + 30;
                    }

                    //// label for Motor Rotation
                    //Label lblMotorRotation = new Label();
                    //lblMotorRotation.Text = string.Empty;    //"Clockwise:0.023, Anticlockwise:0.265";
                    //lblMotorRotation.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                    //lblMotorRotation.Location = new Point(85, ya);
                    //lblMotorRotation.Size = new Size(300, 100);
                    //lblMotorRotation.Name = "lblMotorReading";
                    //grpAutoTest.Controls.Add(lblMotorRotation);
                    //lblMotorRotation.Enabled = false;
                    btnAutomaticStart.Visible = true;
                    grpAutoTest.Visible = true;
                }
                else
                {
                    grpAutoTest.Visible = false;
                }

                //-----Bind Hybrid Test Cases-----//    

               

                grpboxHybrid.Controls.Clear();
                pnlHybridTest.Controls.Clear();
                DataTable dtHybrid = _objDal.GetPCBTestCases(PcbTypeID, "Hybrid");
                if (dtHybrid.Rows.Count > 0)
                {
                    int y = 12;
                    int m = dtHybrid.Rows.Count;
                    Label[] labelsHybrid = new Label[m];
                    Label[] labelsHStatus = new Label[m];
                    Label[] lblHTestCaseId = new Label[m];
                    Label[] lblHSrNo = new Label[m];

                    Button[] btnHStart = new Button[m];
                    Button[] btnHResult = new Button[m];

                    RadioButton[] rdbHPass = new RadioButton[m];
                    RadioButton[] rdbHFail = new RadioButton[m];

                    for (int i = 0; i < m; i++)
                    {
                        //Label Serial No
                        lblHSrNo[i] = new Label();
                        lblHSrNo[i].Text = (i + 1).ToString();
                        lblHSrNo[i].Size = commonSize;
                        lblHSrNo[i].Location = new Point(5, y);
                        lblHSrNo[i].Font = commonFont;
                        pnlHybridTest.Controls.Add(lblHSrNo[i]);

                        //Button for start test
                        btnHStart[i] = new Button();
                        btnHStart[i].Name = "btnStartHybrid" + i;
                        btnHStart[i].Text = "Start";
                        btnHStart[i].Font =commonFont;
                        btnHStart[i].FlatStyle = FlatStyle.Standard;
                        btnHStart[i].Size = new Size(75, 26);
                        btnHStart[i].Location = new Point(30, y);
                        pnlHybridTest.Controls.Add(btnHStart[i]);
                        btnHStart[i].Click += new EventHandler(this.BtnStartHybrid_Click);

                        //ToolTip
                        toolTip1.SetToolTip(btnHStart[i], dtHybrid.Rows[i]["Parameter"].ToString());

                        // Label TestCase Id
                        lblHTestCaseId[i] = new Label();
                        lblHTestCaseId[i].Name = "lblHTestCaseId" + i;
                        lblHTestCaseId[i].Text = "TestCaseID";
                        lblHTestCaseId[i].Location = new Point(17, 60);
                        pnlHybridTest.Controls.Add(lblHTestCaseId[i]);
                        lblHTestCaseId[i].Visible = false;

                        // Label test Cases name
                        labelsHybrid[i] = new Label();
                        labelsHybrid[i].Text = dtHybrid.Rows[i]["TestCaseName"].ToString();
                        labelsHybrid[i].Font = commonFont;
                        labelsHybrid[i].Location = new Point(115, y);
                        labelsHybrid[i].Name = "labelsHybrid" + i;
                        labelsHybrid[i].AutoSize = true;
                        pnlHybridTest.Controls.Add(labelsHybrid[i]);
                        labelsHybrid[i].Enabled = false;

                        btnHResult[i] = new Button();
                        btnHResult[i].Name = "btnHResult" + i;
                        btnHResult[i].Text = "";
                        btnHResult[i].Font =commonFont;
                        btnHResult[i].FlatStyle = FlatStyle.Standard;
                        btnHResult[i].Size = new Size(30, 26);
                        btnHResult[i].Location = new Point(285, y);
                        btnHResult[i].BackColor = Color.Transparent;
                        btnHResult[i].BackgroundImageLayout = ImageLayout.None;
                        btnHResult[i].FlatAppearance.BorderColor = Color.White;
                        btnHResult[i].Enabled = false;
                        pnlHybridTest.Controls.Add(btnHResult[i]);

                        //Radiobutton for Pass
                        rdbHPass[i] = new RadioButton();
                        rdbHPass[i].Name = "rdbHPass" + i;
                        rdbHPass[i].Text = "Pass";
                        rdbHPass[i].Font =commonFont;
                        rdbHPass[i].Location = new Point(320, y);
                        pnlHybridTest.Controls.Add(rdbHPass[i]);
                        rdbHPass[i].Enabled = false;
                        rdbHPass[i].CheckedChanged += RdbHPass_CheckedChanged;

                        //Rediobutton for Fail
                        rdbHFail[i] = new RadioButton();
                        rdbHFail[i].Name = "rdbHFail" + i;
                        rdbHFail[i].Text = "Fail";
                        rdbHFail[i].Font =commonFont;
                        rdbHFail[i].Location = new Point(430, y);
                        pnlHybridTest.Controls.Add(rdbHFail[i]);
                        rdbHFail[i].Enabled = false;
                        rdbHFail[i].CheckedChanged += RdbHFail_CheckedChanged;

                        //label for status Pass/Fail
                        labelsHStatus[i] = new Label();
                        labelsHStatus[i].Text = string.Empty;
                        labelsHStatus[i].Font =commonFont;
                        labelsHStatus[i].Location = new Point(530, y);
                        labelsHStatus[i].Name = "labelsHStatus" + i;
                        pnlHybridTest.Controls.Add(labelsHStatus[i]);
                        labelsHStatus[i].Enabled = false;
                        y = y + 40;
                        lnkNewTest.Visible = false;
                    }
                    grpboxHybrid.Controls.Add(pnlHybridTest);
                    grpboxHybrid.Visible = true;
                }
                else
                {
                    grpboxHybrid.Visible = false;
                }

                //-----Bind Keypad test-----// For HHD Controller BP 120124
                pnlKeyboardTest.Controls.Clear();
                #region COMMENT KEYBOARD CODE
                //if (lblPcbTypeId.Text == "4" || lblPcbTypeId.Text=="6")   // 4-is for hhd controller|| 6-keypad pcb
                //{
                //    pnlKeyboardTest.Visible = true;
                //    pnlKeyboardTest.Controls.Add(btnKeyboardStart);
                //    pnlKeyboardTest.Controls.Add(btn1);
                //    pnlKeyboardTest.Controls.Add(btn2);
                //    pnlKeyboardTest.Controls.Add(btn3);
                //    pnlKeyboardTest.Controls.Add(btn4);
                //    pnlKeyboardTest.Controls.Add(btn5);
                //    pnlKeyboardTest.Controls.Add(btn6);
                //    pnlKeyboardTest.Controls.Add(btn7);
                //    pnlKeyboardTest.Controls.Add(btn8);
                //    pnlKeyboardTest.Controls.Add(btn9);
                //    pnlKeyboardTest.Controls.Add(btnStar);
                //    pnlKeyboardTest.Controls.Add(btn0);
                //    pnlKeyboardTest.Controls.Add(btnHash);
                //    pnlKeyboardTest.Controls.Add(btnUP);
                //    pnlKeyboardTest.Controls.Add(btnDown);
                //    pnlKeyboardTest.Controls.Add(btnRefresh);
                //    pnlKeyboardTest.Controls.Add(btnEnter);                   
                //    pnlKeyboardTest.Controls.Add(rdbKeyboardPass);
                //    pnlKeyboardTest.Controls.Add(rdbKeyboardFail);
                //    pnlKeyboardTest.Controls.Add(lblKeyboardStatus);
                //    grpboxKeyboardTest.Controls.Add(pnlKeyboardTest);
                //    grpboxKeyboardTest.Visible = true;                   
                //    rdbKeyboardPass.Enabled = false;
                //    rdbKeyboardFail.Enabled = false;
                //    lblKeyboardStatus.Enabled = false;
                //    lblKeyboardStatus.Text = "-";
                //}
                //else
                //{
                //    grpboxKeyboardTest.Visible = false;
                //    // Remove background color of keyboard buttons
                //     ChangeKeyboardButtonsColor();
                //}
                #endregion
            }
            catch 
            { }
        }


        private void ChangeKeyboardButtonsColor()
        {
            btn1.BackColor = Color.Transparent;
            btn2.BackColor = Color.Transparent;
            btn3.BackColor = Color.Transparent;
            btn4.BackColor = Color.Transparent;
            btn5.BackColor = Color.Transparent;
            btn6.BackColor = Color.Transparent;
            btn7.BackColor = Color.Transparent;
            btn8.BackColor = Color.Transparent;
            btn9.BackColor = Color.Transparent;
            btnStar.BackColor = Color.Transparent;
            btn0.BackColor = Color.Transparent;
            btnHash.BackColor = Color.Transparent;
            btnUP.BackColor = Color.Transparent;
            btnDown.BackColor = Color.Transparent;
            btnRefresh.BackColor = Color.Transparent;
            btnEnter.BackColor = Color.Transparent;
        }

        //Hybrid Test Pass
        public void RdbHPass_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lnkNewTest.Visible = true;
                DataTable dtHybrid = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Hybrid");
                int m = dtHybrid.Rows.Count;
                var rdb_pass = pnlHybridTest.Controls.OfType<RadioButton>();
                RadioButton rdbPassChecked = sender as RadioButton;
                int j = 0;

                this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);

                for (int i = 0; i < 5; i++) // Loop through the radio buttons
                {
                    RadioButton rdb = rdb_pass.FirstOrDefault(r => r.Name == "rdbHPass" + (j + i));
                    if (rdbPassChecked.Checked && rdb != null && rdb.Enabled)
                    {
                        SetHybridRdbPass(dtHybrid, j, i, i + 1, rdb_pass);
                    }
                }

                this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));

                ////rdb pass 0
                //if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbHPass" + j && rdbPassChecked.Enabled == true)
                //{
                //    SetHybridRdbPass(dtHybrid, j, 0, 1, rdb_pass);
                //    //if (lblPcbType.Text == "Base Board") //because base board pcb has multiple test.
                //    //    SetHybridTestCases(dtHybrid, 1);
                //}
                ////rdb pass 1

                //else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbHPass" + (j + 1) && rdbPassChecked.Enabled == true)
                //{
                //    SetHybridRdbPass(dtHybrid, j, 1, 2, rdb_pass);
                //    //SetHybridTestCases(dtHybrid, 2);
                //}
                ////rdb pass 2

                //else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbHPass" + (j + 2) && rdbPassChecked.Enabled == true)
                //{
                //    SetHybridRdbPass(dtHybrid, j, 2, 3, rdb_pass);
                //   // SetHybridTestCases(dtHybrid, 3);
                //}
                ////rdb pass 3

                //else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbHPass" + (j + 3) && rdbPassChecked.Enabled == true)
                //{
                //    SetHybridRdbPass(dtHybrid, j, 3, 4, rdb_pass);                   
                //} 
                //else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbHPass" + (j + 4) && rdbPassChecked.Enabled == true)
                //{
                //    SetHybridRdbPass(dtHybrid, j, 4, 5, rdb_pass);
                //}
                //this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
                ////Show_PCBStatus();
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }
        
        //Save hybrid Pass Test
        private void SetHybridRdbPass(DataTable dtHybrid, int j, int a, int b, IEnumerable<RadioButton> rdb_pass)
        {
            try
            {
                //Optimized new code 
                USBCommunication.TimerStopTest();
                var lbl_Status = pnlHybridTest.Controls.OfType<Label>()
                                            .Where(u => u.Text != "TestCaseID")
                                            .ToList();  

                int pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);

                foreach (Label lbstatus in lbl_Status)
                {
                    string lblstatusName = lbstatus.Name;

                    if (lblstatusName == "labelsHStatus" + (j + a))
                    {
                        lbstatus.Enabled = true;
                        lbstatus.Text = "Pass";
                        lbstatus.ForeColor = Color.Green;

                        int testCaseId = Convert.ToInt32(dtHybrid.Rows[a]["TestCaseID"]);
                        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Hybrid", "");
                    }
                    else if (lblstatusName.StartsWith("labelsHybrid"))
                    {
                        lbstatus.Enabled = false;
                    }
                }

                foreach (RadioButton rdo in rdb_pass.Where(r => r.Name.StartsWith("rdbH")))
                {
                    rdo.Checked = false;
                    rdo.Enabled = false;
                }

                // Stop frame for Hybrid test           
                if (pcbtypeid == 1)
                {
                    serialPort1.Write("*12A000004STOPFA1D#");
                }

                //Commneted by BBP 12042024 
                //USBCommunication.TimerStopTest();
                //var lbl_Status = pnlHybridTest.Controls.OfType<Label>();
                //lbl_Status = lbl_Status.Where(u => u.Text != "TestCaseID").ToList();    //Filter status list
                //pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                //int testCaseId=0;
                //foreach (Label lbstatus in lbl_Status)
                //{
                //    string lblstatusName1 = lbstatus.Name;                   
                //    if (lblstatusName1 == "labelsHStatus" + (j + a))
                //    {
                //        lbstatus.Enabled = true;
                //        lbstatus.Text = "Pass";
                //        lbstatus.ForeColor = Color.Green;
                //        testCaseId = Convert.ToInt32(dtHybrid.Rows[a]["TestCaseID"]);
                //        //---Save test case result
                //        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Hybrid","");
                //    }
                //    if (lblstatusName1 == "labelsHybrid" + (j + a))
                //    {
                //        lbstatus.Enabled = false;
                //    }
                //}
                //foreach (RadioButton rdo in rdb_pass)
                //{
                //    string lblstatusName = rdo.Name;
                //    if (lblstatusName == "rdbHPass" + (j + a))
                //    {
                //        rdo.Checked = false;
                //    }
                //    if (lblstatusName == "rdbHFail" + (j + a))
                //    {
                //        rdo.Checked = false;
                //    }
                //    rdo.Enabled = false;
                //}
                //// Stop frame for Hybrid test           
                //if (pcbtypeid == 1)
                //serialPort1.Write("*12A000004STOPFA1D#");              

            }     
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        // Hybrid test Fail
        public void RdbHFail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);

                DataTable dtHybrid = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Hybrid");
                int m = dtHybrid.Rows.Count;
                var rdb_pass = pnlHybridTest.Controls.OfType<RadioButton>();
                RadioButton rdbFailChecked = sender as RadioButton;
                int j = 0;

                // rdb Fail 0
                if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbHFail" + j && rdbFailChecked.Enabled == true)
                {
                    SetHybridRdbFail(dtHybrid, j, 0, 1, rdb_pass, rdbFailChecked);
                }
                //rdb Fail 1
                else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbHFail" + (j + 1) && rdbFailChecked.Enabled == true)
                {
                    SetHybridRdbFail(dtHybrid, j, 1, 2, rdb_pass, rdbFailChecked);
                }
                // rdb Fail 2
                else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbHFail" + (j + 2) && rdbFailChecked.Enabled == true)
                {
                    SetHybridRdbFail(dtHybrid, j, 2, 3, rdb_pass, rdbFailChecked);
                }
                // rdb Fail 3
                else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbHFail" + (j + 3) && rdbFailChecked.Enabled == true)
                {
                    SetHybridRdbFail(dtHybrid, j, 3, 4, rdb_pass, rdbFailChecked);
                }
                // rdb Fail 4
                else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbHFail" + (j + 4) && rdbFailChecked.Enabled == true)
                {
                    //SetHybridRdbFail(dtHybrid, j, 4, 5, rdb_pass, rdbFailChecked);
                }
                //rdb Fail 5

                else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbHFail" + (j + 5) && rdbFailChecked.Enabled == true)
                {
                    SetHybridRdbFail(dtHybrid, j, 5, 6, rdb_pass, rdbFailChecked);
                }
                //rdb Fail 6

                else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbHFail" + (j + 6) && rdbFailChecked.Enabled == true)
                {
                    SetHybridRdbFail(dtHybrid, j, 6, 7, rdb_pass, rdbFailChecked);
                }
                //rdb Fail 7
                else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbHFail" + (j + 7) && rdbFailChecked.Enabled == true)
                {
                    SetHybridRdbFail(dtHybrid, j, 7, 8, rdb_pass, rdbFailChecked);
                }
                this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
                //Show_PCBStatus();
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        private void SetHybridRdbFail(DataTable dtHybrid, int j, int a, int b, IEnumerable<RadioButton> rdb_pass, RadioButton rdbFailChecked)
        {
            try
            {
                USBCommunication.TimerStopTest();
                int testCaseId = Convert.ToInt32(dtHybrid.Rows[a]["TestCaseID"]);
                pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                lnkNewTest.Visible = true;

                var lbl_Status = pnlHybridTest.Controls.OfType<Label>()
                                    .Where(u => u.Text != "TestCaseID")
                                    .ToList();

                foreach (Label lbstatus in lbl_Status)
                {
                    string lblstatusName = lbstatus.Name;
                    if (lblstatusName == "labelsHStatus" + (j + a))
                    {
                        lbstatus.Enabled = true;
                        lbstatus.Text = "Fail";
                        lbstatus.ForeColor = Color.Red;

                        foreach (RadioButton rdo in rdb_pass.Where(r => r.Name == "rdbHPass" + (j + a) || r.Name == "rdbHPass" + (j + b) ||
                                                                        r.Name == "rdbHFail" + (j + a) || r.Name == "rdbHFail" + (j + b)))
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }

                        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Hybrid", "");
                    }
                    else if (lblstatusName == "labelsHStatus" + (j + a))
                    {
                        lbstatus.Enabled = true;
                    }
                    else if (lblstatusName.StartsWith("labelsHybrid"))
                    {
                        lbstatus.Enabled = false;
                    }
                }
                // Stop frame for Hybrid test   BP 120124         
                if (pcbtypeid == 1)
                {
                    serialPort1.Write(" *12A000004STOPFA1D#");
                }

                //Commented by BBP 17042024 (Above code is optimized)
                //USBCommunication.TimerStopTest();
                //int testCaseId = Convert.ToInt32(dtHybrid.Rows[a]["TestCaseID"]);
                //pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                //lnkNewTest.Visible = true;
                //var lbl_Status = pnlHybridTest.Controls.OfType<Label>();
                //// Filter status list
                //lbl_Status = lbl_Status.Where(u => u.Text != "TestCaseID").ToList();
                //foreach (Label lbstatus in lbl_Status)
                //{
                //    string lblstatusName = lbstatus.Name;
                //    if (lblstatusName == "labelsHStatus" + (j + a))
                //    {
                //        lbstatus.Enabled = true;
                //        lbstatus.Text = "Fail";
                //        lbstatus.Enabled = true;
                //        lbstatus.ForeColor = Color.Red;
                //        foreach (RadioButton rdo in rdb_pass)
                //        {
                //            string lblrdbName = rdo.Name;
                //            if (lblrdbName == "rdbHPass" + (j + a))
                //            {
                //                rdo.Enabled = false;
                //            }
                //            if (lblrdbName == "rdbHPass" + (j + b))
                //            {
                //                rdo.Enabled = false;
                //            }
                //            if (lblrdbName == "rdbHFail" + (j + a))
                //            {
                //                rdo.Enabled = false;
                //            }
                //            if (lblrdbName == "rdbHFail" + (j + b))
                //            {
                //                rdo.Enabled = false;
                //            }
                //            rdo.Checked = false;
                //        }
                //        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Hybrid","");
                //    }
                //    if (lblstatusName == "labelsHStatus" + (j + a))
                //    {
                //        lbstatus.Enabled = true;
                //    }
                //    if (lblstatusName == "labelsHybrid" + (j + a))
                //    {
                //        lbstatus.Enabled = false;
                //    }
                //    if (lblstatusName == "labelsHybrid" + (j + b))
                //    {
                //        lbstatus.Enabled = false;
                //    }
                //    // Stop frame for Hybrid test   BP 120124         
                //    if (pcbtypeid == 1)
                //    {                      
                //        serialPort1.Write(" *12A000004STOPFA1D#");
                //    }
                //    //if (pcbtypeid == 2)
                //    //{
                //    //    serialPort1.Write("*12B000004STOP82E7#");
                //    //}
                //}
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }
        
        // Start Hybrid Tests
        private void BtnStartHybrid_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblPcbTypeId.Text != string.Empty)
                {
                    USBCommunication.TimerStopTest();
                    DataTable dtHybrid = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Hybrid");
                    Button buttonClicked = sender as Button;
                    //Serial number not empty or less than 11 digits.
                    if (txtserialNo.Text.Length != 11)
                    {
                        SerialNoMsgBox.ShowBox();
                        txtserialNo.Focus();
                        txtserialNo.SelectionStart = txtserialNo.Text.Length;
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {                           
                            if (buttonClicked.Name == "btnStartHybrid" + i)
                            {
                                SetHybridTestCases(dtHybrid, i);
                                break;              // Exit the loop once the matching button is found
                            }
                        }

                        //Commented by BBP 17042024

                        ////this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
                        //// Start manual test 0
                        //if (buttonClicked.Name == "btnStartHybrid" + 0)
                        //{
                        //    SetHybridTestCases(dtHybrid, 0);
                        //}
                        //// Start manual test 1
                        //else if (buttonClicked.Name == "btnStartHybrid" + 1)
                        //{
                        //    SetHybridTestCases(dtHybrid, 1);
                        //}
                        //// Start manual test 2
                        //else if (buttonClicked.Name == "btnStartHybrid" + 2)
                        //{
                        //    SetHybridTestCases(dtHybrid, 2);
                        //}
                        ////Start manual test 3

                        //else if (buttonClicked.Name == "btnStartHybrid" + 3)
                        //{
                        //    SetHybridTestCases(dtHybrid, 3);
                        //}
                        //// Start manual test 4
                        //else if (buttonClicked.Name == "btnStartHybrid" + 4)
                        //{
                        //    SetHybridTestCases(dtHybrid, 4);
                        //}                                   
                    }
                }
                else
                {
                    _msgbox.ShowBox("Please select PCB type...");
                }
            }
            catch (Exception ex)
            {              
                ShowException(ex.Message);
            }
        }

        private void SetHybridTestCases(DataTable dtHybrid, int a)
        {
            this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
            Thread.Sleep(200);   //this is for not getting response immediatly.
            var rdb_pass = pnlHybridTest.Controls.OfType<RadioButton>();
            var lbl_Status = pnlHybridTest.Controls.OfType<Label>();
            pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
            // remove unecessary label from list
            lbl_Status = lbl_Status.Where(u => u.Text != "TestCaseID").ToList();                                   
            int testCaseId = Convert.ToInt32(dtHybrid.Rows[a]["TestCaseID"]);
            string testType = dtHybrid.Rows[a]["TestType"].ToString();
            OpenSerialPort();
            if (serialPort1.PortName == "USBNotConnected")
            {
                OpenSerialPort();
            }
            else
            {
                if (btnUsbStatus.BackColor == Color.Silver)
                    return;

                Messages = "";              
                Cursor.Current = Cursors.WaitCursor;
                WriteDataToSerialPort(dtHybrid, a);
                Cursor.Current = Cursors.Arrow;
                // this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
                
                if (Messages == "Frame sent successfully")             
                {
                    this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                    // MyMessageBox.ShowBox("Please press the " + dtHybrid.Rows[a]["TestCaseName"]);

                    // Read continueously reponse                

                    foreach (Label lbstatus in lbl_Status)
                    {
                        string lblstatusName = lbstatus.Name;
                        if (lblstatusName == "labelsHStatus" + a)   
                        {
                            lbstatus.Enabled = true;
                            lbstatus.Text = string.Empty;
                            //check timer counter for wait 30 sec
                            USBCommunication.TimerStartTest(lbstatus);
                        }
                        if (lblstatusName == "labelsHybrid" + a)    
                        {
                            lbstatus.Enabled = true;
                            USBCommunication.lblTestName = lbstatus;                      
                        }
                    }
                    foreach (RadioButton rdo in rdb_pass)
                    {
                        string lblstatusName = rdo.Name;
                        if (lblstatusName == "rdbHPass" + a)
                        {
                            rdo.Enabled = true;
                            rdo.Checked = false;
                            USBCommunication.rdPass = rdo;
                        }
                        if (lblstatusName == "rdbHFail" + a)
                        {
                            rdo.Enabled = true;
                            rdo.Checked = false;
                            USBCommunication.rdFail = rdo;
                        }
                    }
                    //assign value to the parameters in usbcommunication.cs 
                    USBCommunication.testCaseId = testCaseId;
                    USBCommunication.LoginUserId = LoginUserId;
                    USBCommunication.PcbTypeId = Convert.ToInt32(lblPcbTypeId.Text);
                    USBCommunication.FrameToSend = FrameToSend;
                    USBCommunication.ResponseToReceive = ResponseToReceive;
                    USBCommunication.SerialNo = txtserialNo.Text;
                    USBCommunication.Pcbtype = lblPcbType.Text;
                }
                else if (Messages == "Timeout")
                {
                    HybridTestFailOrTimeout(testCaseId, "Timeout", a);

                }
                else                                     //----- for if exception occures,display test fail  
                {
                    HybridTestFailOrTimeout(testCaseId, "Fail", a);
                   
                    SaveTestCaseResult(testCaseId, "Fail", FrameToSend, ResponseToReceive, "Hybrid", "");
                }
                lnkNewTest.Visible = true;
            }
        }

        private void HybridTestFailOrTimeout(int testCaseId, string status, int a)
        {
            var rdb_pass = pnlHybridTest.Controls.OfType<RadioButton>();
            var lbl_Status = pnlHybridTest.Controls.OfType<Label>();
            foreach (Label lbstatus in lbl_Status)
            {
                string lblstatusName = lbstatus.Name;
                if (lblstatusName == "labelsHStatus" + a)
                {
                    lbstatus.Enabled = true;
                    lbstatus.Text = status;     //"Timeout";
                    lbstatus.ForeColor = Color.Red;
                }
            }
            foreach (RadioButton rdo in rdb_pass)
            {
                string lblrdbName = rdo.Name;
                if (lblrdbName == "rdbHPass" + a)
                {
                    rdo.Enabled = false;
                    rdo.Checked = false;
                }
                if (lblrdbName == "rdbHFail" + a)
                {
                    rdo.Enabled = false;
                    rdo.Checked = false;
                }
            }
            SaveTestCaseResult(testCaseId, status, FrameToSend, ResponseToReceive, "Hybrid", "");
        }

        public void BtnStart_Click(object sender, System.EventArgs e)
        {
            try
            {
                // New optimized code 19042024

                if (lblPcbTypeId.Text != string.Empty)
                {
                    DataTable dtmanual = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Manual");

                    Button buttonClicked = sender as Button;
                    //Serial number not empty or less than 11 digits.
                    if (txtserialNo.Text.Length != 11)
                    {
                        SerialNoMsgBox.ShowBox();
                        txtserialNo.Focus();
                        txtserialNo.SelectionStart = txtserialNo.Text.Length;
                    }
                    else
                    {
                        this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);

                        int testIndex = -1;
                        string buttonName = buttonClicked.Name;
                        if (buttonName.StartsWith("btnStartManual") && int.TryParse(buttonName.Substring(14), out testIndex))
                        {
                            SetManualTestCases(dtmanual, testIndex);
                        }
                    }
                }
                else
                {
                    _msgbox.ShowBox("Please select PCB type...");
                }

                //if (lblPcbTypeId.Text != string.Empty)
                //{
                //    DataTable dtmanual = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Manual");

                //    Button buttonClicked = sender as Button;
                //    //Serial number not empty or less than 11 digits.
                //    // if (txtserialNo.Text.Trim() == string.Empty)      
                //    if(txtserialNo.Text.Length != 11)
                //    { 
                //        SerialNoMsgBox.ShowBox();
                //        txtserialNo.Focus();
                //        txtserialNo.SelectionStart = txtserialNo.Text.Length;
                //    }
                //    else
                //    {
                //        this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);

                //        // Start manual test 0
                //        if (buttonClicked.Name == "btnStartManual" + 0)
                //        {
                //            //Commented by bbp 160123
                //            ////For Communication with PC test of HHD Controller || Encoder UART test || RFID
                //            //if (pcbtypeid == 4 || pcbtypeid==7 || pcbtypeid==8)   
                //            //        HHDManualAutoTest(dtmanual, 0);
                //            //    else
                //            SetManualTestCases(dtmanual, 0);
                //        }
                //        // Start manual test 1
                //        else if (buttonClicked.Name == "btnStartManual" + 1)
                //        {
                //            //Commented by bbp 160123
                //            //if (pcbtypeid == 8)
                //            //    ManualRFIDTestCardWithLock(dtmanual,1);
                //            //else
                //            SetManualTestCases(dtmanual, 1);
                //        }
                //        // Start manual test 2
                //        else if (buttonClicked.Name == "btnStartManual" + 2)
                //        {
                //            //Commented by bbp 160123
                //            //if (pcbtypeid == 8)  //RFID
                //            //    HHDManualAutoTest(dtmanual, 2);
                //            //else
                //            SetManualTestCases(dtmanual, 2);
                //        }
                //        //Start manual test 3

                //        else if (buttonClicked.Name == "btnStartManual" + 3)
                //        {
                //            SetManualTestCases(dtmanual, 3);
                //        }
                //        // Start manual test 4
                //        else if (buttonClicked.Name == "btnStartManual" + 4)
                //        {
                //            SetManualTestCases(dtmanual, 4);
                //        }
                //        //Start manual test 5

                //        else if (buttonClicked.Name == "btnStartManual" + 5)
                //        {
                //            SetManualTestCases(dtmanual, 5);
                //        }
                //        //Start manual test 6
                //        else if (buttonClicked.Name == "btnStartManual" + 6)
                //        {
                //            SetManualTestCases(dtmanual, 6);
                //        }                           
                //        }                          
                //    }
                ////}
                //else
                //{
                //    _msgbox.ShowBox("Please select PCB type...");
                //}
            }
            catch (Exception ex)
            {
                //if (serialPort1 != null && serialPort1.IsOpen)
                //{ serialPort1.Close(); }
                ShowException(ex.Message);
            }
        }

        // For RFID PCB
        private void ManualRFIDTestCardWithLock(DataTable dtmanual, int a)
        {
            var rdb_pass = pnlManualTest.Controls.OfType<RadioButton>();
            var lbl_Status = pnlManualTest.Controls.OfType<Label>();
            // remove unecessary label from list
            lbl_Status = lbl_Status.Where(u => u.Text != "TestCaseID").ToList();
            int testCaseId = Convert.ToInt32(dtmanual.Rows[a]["TestCaseID"]);
          string msgresult = _msgbox.ShowBox("Test RFID card with Lock.");
            if (msgresult.Equals("Yes"))
            {
                foreach (Label lbstatus in lbl_Status)
                {
                    string lblstatusName = lbstatus.Name;
                    if (lblstatusName == "lblStatus_" + a)
                    {
                        lbstatus.Enabled = false;
                        lbstatus.Text = string.Empty;
                    }
                    if (lblstatusName == "lbl_" + a)
                    {
                        lbstatus.Enabled = false;
                    }
                }
                foreach (RadioButton rdo in rdb_pass)
                {
                    string lblrdbName = rdo.Name;
                    if (lblrdbName == "rdbPass" + a)
                    {
                        rdo.Enabled = true;
                        rdo.Checked = false;
                    }
                    if (lblrdbName == "rdbFail" + a)
                    {
                        rdo.Enabled = true;
                        rdo.Checked = false;
                    }
                }
            }
        }

        private void HHDManualAutoTest(DataTable dtmanual, int a)
        {
            var rdb_pass = pnlManualTest.Controls.OfType<RadioButton>();
            var lbl_Status = pnlManualTest.Controls.OfType<Label>();
            // remove unecessary label from list
            lbl_Status = lbl_Status.Where(u => u.Text != "TestCaseID").ToList();
            int testCaseId = Convert.ToInt32(dtmanual.Rows[a]["TestCaseID"]);
            //RadioButton rdbFailChecked = sender as RadioButton;
            OpenSerialPort();
            if (serialPort1.PortName == "USBNotConnected")
            {
                OpenSerialPort();
            }
            else
            {
                Messages = "";
                Cursor.Current = Cursors.WaitCursor;
                WriteDataToSerialPort(dtmanual, a);
                Cursor.Current = Cursors.Arrow;
                if (Messages == "Frame sent successfully")
                {
                    if (ResponseToReceive == dtmanual.Rows[a]["PassFrame"].ToString())
                    {
                        SetManualRdbPass(dtmanual, a, 0, 1, rdb_pass);
                        if (pcbtypeid != 8) { 
                            SetManualTestCases(dtmanual, 1);   //if auto test pass,automatically start next test.
                             }
                    }
                    else
                    {
                        SetManulRdbFail(dtmanual, a, 0, 1, rdb_pass);
                    }                   
                }
                else    //----- for if exception occures,display test fail  
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        string lblstatusName = lbstatus.Name;
                        if (lblstatusName == "lblStatus_" + a)
                        {
                            lbstatus.Enabled = true;
                            lbstatus.Text = "Fail";
                            lbstatus.ForeColor = Color.Red;
                        }
                    }
                    foreach (RadioButton rdo in rdb_pass)
                    {
                        string lblrdbName = rdo.Name;
                        if (lblrdbName == "rdbPass" + a)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                        if (lblrdbName == "rdbFail" + a)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                    }
                    SaveTestCaseResult(testCaseId, "Fail", FrameToSend, ResponseToReceive, "Manual", "");
                }
                lnkNewTest.Visible = true;
            }
        }        

        private void SetManualTestCases(DataTable dtmanual, int a)
        {
            var rdb_pass = pnlManualTest.Controls.OfType<RadioButton>();
            var lbl_Status = pnlManualTest.Controls.OfType<Label>().Where(u => u.Text != "TestCaseID").ToList();
            int testCaseId = Convert.ToInt32(dtmanual.Rows[a]["TestCaseID"]);
            OpenSerialPort();
            if (serialPort1.PortName == "USBNotConnected")               //|| !serialPort1.IsOpen
            {
                OpenSerialPort();
            }
            else
            {
                if (btnUsbStatus.BackColor == Color.Silver)
                    return;

                Messages = "";
                Cursor.Current = Cursors.WaitCursor;
                WriteDataToSerialPort(dtmanual, a);
                bool CheckResponceVBAT = CheckVBAT_ReadResponce(dtmanual, a);
                Cursor.Current = Cursors.Arrow;              

                if (Messages == "Frame sent successfully")
                {
                    rdbPassClicked = false;
                    rdbFailClicked = false;

                    foreach (Label lbstatus in lbl_Status)
                    {
                        string lblstatusName = lbstatus.Name;
                        if (lblstatusName == "lblStatus_" + a)
                        {
                            lbstatus.Enabled = true;
                            lbstatus.Text = CheckResponceVBAT ? "Pass" : string.Empty;
                            lbstatus.ForeColor = CheckResponceVBAT ? Color.Green : lbstatus.ForeColor;

                            //lbstatus.Enabled = true;
                            //lbstatus.Text = string.Empty;
                            //if (CheckResponceVBAT)
                            //{
                            //    lbstatus.Text = "Pass";
                            //    lbstatus.ForeColor = Color.Green;
                            //}
                        }
                        if (lblstatusName == "lbl_" + a)
                        {
                            lbstatus.Enabled = true;
                        }
                    }
                    foreach (RadioButton rdo in rdb_pass)
                    {
                        string lblstatusName = rdo.Name;
                        if (lblstatusName == "rdbPass" + a)
                        {
                            rdo.Enabled = true;
                            rdo.Checked = false;
                            if (CheckResponceVBAT)
                            {
                                rdo.Checked = true;
                                int testcaseid = Convert.ToInt32(dtmanual.Rows[a]["TestCaseID"]);
                                SaveTestCaseResult(testcaseid, "Pass", VBAT_RTC_Write_FrameToSend, ResponseToReceive, "Manual", "");
                            }                            
                        }
                        if (lblstatusName == "rdbFail" + a)
                        {
                            rdo.Enabled = true;
                            rdo.Checked = false;
                        }
                    }

                    int timeoutDuration = 30000; // 30 seconds     
                   
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    // Start a task to wait for the specified duration
                    Task.Delay(timeoutDuration, cancellationTokenSource.Token).ContinueWith((task) =>
                    {
                        if (task.IsCanceled)
                        {
                            return; // Exit the task if cancellation occurred
                        }
                        // Check if neither radio button was clicked within the                        

                        foreach (RadioButton rdo in rdb_pass)
                        {
                            if ((rdo.Name == "rdbPass" + a && !rdbPassClicked && rdo.Enabled == true) || (rdo.Name == "rdbFail" + a && !rdbFailClicked && rdo.Enabled == true))
                            {
                                // Invoke UI update operation on the main UI thread
                                Invoke((MethodInvoker)delegate
                                {
                                    // SetManualTestcasesToTimeout(a);
                                    foreach (Label lbstatus in lbl_Status)
                                    {
                                        string lblstatusName = lbstatus.Name;
                                        if (lblstatusName == "lblStatus_" + a)
                                        {
                                            lbstatus.Enabled = true;
                                            lbstatus.Text = "Timeout";
                                            lbstatus.ForeColor = Color.Red;
                                        }
                                        if (lblstatusName == "lbl_" + a)
                                        {
                                            lbstatus.Enabled = false;
                                        }
                                    }

                                    string lblrdbName = rdo.Name;
                                    if (lblrdbName == "rdbPass" + a || lblrdbName == "rdbFail" + a)
                                    {
                                        rdo.Enabled = false;
                                        rdo.Checked = false;
                                    }
                                });
                                SaveTestCaseResult(testCaseId, "Timeout", FrameToSend, ResponseToReceive, "Manual", "");
                                this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
                            }

                            //if (rdbPassName == currentRdbPass && rdbPassClicked || (rdbPassName == currentRdbFail && rdbFailClicked))
                            if ((rdo.Name == "rdbPass" + a && rdbPassClicked && rdo.Enabled == true) || (rdo.Name == "rdbFail" + a && rdbFailClicked && rdo.Enabled == true))
                            {
                                // Cancel the delay task if either flag becomes true
                                cancellationTokenSource.Cancel();
                            }
                        }
                    });
                }
                else if (Messages == "Timeout")
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        string lblstatusName = lbstatus.Name;
                        if (lblstatusName == "lblStatus_" + a)
                        {
                            lbstatus.Enabled = true;
                            lbstatus.Text = "Timeout";
                            lbstatus.ForeColor = Color.Red;
                        }
                        if (lblstatusName == "lbl_" + a)
                            lblStatus.Enabled = false;
                    }
                    foreach (RadioButton rdo in rdb_pass)
                    {
                        string lblrdbName = rdo.Name;
                        if (lblrdbName == "rdbPass" + a)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                        if (lblrdbName == "rdbFail" + a)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                    }
                    SaveTestCaseResult(testCaseId, "Timeout", FrameToSend, ResponseToReceive, "Manual", "");
                    this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
                }
                else    //----- for if exception occures,display test fail  
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        string lblstatusName = lbstatus.Name;
                        if (lblstatusName == "lblStatus_" + a)
                        {
                            lbstatus.Enabled = true;
                            lbstatus.Text = "Fail";
                            lbstatus.ForeColor = Color.Red;
                        }
                    }
                    foreach (RadioButton rdo in rdb_pass)
                    {
                        string lblrdbName = rdo.Name;
                        if (lblrdbName == "rdbPass" + a)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                        if (lblrdbName == "rdbFail" + a)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                    }
                    SaveTestCaseResult(testCaseId, "Fail", FrameToSend, ResponseToReceive, "Manual", "");
                    this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
                }
                lnkNewTest.Visible = true;
            }
        }

        private bool CheckVBAT_ReadResponce(DataTable dtmanual,int j)
        {
            try
            {
                if ((pcbtypeid == 9 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 46) ||
                    (pcbtypeid == 10 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 57) ||
                    (pcbtypeid == 11 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 69))
                {
                    if (ResponseToReceive.Length > 25)
                    {
                        string VBAT_RTC_Read = ResponseToReceive.Substring(10, 15);
                        string VBAT_RTC_ReadDate;

                        //if (Convert.ToInt16(VBAT_RTC_Read.Substring(8, 2)) < 12)
                            VBAT_RTC_ReadDate = VBAT_RTC_Read.Substring(0, 2) + "/" + VBAT_RTC_Read.Substring(2, 2) + "/" + VBAT_RTC_Read.Substring(4, 4) +
                            " " + VBAT_RTC_Read.Substring(8, 2) + ":" + VBAT_RTC_Read.Substring(10, 2) + ":" + VBAT_RTC_Read.Substring(12, 2);// + " AM";
                        //else
                        //    VBAT_RTC_ReadDate = VBAT_RTC_Read.Substring(0, 2) + "/" + VBAT_RTC_Read.Substring(2, 2) + "/" + VBAT_RTC_Read.Substring(4, 4) +
                        //    " " + VBAT_RTC_Read.Substring(8, 2) + ":" + VBAT_RTC_Read.Substring(10, 2) + ":" + VBAT_RTC_Read.Substring(12, 2) + " PM";

                        int Day = Convert.ToInt32(VBAT_RTC_Read.Substring(14, 1));
                        DateTime resuledate;
                        if (DateTime.TryParseExact(VBAT_RTC_ReadDate, "ddMMyyyyHHmmss",CultureInfo.CurrentCulture,DateTimeStyles.AdjustToUniversal, out resuledate))
                        {
                            return false;
                        }                        
                        //if (VBAT_RTC_Write_DateTime < DateTime.ParseExact(VBAT_RTC_ReadDate, "dd/MM/yyyy HH:mm:ss tt", null))
                        if (DateTime.ParseExact(VBAT_RTC_Write_DateTime, "dd/MM/yyyy HH:mm:ss", null) < DateTime.ParseExact(VBAT_RTC_ReadDate, "dd/MM/yyyy HH:mm:ss", null)) 
                        {
                            if ((WeekDay < 7 && WeekDay <= Day) || (WeekDay == 7 && WeekDay >= Day))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        // Start auto test
        private void BtnAutomaticStart_Click(object sender, EventArgs e)
        {
            try
            {                
                var lbl_Status = grpAutoTest.Controls.OfType<Label>();
                var autotestname = lbl_Status.Where(u => u.Name.Contains("lblAutoStatus_"));
                //lbl_Status = lbl_Status.Where(u => u.Text == "Pass" || u.Text == "" || u.Text == "Fail" || u.Name.Contains("lblBatVoltg_") || u.Name== "lblMotorReading").ToList();
                lbl_Status = lbl_Status.Where(u => u.Text == "Pass" || u.Text == "" || u.Text == "Fail" || u.Text == "Timeout" || u.Name.Contains("lblBatVoltg_") || u.Name == "lblMotorReading").ToList();
                if (lblPcbTypeId.Text != string.Empty)
                {
                    if (txtserialNo.Text.Length != 11)
                    {
                        SerialNoMsgBox.ShowBox();
                        txtserialNo.Focus();
                        txtserialNo.SelectionStart = txtserialNo.Text.Length;
                    }
                    else
                    {
                        DataTable dtAuto = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Auto");
                        if (dtAuto.Rows.Count > 0)
                        {
                            int a = dtAuto.Rows.Count;
                            OpenSerialPort();
                            if (serialPort1.PortName == "USBNotConnected")
                            {
                                OpenSerialPort();
                            }
                            else
                            {
                                // Stop data received from serial port                
                                this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                                foreach (Label lbstatus in lbl_Status)
                                {
                                    //if (lbstatus.Text == "Pass" || lbstatus.Text == "Fail" || lbstatus.Name.Contains("lblMotorReading") || lbstatus.Name.Contains("lblBatVoltg_"))
                                    if (lbstatus.Text != "")       
                                    {
                                        lbstatus.Text = string.Empty;
                                    }
                                }
                                Autoj = 0;
                                foreach (Label lbstatus in lbl_Status)
                                {
                                    Cursor.Current = Cursors.WaitCursor;
                                    OpenSerialPort();
                                    if (serialPort1.PortName == "USBNotConnected")
                                    {
                                        OpenSerialPort();
                                    }
                                    else
                                    {
                                        if (btnUsbStatus.BackColor == Color.Silver)
                                            return;

                                        string lblstatusName = lbstatus.Name;
                                        if (lblstatusName == "lblAutoStatus_" + Autoj)
                                        {
                                            int testCaseId = Convert.ToInt32(dtAuto.Rows[Autoj]["TestCaseID"]);
                                            Messages = "";
                                            //write data to serial port
                                            Cursor.Current = Cursors.WaitCursor;
                                            if (testCaseId != 34)
                                            {
                                                Cursor.Current = Cursors.WaitCursor;
                                                WriteDataToSerialPort(dtAuto, Autoj);
                                                Cursor.Current = Cursors.Arrow;
                                            }
                                            //BP 120124
                                            //else if(pcbtypeid == 7 && testCaseId==34) // For encoder auto test and RFID read/write test
                                            //{                                                
                                            //    string msgresult = _msgbox.ShowBox("Keep RFID card in front of encoder.");       // ("Set battery voltage between 4.60V to 4.80V and then click on OK", "lbllongmsg");   //dropdown battery voltage
                                            //    Cursor.Current = Cursors.WaitCursor;
                                            //    if (msgresult.Equals("Yes"))
                                            //    {
                                            //        WriteDataToSerialPort(dtAuto, Autoj);
                                            //    }
                                            //    //Cursor.Current = Cursors.Arrow;
                                            //}
                                            //Cursor.Current = Cursors.Arrow;

                                            if (Messages == "Frame sent successfully")
                                            {
                                                CheckAutoTests(Autoj, dtAuto, testCaseId);
                                            }
                                            else if (Messages == "Timeout")
                                            {
                                                if (lbstatus.Name == "lblAutoStatus_" + Autoj)
                                                {
                                                    var lasttestnm = autotestname.Last();
                                                    // Autoj++;
                                                    if (lbstatus.Name == lasttestnm.Name)
                                                    {
                                                        lbstatus.Enabled = true;
                                                        lbstatus.Text = "Timeout";
                                                        lbstatus.ForeColor = Color.Red;
                                                        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");   //Save output value
                                                    }
                                                    else
                                                    {
                                                        lbstatus.Enabled = true;
                                                        lbstatus.Text = "Timeout";
                                                        lbstatus.ForeColor = Color.Red;
                                                        // Insert data to database
                                                        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");   //Save output value
                                                    }
                                                    lnkNewTest.Visible = true;
                                                }
                                            }
                                            else
                                            {
                                                if (lbstatus.Name == "lblAutoStatus_" + Autoj)
                                                {
                                                    var lasttestnm = autotestname.Last();
                                                    if (lbstatus.Name == lasttestnm.Name)
                                                    {
                                                        AutoTestFail(lbstatus);
                                                        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");   //Save output value
                                                    }
                                                    else
                                                    {
                                                       AutoTestFail(lbstatus);
                                                       // Insert data to database
                                                       SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");   //Save output value
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
                                //Show_PCBStatus();       //Show current pcb status
                                Cursor.Current = Cursors.Arrow;                                
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select PCB type...");
                }
            }
            catch (Exception ex)
            {
                //if (serialPort1 != null && serialPort1.IsOpen)
                //{ serialPort1.Close(); }
                ShowException(ex.Message);
            }
        }
        
        //// Auto test 
        private void CheckAutoTests(int j, DataTable dtAuto, int testCaseId)
        {
            try
            {
                var lbl_Status = grpAutoTest.Controls.OfType<Label>();
                string outputval = string.Empty;
                var autotestname = lbl_Status.Where(u => u.Name.Contains("lblAutoStatus_"));
                // remove unecessary item from label list
                //lbl_Status = lbl_Status.Where(u => u.Text == "Pass" || u.Text == "" || u.Text == "Fail" || u.Name.Contains("lblBatVoltg_") || u.Name== "lblMotorReading").ToList();
                lbl_Status = lbl_Status.Where(u => u.Text == "Pass" || u.Text == "" || u.Text == "Fail" || u.Text == "Timeout" || u.Name.Contains("lblBatVoltg_") || u.Name == "lblMotorReading").ToList();
                int rowcount = dtAuto.Rows.Count;
                if (Messages == "Frame sent successfully")
                {
                    if (testCaseId == 5 || testCaseId == 6 || testCaseId == 18 || testCaseId == 19)
                    {
                        DCToDConvertorTest(dtAuto, testCaseId, j);
                    }
                    else if (pcbtypeid == 9)
                    {
                        if ((ResponseToReceive.Substring(12, 1) == "P" && testCaseId == 50) || (ResponseToReceive.Substring(13, 1) == "P" && testCaseId == 51))
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Pass";
                                    lbstatus.ForeColor = Color.Green;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", string.Empty);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Fail";
                                    lbstatus.ForeColor = Color.Red;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    // Insert data to database                                    
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", string.Empty);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    else if (pcbtypeid == 10)
                    {
                        if ((ResponseToReceive.Substring(16, 1) == "P" && testCaseId == 61) || (ResponseToReceive.Substring(13, 1) == "P" && testCaseId == 62) )
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Pass";
                                    lbstatus.ForeColor = Color.Green;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", string.Empty);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Fail";
                                    lbstatus.ForeColor = Color.Red;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    // Insert data to database                                    
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", string.Empty);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    else if (pcbtypeid == 11)
                    {
                        if ((ResponseToReceive.Substring(16, 1) == "P" && testCaseId == 73) || (ResponseToReceive.Substring(13, 1) == "P" && testCaseId == 74))
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Pass";
                                    lbstatus.ForeColor = Color.Green;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", string.Empty);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Fail";
                                    lbstatus.ForeColor = Color.Red;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    // Insert data to database                                    
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", string.Empty);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    else if(pcbtypeid==7)   // Encoder Auto tests
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        if (ResponseToReceive == dtAuto.Rows[j]["PassFrame"].ToString())
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Pass";
                                    lbstatus.ForeColor = Color.Green;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    // Insert data to database                                    
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", string.Empty);

                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Fail";
                                    lbstatus.ForeColor = Color.Red;
                                    lnkNewTest.Visible = true;
                                    Autoj++;                                                               
                                    // Insert data to database                                    
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", string.Empty);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                            //foreach (Label lbstatus in lbl_Status)
                            //{
                            //    if (lbstatus.Name == "lblAutoStatus_" + j)
                            //    {
                            //        var lasttestnm = autotestname.Last();

                            //        if (lbstatus.Name == lasttestnm.Name)
                            //        {
                            //            AutoTestFail(lbstatus);
                            //            SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", outputval); //Save output value
                            //        }
                            //    }
                            //}
                        }
                        Cursor.Current = Cursors.Arrow;
                    }
                    else
                    {
                        if (ResponseToReceive == dtAuto.Rows[j]["PassFrame"].ToString())
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Pass";
                                    lbstatus.ForeColor = Color.Green;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    // Insert data to database                                    
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto",string.Empty);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }           

                        //In HHD controller PCB- Comm to Lock Test -Error Code(No response from lock, Mismatch data)
                        else if (ResponseToReceive == "*D0004100077F1#" || ResponseToReceive == "*D00041008F6F9#")
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    lbstatus.Enabled = true;
                                    lbstatus.Text = "Fail";
                                    lbstatus.ForeColor = Color.Red;
                                    lnkNewTest.Visible = true;
                                    Autoj++;
                                    string errormsg = string.Empty;
                                    if (ResponseToReceive == "*D0004100077F1#")
                                    { errormsg = "No response from lock"; }
                                    else { errormsg = "Mismatch data"; }
                                    // Insert data to database                                    
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", errormsg);
                                    if (j != rowcount - 1)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                }
                            }
                        }

                        #region comment
                        //else if(pcbtypeid==7 && ResponseToReceive != dtAuto.Rows[j]["PassFrame"].ToString() )
                        //{
                        //       foreach (Label lbstatus in lbl_Status)
                        //        {
                        //            if (lbstatus.Name == "lblAutoStatus_" + j)
                        //            {
                        //            var lasttestnm = autotestname.Last();
                        //            if (lbstatus.Name == lasttestnm.Name)
                        //            {
                        //                AutoTestFail(lbstatus);
                        //                SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");//Save output value
                        //            }
                        //            else
                        //            {
                        //                AutoTestFail(lbstatus);
                        //                // Insert data to database
                        //                SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");//Save output value
                        //            }

                        //            ////var lasttestnm = autotestname.Last();

                        //            ////if (lbstatus.Name == lasttestnm.Name)
                        //            ////{
                        //            //    AutoTestFail(lbstatus);
                        //            //    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", outputval); //Save output value
                        //            ////}
                        //        }
                        //        }                            
                        //}
                        #endregion comment
                        else
                        {
                            foreach (Label lbstatus in lbl_Status)
                            {
                                if (lbstatus.Name == "lblAutoStatus_" + j)
                                {
                                    var lasttestnm = autotestname.Last();

                                    if (lbstatus.Name == lasttestnm.Name)
                                    {
                                        AutoTestFail(lbstatus);
                                        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", outputval); //Save output value
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        if (lbstatus.Name == "lblAutoStatus_" + j)
                        {                           
                            var lasttestnm = autotestname.Last();
                            if (lbstatus.Name == lasttestnm.Name)
                            {
                                AutoTestFail(lbstatus);
                                SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto","");//Save output value
                            }
                            else
                            {                                 
                                AutoTestFail(lbstatus);
                                // Insert data to database
                                SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");//Save output value
                             }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }

        private void RFIDTests()
        {
            
        }

        private void MotordriveCurrentTest(DataTable dtAuto, int testCaseId, int j)
        {
            try
            {
                string dbResponse = dtAuto.Rows[j]["PassFrame"].ToString();
                string removecrccinresponse = string.Empty; string chkresponse = string.Empty; string getAntiClockwisevalue = string.Empty;
                string getClockwisevalue = string.Empty;
                int index1 = dbResponse.Trim().IndexOf("xxxxxCRCC#");       //Anticlockwise value
                int index2 = dbResponse.Trim().IndexOf("xxxxxxxxxxCRCC#");  //Clockwise value
                string a = ResponseToReceive;
                getAntiClockwisevalue = ResponseToReceive.Substring(index1, 5);
                getClockwisevalue = ResponseToReceive.Substring(index2,5);                

                //Get Controlller - Motor drive Current Test min max value
                DataTable dtAnticlockwise = _objDal.CompareADCMinMaxValue(getAntiClockwisevalue, Convert.ToInt32(lblPcbTypeId.Text), testCaseId);
                DataTable dtclockwise = _objDal.CompareADCMinMaxValue(getClockwisevalue, Convert.ToInt32(lblPcbTypeId.Text), testCaseId);
                var lbl_Status = grpAutoTest.Controls.OfType<Label>();
                lbl_Status = lbl_Status.Where(u => (u.Name.Contains("lblAutoStatus_")));

                //----------Show current voltage-------------//

                var lblAuto = grpAutoTest.Controls.OfType<Label>();
                Label lblMotorRotaion = new Label();
                lblAuto = lblAuto.Where(u => (u.Name.Contains("lblMotorReading")));
                string motorcurrentvalue = string.Empty;
                foreach (Label lbl in lblAuto)
                {                   
                    if (lbl.Name == "lblMotorReading")
                    {                        
                       lbl.Text = "Clockwise:" + getClockwisevalue + "A, Anticlockwise:" + getAntiClockwisevalue + "A";
                      
                        lbl.Enabled = true;
                        motorcurrentvalue = lbl.Text;
                    }
                }
                //-------------------------------------------//

                if (dtAnticlockwise.Rows.Count > 0 && dtclockwise.Rows.Count > 0)
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        if (lbstatus.Name == "lblAutoStatus_" + j)
                        {
                            lbstatus.Enabled = true;  //Test is Pass
                            lbstatus.Text = "Pass";
                            lbstatus.ForeColor = Color.Green;
                            lnkNewTest.Visible = true;
                            Autoj++;
                            SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", motorcurrentvalue);   //Save output value
                            //6/12/18
                            int rowcount = dtAuto.Rows.Count;
                            if (j != rowcount-1)
                            {
                                PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                            }
                        }
                    }
                }
                else
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        if (lbstatus.Name == "lblAutoStatus_" + j)
                        {
                            lbstatus.Enabled = true; //Test is fail
                            lbstatus.Text = "Fail";
                            lbstatus.ForeColor = Color.Red;
                            lnkNewTest.Visible = true;
                            SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", motorcurrentvalue);   //Save output value
                            //PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                            int rowcount = dtAuto.Rows.Count;
                            if (j != rowcount - 1)
                            {
                                PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }

        private void AutoTestFail(Label lbstatus)
        {
            lbstatus.Enabled = true;
            lbstatus.Text = "Fail";
            lbstatus.ForeColor = Color.Red;
            lnkNewTest.Visible = true;
        }

        private void DCToDConvertorTest(DataTable dtAuto, int testCaseId, int j)
        {
            try
            {
                string dbResponse = dtAuto.Rows[j]["PassFrame"].ToString();
                int rowcount = dtAuto.Rows.Count;
                string removecrccinresponse = string.Empty; string chkresponse = string.Empty; string getcurrentvalue = string.Empty;
                int index1 = dbResponse.Trim().IndexOf("xxxxCRCC#");

                if (index1 != -1)
                {
                    removecrccinresponse = dbResponse.Remove(index1);
                }
                chkresponse = ResponseToReceive.Remove(index1);

                if (chkresponse == removecrccinresponse)
                {
                    getcurrentvalue = ResponseToReceive.Substring(index1, 4);
                }
                //Get Controlller-DC to DC Converter test min max value
                DataTable dt = _objDal.CompareADCMinMaxValue(getcurrentvalue, Convert.ToInt32(lblPcbTypeId.Text), testCaseId);
                var lbl_Status = grpAutoTest.Controls.OfType<Label>();               
                lbl_Status = lbl_Status.Where(u => (u.Name.Contains("lblAutoStatus_")));

                //--------------Display Current Value----------------
                var lblBatVoltg = grpAutoTest.Controls.OfType<Label>();
                lblBatVoltg = lblBatVoltg.Where(u => (u.Name.Contains("lblBatVoltg_")));
                string BatVoltgValue = string.Empty;                     //used for Save battery current
                foreach (Label lblBatVoltage in lblBatVoltg)
                {
                    string lbl = lblBatVoltage.Name;
                    if (lbl == "lblBatVoltg_" + (j))
                    {
                        lblBatVoltage.Enabled = true;
                        lblBatVoltage.Text = getcurrentvalue +"V";
                        BatVoltgValue = lblBatVoltage.Text;
                    }
                }
                //---------------------------------------------------//
                if (dt.Rows.Count > 0)
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        if (lbstatus.Name == "lblAutoStatus_" + j)
                        {
                            lbstatus.Enabled = true;  //Test is Pass
                            lbstatus.Text = "Pass";
                            lbstatus.ForeColor = Color.Green;
                            lnkNewTest.Visible = true;
                            Autoj++;
                            SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", BatVoltgValue);  //Save output value

                            if (j == 0 && (pcbtypeid==1 || pcbtypeid==4))          // if (j != rowcount - 1)
                            {
                                string msgresult = _msgbox.ShowMSg();       // ("Set battery voltage between 4.60V to 4.80V and then click on OK", "lbllongmsg");   //dropdown battery voltage
                                if (msgresult.Equals("Yes"))
                                {
                                    PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());                                   
                                }
                                else
                                {
                                    AutoTestFail(lbstatus);
                                    // Insert data to database
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");//Save output value
                                }
                            }
                            if (j != rowcount - 1 && j!=0)
                            {
                                PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                            }
                        }
                    }
                }
                else
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        if (lbstatus.Name == "lblAutoStatus_" + j)
                        {
                            lbstatus.Enabled = true; //Test is fail
                            lbstatus.Text = "Fail";
                            lbstatus.ForeColor = Color.Red;
                            lnkNewTest.Visible = true;
                            SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto",BatVoltgValue);  //Save output value
                             
                            if (j == 0)          //if (j != rowcount - 1)
                            {
                                string msgresult = _mydialogBox.ShowBox();       // For do u want to continue
                                if (msgresult.Equals("Yes"))
                                {
                                    string msgresult1 = _msgbox.ShowMSg();      // ("Set battery voltage between 4.60V to 4.80V and then click on OK");   //dropdown battery voltage
                                    if (msgresult1.Equals("Yes"))
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());                                       
                                    }
                                    else
                                    {
                                        AutoTestFail(lbstatus);
                                        // Insert data to database
                                        SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");//Save output value
                                    }
                                    if (j != rowcount - 1 && j != 0)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                    Autoj++;
                                }
                                else
                                {
                                    AutoTestFail(lbstatus);
                                    // Insert data to database
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");   //Save output value
                                }                                
                            }
                            else 
                            {
                                string msgresult = _mydialogBox.ShowBox();       // For do u want to continue
                                if (msgresult.Equals("Yes"))
                                {                                    
                                    if (j != rowcount - 1 && j != 0)
                                    {
                                        PleaseWaitMsg.ShowBox(dtAuto.Rows[j + 1]["TestCaseName"].ToString());
                                    }
                                    Autoj++;
                                }
                                else
                                {
                                    AutoTestFail(lbstatus);
                                    // Insert data to database
                                    SaveTestCaseResult(testCaseId, lbstatus.Text, FrameToSend, ResponseToReceive, "Auto", "");   //Save output value
                                }
                            }                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }

        // Save test case result
        private void SaveTestCaseResult(int TestCaseID, string status, string FrameToSend, string ResponseToReceive, string TestType,string TestOutputValue)
        {
            try
            {
                _objBE.PropUserID = LoginUserId;
                _objBE.PropSerialNo = txtserialNo.Text;
                _objBE.PropPCBTypeID = Convert.ToInt32(lblPcbTypeId.Text);
                _objBE.PropPCBType = lblPcbType.Text;
                _objBE.PropTestCaseID = TestCaseID;
                _objBE.PropTestType = TestType;
                _objBE.PropStatus = status;
                _objBE.PropFrameToSend = FrameToSend;
                _objBE.PropResponseFrame = ResponseToReceive;
                _objBE.PropCreatedBy = LoginUserId;
                _objBE.PropComment = TestOutputValue;
                _objBE.ProductTypeID = GlobalInformation.ProductTypeID;
                string CurrentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt");
                _objBE.PropCurrentDateTime = CurrentDateTime;
                Messages = _objDal.InsertTestCaseResult(_objBE);
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }       
        private void OpenSerialPort()
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    string myPort = STMUSBCommunication.AlphaPort();
                    if (myPort != "")
                    {
                        if (!serialPort1.IsOpen)
                        {
                            serialPort1.PortName = myPort;
                            serialPort1.BaudRate = 115200;
                            serialPort1.ReadTimeout = 5000;
                            serialPort1.WriteBufferSize = 1024;
                            serialPort1.ReadBufferSize = 1024;
                            serialPort1.Parity = Parity.None;
                            serialPort1.DataBits = 8;
                            serialPort1.StopBits = StopBits.One;
                            serialPort1.Open();
                            btnUsbStatus.BackColor = Color.Green;                            
                        }                        
                    }
                    else
                    {
                        // serialPort1.PortName = "USBNotConnected";
                        btnUsbStatus.BackColor = Color.Silver;
                        _msgbox.ShowBox("Please connect USB cable...");
                       // return;
                    }
                }
            }
            catch (Exception ex)
            {
                if (serialPort1.IsOpen)
                { serialPort1.Close(); }
                ShowException(ex.Message);                
            }
        }
      
        private void ReadEthernetData(NetworkStream stream, Byte[] data1, Byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                do
                {
                  stream.Read(data1, 0, data.Length);
                  memoryStream.Write(data1, 0, data.Length);
                } while (stream.DataAvailable);
                ResponseToReceive = Encoding.ASCII.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            }
        }

        private string VBAT_RTC_Write_DateTime;        
        private int WeekDay;
        private string VBAT_RTC_Write_FrameToSend;
        private void WriteDataToSerialPort(DataTable dtmanual, int j)
        {
            try
            {
                isTestRunning = true;
                //-- Serial Port
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();

                //-- Frame from database    
                string dtFrameToSend = dtmanual.Rows[j]["SendFrame"].ToString();
                dtFrameToSend = CalculateDateFrameForVBATTest(dtmanual, j, dtFrameToSend);
                string TestType= dtmanual.Rows[j]["TestType"].ToString();
                int testCaseId = Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]);               
                                
                serialPort1.Write(dtFrameToSend);
                //BP 10112023
                Cursor.Current = Cursors.WaitCursor;
                SerialPortReadData(testCaseId, TestType);
                Cursor.Current = Cursors.Arrow;

                isTestRunning = false;
                //serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                //SM 20112023
                //int index = 0;
                //do
                //{
                //    //WriteDataToSerialPort(writeFrame);
                //    ResponseToReceive = STMUSBCommunication.SerialPortReadData();
                //    index++;
                //}
                //while ((string.IsNullOrEmpty(ResponseToReceive) || !ResponseToReceive.Contains("*") || !ResponseToReceive.Contains("#")) && index <= 3);
            }               
            catch (Exception ex)
            {
                ShowException(ex.Message);
            }
        }

        private string CalculateDateFrameForVBATTest(DataTable dtmanual, int j,string dtFrameToSend)
        {
            if (pcbtypeid == 9 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 45 ||
                    pcbtypeid == 10 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 56 ||
                      pcbtypeid == 11 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 68)
            {
                VBAT_RTC_Write_DateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");                
                WeekDay = Convert.ToInt32(DateTime.Now.DayOfWeek);
                string currentDateTime = DateTime.Now.ToString("ddMMyyyyHHmmss") + Convert.ToInt32(DateTime.Now.DayOfWeek);
                string StrInitialData = dtFrameToSend.Substring(0, 10) + currentDateTime;
                string CRC = CalculateCRCC(StrInitialData);
                VBAT_RTC_Write_FrameToSend = StrInitialData + CRC + "#";
                return VBAT_RTC_Write_FrameToSend;
            }
            else if (pcbtypeid == 9 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 46 ||
                    pcbtypeid == 10 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 57 ||
                      pcbtypeid == 11 && Convert.ToInt32(dtmanual.Rows[j]["TestCaseID"]) == 69)
            {
                _msgbox.ShowMsgVBATReadTest();
                return dtFrameToSend;
            }
            else
                return dtFrameToSend;
        }

        public string CalculateCRCC(string strcardDetails)
        {
            string asciinput = strcardDetails;  // FrameSubString;

            // Convert string to byte 
            Encoding ascii = Encoding.ASCII;
            Byte[] asciibytes = ascii.GetBytes(asciinput);

            string asci = Crc16CcittXModem(asciibytes).ToString("x2", CultureInfo.InvariantCulture);
            string CCRC_Code = asci.ToUpper(CultureInfo.CurrentCulture);

            //Append 0 to CRCC Code lenght is < 4.
            int Length1 = CCRC_Code.Length;
            int j11 = 0;
            if (Length1 <= 4)
            {
                for (int i = 0; i < (4 - Length1); i++)
                {
                    CCRC_Code = '0' + CCRC_Code;
                    j11++;
                }
            }
            return CCRC_Code;
        }
        // Read data from serial port
        //private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    try
        //    {
        //        pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
        //        //Thread.Sleep(2000);
        //        //if (TestCaseID == Convert.ToInt32("14"))   //For USB Test
        //        //{
        //        //    Thread.Sleep(1000);
        //        //}
        //        //--Read data from serial port                
        //        int dataLength = serialPort1.BytesToRead;
        //        byte[] data = new byte[dataLength];
        //        int nbrDataRead = serialPort1.Read(data, 0, dataLength);
        //        //Thread.Sleep(1000);
        //        ResponseToReceive = System.Text.Encoding.Default.GetString(data); 
               
        //            string errorstr = ResponseToReceive.Substring(10, 4);
        //            DataTable dtErrorCode = _objDal.GetErrorDetails(errorstr);
        //            if (dtErrorCode.Rows.Count > 0)
        //            {
        //                if (errorstr == dtErrorCode.Rows[0]["ErrorCode"].ToString())
        //                {
        //                    ErrorMsg = dtErrorCode.Rows[0]["ErrorMessage"].ToString();

        //                    // Code for to stop continues frame from PCB 
        //                    if (pcbtypeid == 1)
        //                    { serialPort1.Write(" *12A000004STOP6709#"); }
        //                    if (pcbtypeid == 2) { serialPort1.Write("*12B000004STOP82E7#"); }
        //                    _msgbox.ShowBox(ErrorMsg);
        //                }
        //            }
        //            else
        //            {
        //                Messages = "Frame sent successfully";
        //            }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
        //    }
        //}

        private string GetResponseFromSerialPort()
        {
            int dataLength = serialPort1.BytesToRead;
            byte[] data = new byte[dataLength];
            int nbrDataRead = serialPort1.Read(data, 0, dataLength);
            return ResponseToReceive = System.Text.Encoding.Default.GetString(data);
        }

        private void SerialPortReadData(int TestCaseID, string TestType)
        {
            try
            {
                // this.serialPort1.DataReceived+= new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
                pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                Thread.Sleep(2000);
                if (TestCaseID == 4 || TestCaseID == 5 || TestCaseID == 11 || TestCaseID==12)   //For Relay Test 3sec
                {
                    Thread.Sleep(1000);
                }
                //--Read data from serial port                
                int dataLength = serialPort1.BytesToRead;
                byte[] data = new byte[dataLength];
                int nbrDataRead = serialPort1.Read(data, 0, dataLength);
                Thread.Sleep(100);
                ResponseToReceive = System.Text.Encoding.Default.GetString(data);

                DateTime Tthen = DateTime.Now;
                //if (String.IsNullOrEmpty(ResponseToReceive) || ResponseToReceive.Count() < 3)
                //{
                //    int index = 0;
                //    do
                //    {
                //        ResponseToReceive= GetResponseFromSerialPort();
                //        Thread.Sleep(135);
                //    }
                //    while ((string.IsNullOrEmpty(ResponseToReceive) || !ResponseToReceive.Contains("*") || !ResponseToReceive.Contains("#")) && index <= 3);
                //    //(string.IsNullOrEmpty(ResponseToReceive) || !ResponseToReceive.Contains("*") || !ResponseToReceive.Contains("#")) && index <= 3
                //}


                if (String.IsNullOrEmpty(ResponseToReceive) || ResponseToReceive.Length < 3)
                {
                    DateTime startTime = DateTime.Now;
                    TimeSpan timeout = TimeSpan.FromSeconds(30);
                    int index = 0;
                    do
                    {
                        ResponseToReceive = GetResponseFromSerialPort();
                        Thread.Sleep(135);
                        index++;
                    }
                    while ((string.IsNullOrEmpty(ResponseToReceive) || !ResponseToReceive.Contains("*") || !ResponseToReceive.Contains("#")) && DateTime.Now - startTime < timeout);
                    // while ((string.IsNullOrEmpty(ResponseToReceive) || !ResponseToReceive.Contains("*") || !ResponseToReceive.Contains("#")) && index <= 3 && DateTime.Now - startTime < timeout);
                }

                //new logic for checking error or success

                if (ResponseToReceive == string.Empty)
                {
                    //Get error code details                   
                    DataTable dtmsg = _objDal.GetErrorMsg(TestCaseID);
                    if (dtmsg.Rows.Count > 0)
                    {
                        if (TestCaseID == Convert.ToInt32(dtmsg.Rows[0]["TestCaseId"]))
                        {
                            //Thread.Sleep(5000);
                            //GetResponseFromSerialPort(TestCaseID);
                            if (ResponseToReceive == string.Empty)
                            {
                                ErrorMsg = dtmsg.Rows[0]["ErrorMessage"].ToString();
                                _msgbox.ShowBox(ErrorMsg);
                            }
                        }
                    }
                    else
                    {
                        Messages = "Timeout";
                        //ErrorMsg = "USB communication error...";
                        //// (For hybrid test timed out after 30 sec)
                        ////TimeoutMsg = "Time out";
                        //_msgbox.ShowBox(ErrorMsg);
                    }
                    // Code for to stop continues frame from PCB 
                    if (pcbtypeid == 1)
                    { serialPort1.Write("*12A000004STOP6709#"); }
                    if (pcbtypeid == 2) { serialPort1.Write("*12B000004STOP82E7#"); }
                }
                else
                {
                    string errorstr = ResponseToReceive.Substring(10, 4);
                    DataTable dtErrorCode = _objDal.GetErrorDetails(errorstr, TestCaseID);
                    if (dtErrorCode.Rows.Count > 0)
                    {
                        if (errorstr == dtErrorCode.Rows[0]["ErrorCode"].ToString())
                        {
                            ErrorMsg = dtErrorCode.Rows[0]["ErrorMessage"].ToString();
                            _msgbox.ShowBox(ErrorMsg);
                        }
                        // Code for to stop continues frame from PCB 
                        if (pcbtypeid == 1) { serialPort1.Write(" *12A000004STOP6709#"); }
                        if (pcbtypeid == 2) { serialPort1.Write("*12B000004STOP82E7#"); }
                    }
                    else
                    {
                        Messages = "Frame sent successfully";
                    }
                }


                //if (ResponseToReceive == string.Empty)
                //{
                //    Thread.Sleep(1000);
                //    GetResponseFromSerialPort(TestCaseID);
                //}
                //if (ResponseToReceive == string.Empty)
                //{
                //    Thread.Sleep(1000);
                //    GetResponseFromSerialPort(TestCaseID);
                //}
                //if (ResponseToReceive == string.Empty)
                //{
                //    Thread.Sleep(1000);
                //    GetResponseFromSerialPort(TestCaseID);
                //}
                //if (ResponseToReceive == string.Empty)
                //{
                //    Thread.Sleep(1000);
                //    GetResponseFromSerialPort(TestCaseID);
                //}
                //if (ResponseToReceive == string.Empty)
                //{
                //    Thread.Sleep(5000);
                //    GetResponseFromSerialPort(TestCaseID);
                //    if (ResponseToReceive == string.Empty)
                //    {
                //        Thread.Sleep(5000);
                //        GetResponseFromSerialPort(TestCaseID);

                //        if (ResponseToReceive == string.Empty)
                //        {
                //            Thread.Sleep(10000);
                //            GetResponseFromSerialPort(TestCaseID);
                //            if (ResponseToReceive == string.Empty)
                //            {
                //                //Get error code details                   
                //                DataTable dtmsg = _objDal.GetErrorMsg(TestCaseID);
                //                if (dtmsg.Rows.Count > 0)
                //                {
                //                    if (TestCaseID == Convert.ToInt32(dtmsg.Rows[0]["TestCaseId"]))
                //                    {
                //                        Thread.Sleep(5000);
                //                        GetResponseFromSerialPort(TestCaseID);
                //                        if (ResponseToReceive == string.Empty)
                //                        {
                //                            ErrorMsg = dtmsg.Rows[0]["ErrorMessage"].ToString();
                //                            _msgbox.ShowBox(ErrorMsg);
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    ErrorMsg = "USB communication error...";
                //                    // (For hybrid test timed out after 30 sec)
                //                    //TimeoutMsg = "Time out";
                //                    _msgbox.ShowBox(ErrorMsg);
                //                }
                //                // Code for to stop continues frame from PCB 
                //                if (pcbtypeid == 1)
                //                { serialPort1.Write("*12A000004STOP6709#"); }
                //                if (pcbtypeid == 2) { serialPort1.Write("*12B000004STOP82E7#"); }
                //            }
                //            else
                //            {
                //                string errorstr = ResponseToReceive.Substring(10, 4);
                //                DataTable dtErrorCode = _objDal.GetErrorDetails(errorstr, TestCaseID);
                //                if (dtErrorCode.Rows.Count > 0)
                //                {
                //                    if (errorstr == dtErrorCode.Rows[0]["ErrorCode"].ToString())
                //                    {
                //                        ErrorMsg = dtErrorCode.Rows[0]["ErrorMessage"].ToString();
                //                        _msgbox.ShowBox(ErrorMsg);
                //                    }
                //                    // Code for to stop continues frame from PCB 
                //                    if (pcbtypeid == 1) { serialPort1.Write(" *12A000004STOP6709#"); }
                //                    if (pcbtypeid == 2) { serialPort1.Write("*12B000004STOP82E7#"); }
                //                }
                //                else
                //                {
                //                    Messages = "Frame sent successfully";
                //                }
                //            }
                //        }
                //        else
                //        {
                //            string errorstr = ResponseToReceive.Substring(10, 4);
                //            DataTable dtErrorCode = _objDal.GetErrorDetails(errorstr, TestCaseID);
                //            if (dtErrorCode.Rows.Count > 0)
                //            {
                //                if (errorstr == dtErrorCode.Rows[0]["ErrorCode"].ToString())
                //                {
                //                    ErrorMsg = dtErrorCode.Rows[0]["ErrorMessage"].ToString();
                //                    _msgbox.ShowBox(ErrorMsg);
                //                }
                //                // Code for to stop continues frame from PCB 
                //                if (pcbtypeid == 1) { serialPort1.Write(" *12A000004STOP6709#"); }
                //                if (pcbtypeid == 2) { serialPort1.Write("*12B000004STOP82E7#"); }
                //            }
                //            else
                //            {
                //                Messages = "Frame sent successfully";
                //            }
                //        }
                //    }
                //    else
                //    {
                //        string errorstr = ResponseToReceive.Substring(10, 4);
                //        DataTable dtErrorCode = _objDal.GetErrorDetails(errorstr, TestCaseID);
                //        if (dtErrorCode.Rows.Count > 0)
                //        {
                //            if (errorstr == dtErrorCode.Rows[0]["ErrorCode"].ToString())
                //            {
                //                ErrorMsg = dtErrorCode.Rows[0]["ErrorMessage"].ToString();
                //                _msgbox.ShowBox(ErrorMsg);
                //            }
                //            // Code for to stop continues frame from PCB 
                //            if (pcbtypeid == 1) { serialPort1.Write(" *12A000004STOP6709#"); }
                //            if (pcbtypeid == 2) { serialPort1.Write("*12B000004STOP82E7#"); }
                //        }
                //        else
                //        {
                //            Messages = "Frame sent successfully";
                //        }
                //    }
                //}
                //else
                //{
                //    string errorstr = ResponseToReceive.Substring(10, 4);
                //    DataTable dtErrorCode = _objDal.GetErrorDetails(errorstr, TestCaseID);
                //    if (dtErrorCode.Rows.Count > 0)
                //    {
                //        if (errorstr == dtErrorCode.Rows[0]["ErrorCode"].ToString())
                //        {
                //            ErrorMsg = dtErrorCode.Rows[0]["ErrorMessage"].ToString();

                //            // Code for to stop continues frame from PCB 
                //            if (pcbtypeid == 1)
                //            { serialPort1.Write(" *12A000004STOP6709#"); }
                //            if (pcbtypeid == 2) { serialPort1.Write("*12B000004STOP82E7#"); }
                //            _msgbox.ShowBox(ErrorMsg);
                //        }
                //    }
                //    else
                //    {
                //        Messages = "Frame sent successfully";
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }

        private string CheckForOtherError(int TestCaseID)
        {
            try
            {
                if(pcbtypeid == 9 && (TestCaseID == 48 || TestCaseID == 49 || TestCaseID == 50 | TestCaseID == 51))
                {
                    if (ResponseToReceive.Substring(15, 1) == "F")
                        return "Card absent";
                    else if (ResponseToReceive.Substring(15, 1) == "P")
                        return "Card present";
                    else if (ResponseToReceive.Substring(15, 1) == "E")
                        return "Initialization error";
                }
            }
            catch 
            {
                throw;
            }
            return "";
        }

        //Read data from serial port
        private void GetResponseFromSerialPort(int TestCaseID)
        {                     
            int dataLength = serialPort1.BytesToRead;
            byte[] data = new byte[dataLength];
            int nbrDataRead = serialPort1.Read(data, 0, dataLength);
            ResponseToReceive = System.Text.Encoding.Default.GetString(data);
        }

        static byte[] HexToBytes(string input)
        {
            byte[] result = new byte[input.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(input.Substring(2 * i, 2), 16);
            }
            return result;
        }

        // CRCC calculation
        private ushort Crc16CcittXModem(byte[] bytes)
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0x0000;
            ushort temp, a;
            ushort crc = initialValue;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort)((temp << 1) ^ poly);
                    else
                        temp <<= 1;
                    a <<= 1;
                }
                table[i] = temp;
            }
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }
        private void RdbFail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // New optimized code 19042024
                DataTable dtmanual = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Manual");
                int m = dtmanual.Rows.Count;
                pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                var rdb_pass = pnlManualTest.Controls.OfType<RadioButton>();
                RadioButton rdbFailChecked = sender as RadioButton;
                int j = 0;

                for (int i = 0; i < Math.Min(7, m); i++)
                {
                    if (rdbFailChecked.Checked && rdbFailChecked.Name == $"rdbFail{j + i}" && rdbFailChecked.Enabled)
                    {
                        if (pcbtypeid == 8 && i == 1) // RFID and rdbFail 1
                        {
                            SetManualRFIDTest(dtmanual, j, i, i + 1, rdb_pass, "Fail");
                        }
                        else
                        {
                            SetManulRdbFail(dtmanual, j, i, i + 1, rdb_pass);
                        }
                    }
                }

                this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));


                //DataTable dtmanual = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Manual");
                //int m = dtmanual.Rows.Count;
                //pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                //var rdb_pass = pnlManualTest.Controls.OfType<RadioButton>();
                //RadioButton rdbFailChecked = sender as RadioButton;
                //int j = 0;                

                //// rdb Fail 0
                //if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbFail" + j && rdbFailChecked.Enabled == true)
                //{
                //        SetManulRdbFail(dtmanual, j, 0, 1, rdb_pass);                  
                //}
                ////rdb Fail 1
                //else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbFail" + (j + 1) && rdbFailChecked.Enabled == true)
                //{             
                //    if(pcbtypeid==8)        // RFID
                //     SetManualRFIDTest(dtmanual, j, 1, 2, rdb_pass, "Fail");
                //    else
                //     SetManulRdbFail(dtmanual, j, 1, 2, rdb_pass);                  
                //}
                //// rdb Fail 2
                //else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbFail" + (j + 2) && rdbFailChecked.Enabled == true)
                //{                   
                //        SetManulRdbFail(dtmanual, j, 2, 3, rdb_pass);                  
                //}
                //// rdb Fail 3
                //else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbFail" + (j + 3) && rdbFailChecked.Enabled == true)
                //{                    
                //        SetManulRdbFail(dtmanual, j, 3, 4, rdb_pass);                   
                //}
                //// rdb Fail 4
                //else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbFail" + (j + 4) && rdbFailChecked.Enabled == true)
                //{
                //    SetManulRdbFail(dtmanual, j, 4, 5, rdb_pass);
                //}
                ////rdb Fail 5

                //else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbFail" + (j + 5) && rdbFailChecked.Enabled == true)
                //{
                //    SetManulRdbFail(dtmanual, j, 5, 6, rdb_pass);
                //}
                ////rdb Fail 6

                //else if (rdbFailChecked.Checked == true && rdbFailChecked.Name == "rdbFail" + (j + 6) && rdbFailChecked.Enabled == true)
                //{
                //    SetManulRdbFail(dtmanual, j, 6, 7, rdb_pass);
                //}              

                //this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));

            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        //, RadioButton rbpass
        private void SetManulRdbFail(DataTable dtmanual, int j, int a, int b, IEnumerable<RadioButton> rdb_pass)
        {
            try
            {               
                int testcaseid = Convert.ToInt32(dtmanual.Rows[a]["TestCaseID"]);
                lnkNewTest.Visible = true;
                var lbl_Status = pnlManualTest.Controls.OfType<Label>();
                // Filter status list
                lbl_Status = lbl_Status.Where(u => u.Text != "TestCaseID").ToList();
                foreach (Label lbstatus in lbl_Status)
                {
                    string lblstatusName = lbstatus.Name;
                    if (lblstatusName == "lblStatus_" + (j + a))
                    {
                        lbstatus.Enabled = true;
                        lbstatus.Text = "Fail";
                        lbstatus.Enabled = true;
                        lbstatus.ForeColor = Color.Red;
                        foreach (RadioButton rdo in rdb_pass)
                        {
                            string lblrdbName = rdo.Name;
                            if (lblrdbName == "rdbPass" + (j + a))
                            {
                                rdo.Enabled = false;
                            }
                            if (lblrdbName == "rdbPass" + (j + b))
                            {
                                rdo.Enabled = false;
                            }
                            if (lblrdbName == "rdbFail" + (j + a))
                            {
                                rdo.Enabled = false;
                            }
                            if (lblrdbName == "rdbFail" + (j + b))
                            {
                                rdo.Enabled = false;
                            }
                            rdo.Checked = false;
                        }
                        SaveTestCaseResult(testcaseid, lbstatus.Text, FrameToSend, ResponseToReceive, "Manual","");
                    }
                    if (lblstatusName == "lblStatus_" + (j + a))
                    {
                        lbstatus.Enabled = true;
                    }
                    if (lblstatusName == "lbl_" + (j + a))
                    {
                        lbstatus.Enabled = false;
                    }
                    if (lblstatusName == "lbl_" + (j + b))
                    {
                        lbstatus.Enabled = false;
                    }                   
                }
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }
        private void RdbPass_CheckedChanged(object sender, EventArgs e)
        {

            // New optimized code by BBP 19042024
            try
            {
                lnkNewTest.Visible = true;
                DataTable dtmanual = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Manual");
                int m = dtmanual.Rows.Count;
                pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                var rdb_pass = pnlManualTest.Controls.OfType<RadioButton>();
                RadioButton rdbPassChecked = sender as RadioButton;
                int j = 0;
                int maxIndex = Math.Min(5, m); // Maximum index to iterate over

                // Loop through possible indices
                for (int i = 0; i < maxIndex; i++)
                {
                    // Check if the current rdbPass button is checked and matches the name pattern
                    if (rdbPassChecked.Checked && rdbPassChecked.Name == $"rdbPass{j + i}" && rdbPassChecked.Enabled)
                    {
                        SetManualRdbPass(dtmanual, j, i, i + 1, rdb_pass);
                        SetManualTestCases(dtmanual, i + 1); // If previous test is pass, then auto start next test.
                    }
                }

                this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }

            // Old code till 18-04-2024
            //try
            //{
            //    lnkNewTest.Visible = true;
            //    DataTable dtmanual = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Manual");
            //    int m = dtmanual.Rows.Count;
            //    pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
            //    var rdb_pass = pnlManualTest.Controls.OfType<RadioButton>();
            //    RadioButton rdbPassChecked = sender as RadioButton;
            //    int j = 0;                
            //    //rdb pass 0
            //    if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbPass" + j && rdbPassChecked.Enabled == true)
            //    {
            //        SetManualRdbPass(dtmanual, j, 0, 1, rdb_pass);
            //        //if(pcbtypeid!=5)
            //        SetManualTestCases(dtmanual, 1);   //If previous test is pass, then auto start next test.
            //        // SetManualRdbPass(dtmanual, j+1, 1, 2, rdb_pass);
            //    }
            //    //rdb pass 1

            //    else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbPass" + (j + 1) && rdbPassChecked.Enabled == true)
            //    {                    
            //            SetManualRdbPass(dtmanual, j, 1, 2, rdb_pass);
            //            SetManualTestCases(dtmanual, 2);   //If previous test is pass, then auto start next test.                    
            //    }
            //    //rdb pass 2

            //    else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbPass" + (j + 2) && rdbPassChecked.Enabled == true)
            //    {                   
            //        SetManualRdbPass(dtmanual, j, 2, 3, rdb_pass);                 
            //        SetManualTestCases(dtmanual, 3);   //If previous test is pass, then auto start next test.
            //        //SetManualRdbPass(dtmanual, j + 1, 3, 4, rdb_pass);
            //    }
            //    //rdb pass 3
            //    else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbPass" + (j + 3) && rdbPassChecked.Enabled == true)
            //    {                    
            //        SetManualRdbPass(dtmanual, j, 3, 4, rdb_pass);

            //        //if(pcbtypeid == 1)
            //        //    SetManualTestCases(dtmanual, 4);   //If previous test is pass, then auto start next test.

            //    }

            //   // rdb pass 4
            //    else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbPass" + (j + 4) && rdbPassChecked.Enabled == true)
            //    {
            //        SetManualRdbPass(dtmanual, j, 4, 5, rdb_pass);
            //        //if(pcbtypeid == 9 || pcbtypeid == 10 || pcbtypeid == 11 || pcbtypeid == 12)
            //        //    SetManualTestCases(dtmanual, 5);   //If previous test is pass, then auto start next test.
            //                                           // SetManualRdbPass(dtmanual, j + 1, 5, 6, rdb_pass);
            //    }

            //    //////rdb pass 5
            //    //else if (rdbPassChecked.Checked == true && rdbPassChecked.Name == "rdbPass" + (j + 5) && rdbPassChecked.Enabled == true)
            //    //{
            //    //    SetManualRdbPass(dtmanual, j, 5, 6, rdb_pass);
            //    //    SetManualTestCases(dtmanual, 6);   //If previous test is pass, then auto start next test.
            //    //    //SetManualRdbPass(dtmanual, j + 1, 6, 7, rdb_pass);
            //    //}            


            //    this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
            //    //Code For PCB Status (21-11-2019)
            //    //Show_PCBStatus();
            //}
            //catch (Exception ex)
            //{
            //    log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            //    MessageBox.Show(ex.Message);
            //}
        }


        private void SetManualRFIDTest(DataTable dtmanual, int j, int a, int b, IEnumerable<RadioButton> rdb_pass, string testresult)
        {
           
            try
            {
                var lbl_Status = pnlManualTest.Controls.OfType<Label>();
                lbl_Status = lbl_Status.Where(u => u.Text != "TestCaseID").ToList();    //Filter status list
                int testcaseid = Convert.ToInt32(dtmanual.Rows[a]["TestCaseID"]);

                if (txtserialNo.Text.Trim() == string.Empty || txtserialNo.Text.Length != 11)        // 7 May 19
                {
                    _msgbox.ShowBox("Please enter correct serial number");
                    txtserialNo.Focus();
                    foreach (RadioButton rdo in rdb_pass)
                    {
                        rdo.Checked = false;
                    }

                    foreach (Label lbstatus in lbl_Status)                 // 8 May 2019
                    {
                        string lblstatusName1 = lbstatus.Name;
                        if (lblstatusName1 == "lblStatus_" + (j + a))
                        {
                            lbstatus.Enabled = false;
                            lbstatus.Enabled = true;
                            lbstatus.Text = "-";
                        }
                    }
                }
                else
                {
                    foreach (Label lbstatus in lbl_Status)
                    {
                        string lblstatusName1 = lbstatus.Name;
                        if (lblstatusName1 == "lblStatus_" + (j + a))
                        {
                            lbstatus.Enabled = true;
                            lbstatus.Text = testresult;
                            if (testresult == "Pass")
                            { lbstatus.ForeColor = Color.Green; }
                            else { lbstatus.ForeColor = Color.Red; }
                            //---Save test case result
                            SaveTestCaseResult(testcaseid, lbstatus.Text, FrameToSend, ResponseToReceive, "Manual", "");
                        }
                    }
                    foreach (RadioButton rdo in rdb_pass)
                    {
                        rdo.Enabled = false;
                        rdo.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        private void SetManualRdbPass(DataTable dtmanual, int j, int a, int b, IEnumerable<RadioButton> rdb_pass)
        {
            try
            {
                var lbl_Status = pnlManualTest.Controls.OfType<Label>();
                lbl_Status = lbl_Status.Where(u => u.Text != "TestCaseID").ToList();    // Filter status list
                int testcaseid = Convert.ToInt32(dtmanual.Rows[a]["TestCaseID"]);
                foreach (Label lbstatus in lbl_Status)
                {
                    string lblstatusName1 = lbstatus.Name;
                    if (lblstatusName1 == "lblStatus_" + (j + a))
                    {
                        lbstatus.Enabled = true;
                        lbstatus.Text = "Pass";
                        lbstatus.ForeColor = Color.Green;                     
                        //---Save test case result
                        SaveTestCaseResult(testcaseid, lbstatus.Text, FrameToSend, ResponseToReceive, "Manual","");
                    }
                    if (lblstatusName1 == "lbl_" + (j + a))
                    {
                        lbstatus.Enabled = false;
                    }
                }
                foreach (RadioButton rdo in rdb_pass)
                {
                    string lblstatusName = rdo.Name;
                    if (lblstatusName == "rdbPass" + (j + a))
                    {
                        rdo.Checked = false;
                    }
                    if (lblstatusName == "rdbFail" + (j + a))
                    {
                        rdo.Checked = false;
                    }
                    rdo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        //Display PCB status(Pass/Fail) 21-01-2019
        private void Show_PCBStatus()
        {
            DataTable dtPcbStatus = _objDal.GetPCBTestStatus(Convert.ToInt32(lblPcbTypeId.Text), txtserialNo.Text);
            if (dtPcbStatus.Rows.Count > 0)
            {
                if (dtPcbStatus.Rows[0]["SerialNumber"].ToString() != string.Empty)
                {
                    //if ((lblPcbType.Text == "Latch" && txtserialNo.Text.Length == 11) || (lblPcbType.Text != "Latch" && txtserialNo.Text.Length == 11))
                    //{
                        string status = dtPcbStatus.Rows[0]["Status"].ToString();
                        if (status == "Pass")
                        { lblPcbStatus.ForeColor = Color.Green; }
                        else if (status == "Fail" || status == "Timeout")
                        { lblPcbStatus.ForeColor = Color.Red; }

                        lblPcbStatus.Text = dtPcbStatus.Rows[0]["Status"].ToString();
                    //}
                }
            }
            else { lblPcbStatus.Text = "-"; }
        }

        private void LnkNewTest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                txtserialNo.Text = string.Empty;
                ResponseToReceive = string.Empty;
                FrameToSend = string.Empty;

                //Clear Manual Test Previous Status
                var btn = pnlManualTest.Controls.OfType<Button>();
                var lbls = pnlManualTest.Controls.OfType<Label>();
                foreach (Button btns in btn)
                {
                    if (btns.Name == "btnStartManual" + 0)
                    {
                        string btnname = btns.Name;

                        if (btnname == "btnStartManual" + 0)
                        {
                            btns.Enabled = true;
                        }
                    }
                }
                var rdb_pass = pnlManualTest.Controls.OfType<RadioButton>();
                var lbl_Status = pnlManualTest.Controls.OfType<Label>();            

                int j = 0;
                foreach (Button btns in btn)
                {
                    string btnname = btns.Name;
                    foreach (Label lbstatus in lbl_Status)
                    {
                        string lblstatusName = lbstatus.Name;
                        if (lblstatusName == "lblStatus_" + j)
                        {
                            lbstatus.Enabled = false;
                            lbstatus.Text = string.Empty;
                            lbstatus.ForeColor = Color.Black;
                        }
                        if (lblstatusName == "lbl_" + j)
                        {
                            lbstatus.Enabled = false;
                        }
                    }
                    foreach (RadioButton rdo in rdb_pass)
                    {
                        string lblstatusName = rdo.Name;
                        if (lblstatusName == "rdbPass" + j)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                        if (lblstatusName == "rdbFail" + j)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                    }
                    j++;
                }
                //Clear Auto Test Previous Status
                var lblresultStatus = grpAutoTest.Controls.OfType<Label>();
                int l = 0;
                foreach (Label lbl in lblresultStatus)
                {
                    string lblstatusName = lbl.Name;
                    if (lblstatusName != "lblAutoTestCase_" + l) 
                    {
                        lbl.Text = string.Empty;
                        lbl.Enabled = false;
                        lbl.ForeColor = Color.Black;                                            
                    }
                    else
                    {
                        lbl.Enabled = false;
                        l++;
                    }
                }
                l = 0;

                // Clear Hybrid Test Previous Status
                var Hybridbtn = pnlHybridTest.Controls.OfType<Button>();
                var Hybridlbls = pnlHybridTest.Controls.OfType<Label>();
                foreach (Button btns in Hybridbtn)
                {
                    if (btns.Name == "btnStartHybrid" + 0)
                    {
                        string btnname = btns.Name;
                        if (btnname == "btnStartHybrid" + 0)
                        {
                            btns.Enabled = true;
                        }
                    }
                }
                var Hybridrdb_pass = pnlHybridTest.Controls.OfType<RadioButton>();
                var Hybridlbl_Status = pnlHybridTest.Controls.OfType<Label>();
                int j1 = 0;
                foreach (Button btns in Hybridbtn)
                {
                    string btnname = btns.Name;
                    foreach (Label lbstatus in Hybridlbl_Status)
                    {
                        string lblstatusName = lbstatus.Name;
                        if (lblstatusName == "labelsHStatus" + j1)
                        {
                            lbstatus.Enabled = false;
                            lbstatus.Text = string.Empty;
                            lbstatus.ForeColor = Color.Black;
                        }
                        if (lblstatusName == "labelsHybrid" + j1)
                        {
                            lbstatus.Enabled = false;
                            //lbstatus.Text = string.Empty;
                        }
                    }
                    foreach (RadioButton rdo in Hybridrdb_pass)
                    {
                        string lblstatusName = rdo.Name;
                        if (lblstatusName == "rdbHPass" + j1)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                        if (lblstatusName == "rdbHFail" + j1)
                        {
                            rdo.Enabled = false;
                            rdo.Checked = false;
                        }
                    }
                    j1++;
                }              
                txtserialNo.Focus();
                txtserialNo.SelectionStart = txtserialNo.Text.Length;
                //Clear keyboard test panel
               // KeyboardTestControls();
               // ChangeKeyboardButtonsColor();
                lblPcbStatus.Text = "-";
                lblPcbStatus.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }     
      
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(50);
                int dataLength = serialPort1.BytesToRead;
                byte[] data = new byte[dataLength];
                int nbrDataRead = serialPort1.Read(data, 0, dataLength);
                string TestResponse = "";
                TestResponse = System.Text.Encoding.Default.GetString(data); 
                if (TestResponse.Contains("KEYPAD"))
                {
                    ReadKeyboardKeys(TestResponse);
                }
                else
                {
                    DisplayHybridTestResult(TestResponse);
                }
            }
            catch 
            {                
            }
        }
        private void DisplayHybridTestResult(string Response)
        {
            var btnHybridResult = pnlHybridTest.Controls.OfType<Button>();
            var rdb_pass = pnlHybridTest.Controls.OfType<RadioButton>();
            var lbl_Status = pnlHybridTest.Controls.OfType<Label>();
            pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
            DataTable dtHybrid = _objDal.GetPCBTestCases(Convert.ToInt32(lblPcbTypeId.Text), "Hybrid");
            int TotalButtonNumber = btnHybridResult.Count();
            foreach (Button btn in btnHybridResult)
            {
                string btnName = btn.Name;
              
                if (btn.Name.Contains("btnHResult"))
                {  
                    if (Response == string.Empty)
                    {
                    }
                    if (Convert.ToInt32(TotalButtonNumber/2) >= 1)
                    {
                        if (Response == dtHybrid.Rows[0]["PassFrame"].ToString())
                        {
                            if (btnName == "btnHResult0")
                            {
                                btn.BackColor = Color.Green;
                                //Thread.Sleep(1000);
                                //btn.BackColor = Color.Silver;
                                //if(pcbtypeid==1)
                                //SetHybridTestCases(dtHybrid, 1);
                            }
                        }
                        else
                        {
                            if (btnName == "btnHResult0")
                            {
                                btn.BackColor = Color.Silver;
                            }
                        }
                    }
                    if (Convert.ToInt32(TotalButtonNumber / 2) >= 2)
                    {
                        if (Response == dtHybrid.Rows[1]["PassFrame"].ToString())
                        {
                            if (btnName == "btnHResult1")
                            {
                                btn.BackColor = Color.Green;
                                //Thread.Sleep(1000);
                                //btn.BackColor = Color.Silver;
                                //SetHybridTestCases(dtHybrid, 2);
                            }
                        }
                        else
                        {
                            if (btnName == "btnHResult1")
                            {
                                btn.BackColor = Color.Silver;
                            }
                        }
                    }
                    #region comment
                    //else
                    //{
                    //    if (btnName == "btnHResult1")
                    //    {
                    //        btn.BackColor = Color.Silver;
                    //        foreach (Label lbstatus in lbl_Status)
                    //        {
                    //            string lblstatusName = lbstatus.Name;
                    //            if (lblstatusName == "labelsHStatus" + 1)
                    //            {
                    //                lbstatus.Enabled = true;
                    //                lbstatus.Text = string.Empty;
                    //                //check timer counter for wait 30 sec
                    //                USBCommunication.TimerStartTest(lbstatus);
                    //            }
                    //            if (lblstatusName == "labelsHybrid" + 1)
                    //            {
                    //                lbstatus.Enabled = true;
                    //                USBCommunication.lblTestName = lbstatus;
                    //            }
                    //        }
                    //        foreach (RadioButton rdo in rdb_pass)
                    //        {
                    //            string lblstatusName = rdo.Name;
                    //            if (lblstatusName == "rdbHPass" + 1)
                    //            {
                    //                rdo.Enabled = true;
                    //                rdo.Checked = false;
                    //                USBCommunication.rdPass = rdo;
                    //            }
                    //            if (lblstatusName == "rdbHFail" + 1)
                    //            {
                    //                rdo.Enabled = true;
                    //                rdo.Checked = false;
                    //                USBCommunication.rdFail = rdo;
                    //            }
                    //        }
                    //    }
                    //}                   
                    #endregion comment
                    if (Convert.ToInt32(TotalButtonNumber / 2) >= 3)
                    {
                        if (Response == dtHybrid.Rows[2]["PassFrame"].ToString())
                        {
                            if (btnName == "btnHResult2")
                            {
                                btn.BackColor = Color.Green;
                                //Thread.Sleep(1000);
                                //btn.BackColor = Color.Silver;
                                //SetHybridTestCases(dtHybrid, 3);
                            }
                        }
                        else
                        {
                            if (btnName == "btnHResult2")
                            {
                                btn.BackColor = Color.Silver;
                            }
                        }
                    }
                    if (Convert.ToInt32(TotalButtonNumber / 2) >= 4)
                    {
                        if (Response == dtHybrid.Rows[3]["PassFrame"].ToString())
                        {
                            if (btnName == "btnHResult3")
                            {
                                btn.BackColor = Color.Green;
                                //Thread.Sleep(1000);
                                //btn.BackColor = Color.Silver;                           
                            }
                        }
                        else
                        {
                            if (btnName == "btnHResult3")
                            {
                                btn.BackColor = Color.Silver;
                            }
                        }
                    }
                   
                    if (Convert.ToInt32(TotalButtonNumber / 2) >= 5)
                    {
                        if (Response == dtHybrid.Rows[4]["PassFrame"].ToString())
                        {
                            if (btnName == "btnHResult4")
                            {
                                btn.BackColor = Color.Green;
                                //Thread.Sleep(1000);
                                //btn.BackColor = Color.Silver;                           
                            }
                        }
                        else
                        {
                            if (btnName == "btnHResult4")
                            {
                                btn.BackColor = Color.Silver;
                            }
                        }
                    }
                }
            }
        }

        //Check keypad button response in HHD Controller PCB.
        private void ReadKeyboardKeys(string KeypadResponse)
        {
            try
            {
                if (KeypadResponse == string.Empty)
                { }
                if (KeypadResponse == "*21D460008KEYPAD011C9C#" || KeypadResponse == "*21D520008KEYPAD016250#")
                {
                    btn1.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD022CFF#" || KeypadResponse == "*21D520008KEYPAD025233#")
                {
                    btn2.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD033CDE#" || KeypadResponse == "*21D520008KEYPAD034212#")
                {
                    btn3.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD044C39#" || KeypadResponse == "*21D520008KEYPAD0432F5#")
                {
                    btn4.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD055C18#" || KeypadResponse == "*21D520008KEYPAD0522D4#")
                {
                    btn5.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD066C7B#" || KeypadResponse == "*21D520008KEYPAD0612B7#")
                {
                    btn6.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD077C5A#" || KeypadResponse == "*21D520008KEYPAD070296#")
                {
                    btn7.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD088DB5#" || KeypadResponse == "*21D520008KEYPAD08F379#")
                {
                    btn8.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD099D94#" || KeypadResponse == "*21D520008KEYPAD09E358#")
                {
                    btn9.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD103F8C#" || KeypadResponse == "*21D520008KEYPAD104140#")
                {
                    btnStar.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD112FAD#" || KeypadResponse == "*21D520008KEYPAD115161#")
                {
                    btn0.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD121FCE#" || KeypadResponse == "*21D520008KEYPAD126102#")
                {
                    btnHash.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD130FEF#" || KeypadResponse == "*21D520008KEYPAD137123#")
                {
                    btnUP.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD147F08#" || KeypadResponse == "*21D520008KEYPAD1401C4#")
                {
                    btnDown.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD156F29#" || KeypadResponse == "*21D520008KEYPAD1511E5#")
                {
                    btnRefresh.BackColor = Color.Green;
                }
                if (KeypadResponse == "*21D460008KEYPAD165F4A#" || KeypadResponse == "*21D520008KEYPAD162186#")
                {
                    btnEnter.BackColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }
        private void PikboxMinimize_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void PikboxClose_Click_1(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            this.Close();
            Login login = new Login();
            login.Show();
        }
      
        private void Label4_Click(object sender, EventArgs e)  //Report page will be open
        {
           // Cursor.Current = Cursors.WaitCursor;
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }            
            Report rpt = new Report(LoginUserId, Username);
            rpt.Show();
            this.Hide();
           // Cursor.Current = Cursors.Arrow;
        }

        private void LblSignOut_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            this.Close();
            Login login = new Login();
            login.Show();
        }

        // Used for serial number validation
        private void TxtserialNo_KeyPress(object sender, KeyPressEventArgs e)
       {
            // Check if Ctrl key along with C, X, or V is pressed
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (e.KeyChar == 'c' || e.KeyChar == 'v' || e.KeyChar == 'x'))
            {
                // Suppress the key press event
                e.Handled = true;
            }


            txtserialNo.MaxLength = 11;
                if (e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                    SerialNoMsgBox.ShowBox();     // MessageBox.Show("Special character are not allowed...");              
                }
                else
                {
                    if (char.IsDigit(e.KeyChar))
                    if ((sender as TextBox).Text.Count() <= 2)
                    {
                        if (char.IsLetter(e.KeyChar))   //allow backspace(10-4-19)&& e.KeyChar != (char)Keys.Back
                    {
                            e.Handled = false;
                            return;
                        }
                        if (char.IsDigit(e.KeyChar))
                        {
                            e.Handled = true;
                            SerialNoMsgBox.ShowBox();   // MessageBox.Show("Please enter character only"); 
                        }
                    }
             
                if ((sender as TextBox).Text.Count() >= 3 )
                   {
                        if (char.IsDigit(e.KeyChar))
                        {
                            e.Handled = false;
                            return;
                        }
                        if (char.IsLetter(e.KeyChar))
                        {
                            e.Handled = true;
                            SerialNoMsgBox.ShowBox();    // MessageBox.Show("Please enter number only");
                        }
                    }
                }               
        }          
        
        private void ShowException(string excep)
        {
            //Close serial port
            if (serialPort1.IsOpen)
            { serialPort1.Close(); }

            serialPort1.Close();           
            //
            if (excep.Contains("does not exist") || excep.Contains("timed out"))
            {
                _msgbox.ShowBox("Please reconnect USB cable...");
            }
            else if (excep.Contains("semaphore timeout"))
            {
                _msgbox.ShowBox("Please Reset " + lblPcbType.Text);
            }
            else if (excep.Contains("There is no row at position"))
            { }
            else if (excep.Contains("I/O operation has been aborted") || excep.Contains("device attached to the system is not functioning"))
            {
                _msgbox.ShowBox("Please check the " + lblPcbType.Text + " connections", "longmsg");
            }
            else if(excep.Contains("closed"))       //The port is closed.
            {           //OpenSerialPort();
            }
            else if (excep.Contains("denied") || (excep.Contains("Object reference")))
            {
               //serialPort1.Close();
            }
            else
            {               
                MessageBox.Show(excep);
            }          
            //OpenSerialPort();       //changes 04-02-19
            log.CreateLog(excep, excep.ToString(), this.FindForm().Name);
        }        

    
        // For validation
        //private void txtCurrent_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Enter)
        //    {
        //        e.Handled = true;
        //        _msgbox.ShowBox("Please enter numbers only...");     // Special character are not allowed;              
        //    }
        //    if (char.IsDigit(e.KeyChar))
        //    {
        //        e.Handled = false;            // Characters not allowed
        //        return;
        //    }
        //    if (char.IsLetter(e.KeyChar))
        //    {
        //        e.Handled = true;
        //        _msgbox.ShowBox("Please enter numbers only...");   // Enter only numbers
        //    }
        //}

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        private void SysTimerTick()
        {
            string myPort = STMUSBCommunication.AlphaPort();
            if (myPort != "")
            {
                btnUsbStatus.BackColor = Color.Green;
            }
            else
            {
                btnUsbStatus.BackColor = Color.Silver;
            }
        }

        private void LblUser_Click(object sender, EventArgs e)
        {
            AddUser frmAddUser = new AddUser(LoginUserId, Username);
            frmAddUser.Show();
            this.Hide();
        }

        private void TxtserialNo_TextChanged(object sender, EventArgs e)
        {          
            //if (!txtserialNo.Text.StartsWith("LK"))    
            //{
            //    pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
            //    if (pcbtypeid == 1) { txtserialNo.Text = "LKR"; }       //Base Board
            //    else if (pcbtypeid == 2) { txtserialNo.Text = "LKA"; }  //Antenna
            //    else if (pcbtypeid == 3) { txtserialNo.Text = "LKP"; }  //USB
            //    else if (pcbtypeid == 4) { txtserialNo.Text = "LKH"; }  //HHD Controller
            //    else if (pcbtypeid == 5) { txtserialNo.Text = "LKD"; }  //HHD Dispaly       
            //    else if (pcbtypeid == 6) { txtserialNo.Text = "LKK"; }  //KeyPad
            //    else if (pcbtypeid == 7) { txtserialNo.Text = "LKE"; }  //Encoder
            //    else if (pcbtypeid == 8) { txtserialNo.Text = "LKC"; }  //RFID
            //    txtserialNo.Focus();
            //    txtserialNo.SelectionStart = txtserialNo.Text.Length;
            //}
        }

        private void BtnKeyboardStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblPcbTypeId.Text != string.Empty)
                {
                    if (txtserialNo.Text.Length != 11)
                    {
                        SerialNoMsgBox.ShowBox();
                        txtserialNo.Focus();
                        txtserialNo.SelectionStart = txtserialNo.Text.Length;
                    }
                    else
                    { 
                    if (btn1.BackColor == Color.Green || btn2.BackColor == Color.Green || btn3.BackColor == Color.Green || btn4.BackColor == Color.Green || btn5.BackColor == Color.Green || btn6.BackColor == Color.Green || btn7.BackColor == Color.Green || btn8.BackColor == Color.Green || btn9.BackColor == Color.Green || btnStar.BackColor == Color.Green || btn0.BackColor == Color.Green || btnHash.BackColor == Color.Green)
                    {
                        ChangeKeyboardButtonsColor();
                    }
                    // txtComment.Focus();
                    KeyboardTestControls();
                    DataTable dtKeyboardStart = _objDal.GetKeyBoardStart(Convert.ToInt32(lblPcbTypeId.Text));
                    OpenSerialPort();
                    if (serialPort1.PortName == "USBNotConnected")
                    {
                        OpenSerialPort();
                    }
                    else
                    {
                        this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                        Cursor.Current = Cursors.WaitCursor;
                        WriteDataToSerialPort(dtKeyboardStart, 0);
                        Cursor.Current = Cursors.Arrow;
                        if (Messages == "Frame sent successfully")
                        {
                            _msgbox.ShowBox("Please press the keypad button...");                          
                            rdbKeyboardPass.Enabled = true;
                            rdbKeyboardFail.Enabled = true;
                            lblKeyboardStatus.Enabled = true;
                            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                        }
                        else
                        {
                            lnkNewTest.Visible = true;
                        }
                    }
                }
                    }               
                else
                {
                    _msgbox.ShowBox("Please select PCB type...");
                }
            }
            catch (Exception ex)
            {             
                ShowException(ex.Message);
            }
        }

        private void KeyboardTestControls()
        {
            rdbKeyboardPass.Checked = false;
            rdbKeyboardFail.Checked = false;
            rdbKeyboardPass.Enabled = false;
            rdbKeyboardFail.Enabled = false;
            lblKeyboardStatus.Text = string.Empty;
            lblKeyboardStatus.Enabled = false;
        }

        private void RdbKeyboardPass_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbKeyboardPass.Checked == true)
                {                  
                        // -= removes the handler from the event's list of "listeners"         
                        this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                        DataTable dtKeyboardStart = _objDal.GetKeyBoardStart(Convert.ToInt32(lblPcbTypeId.Text));
                        int pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                        OpenSerialPort();
                      
                        if (Messages == "Frame sent successfully")
                        {
                            int TestCaseId = Convert.ToInt32(dtKeyboardStart.Rows[0]["TestCaseID"]);
                            InsertKeyboardTest(TestCaseId, "Pass", FrameToSend, ResponseToReceive);
                            KeyboardTestStatus("Pass");
                            lblKeyboardStatus.ForeColor = Color.Green;

                        //Stop keypad test
                        serialPort1.Write("*12D000004STOP7313#");
                    }
                    }
                this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
                //Show_PCBStatus();
                }            
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        private void RdbKeyboardFail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbKeyboardFail.Checked == true)
                {                    
                        // -= removes the handler from the event's list of "listeners": 
                        this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                        DataTable dtKeyboard = _objDal.GetKeyBoardStart(Convert.ToInt32(lblPcbTypeId.Text));
                        DataTable dtKeyboardStart = _objDal.GetKeyBoardStart(Convert.ToInt32(lblPcbTypeId.Text));
                        int pcbtypeid = Convert.ToInt32(lblPcbTypeId.Text);
                        OpenSerialPort();                     
                        if (Messages == "Frame sent successfully")
                        {
                            int TestCaseId = Convert.ToInt32(dtKeyboard.Rows[0]["TestCaseID"]);
                            InsertKeyboardTest(TestCaseId, "Fail", FrameToSend, ResponseToReceive);
                            KeyboardTestStatus("Fail");
                            lblKeyboardStatus.ForeColor = Color.Red;

                           //Stop keypad test
                             serialPort1.Write("*12D000004STOP7313#");                           
                        }
                    }
                this.BeginInvoke(new SimpleDelegateLabelStatus(Show_PCBStatus));
                //Show_PCBStatus();
                  }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        // Save keyboard testcase result
        private void InsertKeyboardTest(int TestCaseID, string status, string FrameToSend, string ResponseToReceive)
        {
            try
            {
                _objBE.PropUserID = LoginUserId;
                _objBE.PropSerialNo = txtserialNo.Text;
                _objBE.PropPCBTypeID = Convert.ToInt32(lblPcbTypeId.Text);
                _objBE.PropPCBType = lblPcbType.Text;
                _objBE.PropTestCaseID = TestCaseID;
                _objBE.PropStatus = status;
                _objBE.PropFrameToSend = FrameToSend;
                _objBE.PropResponseFrame = ResponseToReceive;
                _objBE.PropCreatedBy = LoginUserId;              
                string CurrentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt");
                _objBE.PropCurrentDateTime = CurrentDateTime;
                Messages = _objDal.SaveKeyboardTest(_objBE);
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }

        private void KeyboardTestStatus(string TestStatus)
        {
            lblKeyboardStatus.Visible = true;
            lblKeyboardStatus.Text = TestStatus;
            lnkNewTest.Visible = true;
            rdbKeyboardPass.Enabled = false;
            rdbKeyboardFail.Enabled = false;           
        }
        
        // This is used for background working process.
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // select connected devices            
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            //insertWatcher.Options.Timeout = new TimeSpan(1,0,0);
            insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
            removeWatcher.Start();
        }
        public void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            ManagementObjectCollection collection; // Create a object of ManagementObjectCollection class which is hold the list of connected devices
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub")) // This line fetch all the connected device with windows query
            {
                collection = searcher.Get();
            }
            foreach (var device in collection)
            {
                PIdVIdDeviceID = (string)device.GetPropertyValue("PNPDeviceID");
                USBDescription = (string)device.GetPropertyValue("Description");
                //if (PIdVIdDeviceID == "USB\\VID_1FC9&PID_0083\\NXP-77")
                if (USBDescription == "USB Composite Device")
                {
                    btnUsbStatus.BackColor = Color.Green;
                    return;
                }
                else
                {
                    btnUsbStatus.BackColor = Color.Silver;
                }
            }
            collection.Dispose();
        }
        public void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            ManagementObjectCollection collection; // Create a object of ManagementObjectCollection class which is hold the list of connected devices
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub")) // This line fetch all the connected device with windows query
            {
                collection = searcher.Get();
            }
            foreach (var device in collection)
            {
                PIdVIdDeviceID = (string)device.GetPropertyValue("PNPDeviceID");
                USBDescription = (string)device.GetPropertyValue("Description");

                // if (PIdVIdDeviceID == "USB\\VID_1FC9&PID_0083\\NXP-77")

                if (USBDescription == "USB Composite Device")
                {
                    btnUsbStatus.BackColor = Color.Green;
                    return;
                }
                else
                {
                    btnUsbStatus.BackColor = Color.Silver;
                }
            }
            collection.Dispose();
        }

        private void lblPcbStatus_Click(object sender, EventArgs e)
        {

        }

        private void txtserialNo_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Ctrl key along with C, X, or V is pressed
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.V || e.KeyCode == Keys.X))
            {
                // Suppress the key down event
                e.SuppressKeyPress = true;
            }
            if ((sender as TextBox).Text.Count() == 3)  //allow backspace(10-4-19)&& e.KeyChar != (char)Keys.Back
            {
                string thirdChar = txtserialNo.Text.Substring(2, 1);
                if (pcbtypeid == 1)
                {
                    if (thirdChar != "C")
                        txtserialNo.Text = txtserialNo.Text.Substring(0, 2) + "C" + txtserialNo.Text.Substring(3, txtserialNo.Text.Count() - 3);
                }
                else if (pcbtypeid == 2)
                {
                    if (thirdChar != "R")
                        txtserialNo.Text = txtserialNo.Text.Substring(0, 2) + "R" + txtserialNo.Text.Substring(3, txtserialNo.Text.Count() - 3);
                }
            }
            }       
        private void lblProdcutSelect_Click(object sender, EventArgs e)
        {
            frmProductType frmPro = new frmProductType(this);
            frmPro.Owner = this;
            frmPro.ShowDialog();
            BindData();
            //if(frmProductType.frmProductStatus)
            //BindGridView();
            //if (grdPcbType.Rows.Count > 0)
            //{
            //    lblPleaseSelectProduct.Visible = false;
            //    BindDefaultPCBTestCases();
            //}
            //else
            //    lblPleaseSelectProduct.Visible = true;
        }
    }
}

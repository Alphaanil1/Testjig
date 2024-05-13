using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Windows.Forms;


namespace ESD_Testjig
{
    public partial class Report : Form
    {
        readonly int LoginUserId;
        readonly ESD_TestjigProperties _objDal = new ESD_TestjigProperties();
        readonly MyMessageBox _msgbox = new MyMessageBox();
        readonly LogFile log1 = new LogFile();
        int scr_val;
        System.Data.DataTable dt;
        readonly DataSet NewDs = new DataSet();
        readonly string Username = string.Empty;
        static int i1 = 0;
        string PIdVIdDeviceID = string.Empty;
        DataTable dtUserRole;
        string LoginUserRole = string.Empty;
        int LoginUserRoleID;

        public Report(int UserId, string UserName)
        {
            InitializeComponent();
            LoginUserId = UserId;
            Username = UserName;
            scr_val = 0;
            btnShowReport.Cursor = Cursors.Hand;
        }
        private void Report_Load(object sender, EventArgs e)
        {
            try
            {
                //Get user role
                dtUserRole = _objDal.GetUserRole(LoginUserId);
                LoginUserRole = dtUserRole.Rows[0]["Role"].ToString();
                LoginUserRoleID = Convert.ToInt32(dtUserRole.Rows[0]["RoleID"]);

                //User id admin
                if (LoginUserRoleID == 1)        // if (Role == "Admin" || Role == "admin")
                {
                    lbluser.Visible = true;
                    cmbUser.Visible = true;
                }
                else
                {
                    lbluser.Visible = false;
                    cmbUser.Visible = false;
                }
                cmbStatus.SelectedIndex = 0;
                cmbRepeateTest.SelectedIndex = 0;

                //Get all PCB types
                BindcmbPCBType();
                //Get All user role
                BindcmbUserRole();

                cmbPcbType.SelectedIndex = 0;
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                //for Control location(Screen size,Minimize,Maximize button)
                const int margin = 0;
                System.Drawing.Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                workingArea.X + margin,
                workingArea.Y + margin,
                workingArea.Width,
                workingArea.Height);
                this.Bounds = rect;

                int screenWidth = workingArea.Width;
                pikboxClose.Location = new Point(screenWidth - 40, 4);
                pikboxMinimize.Location = new Point(screenWidth - 70, 5);
                lblSignOut.Text = Username + "|Sign Out";
                lblSignOut.Location = new Point(screenWidth - 128, 48);
                btnUsbStatus.Location = new Point(screenWidth - 335, 11);

                using (Graphics g = lblSignOut.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(lblSignOut.Text, lblSignOut.Font);
                    int newX = Math.Max(screenWidth - (int)textSize.Width - 10, 0);
                    lblSignOut.Location = new Point(newX, lblSignOut.Location.Y);
                    lblSignOut.Width = (int)textSize.Width + 10;
                }

                BindVisibleFalse();
                USBCommunication.showProductSelection = false;
               // grdReport.ColumnHeaderMouseClick += GrdReport_ColumnHeaderMouseClick;
            }
            catch (Exception ex)
            {
                log1.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }

        private void GrdReport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn column = grdReport.Columns[e.ColumnIndex];
            if (column.SortMode != DataGridViewColumnSortMode.NotSortable)
            {
                // Determine the sort order
                ListSortDirection direction;
               // if (grdReport.SortedColumn != null && grdReport.SortedColumn.Index == e.ColumnIndex && grdReport.SortOrder == SortOrder.Ascending)
               if (grdReport.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                }
                grdReport.Sort(column, direction);
            }
        }

        private void ShowException(string excep)
        {           
            log1.CreateLog(excep, excep.ToString(), this.FindForm().Name);
            if (excep.Contains("does not exist") || excep.Contains("timed out"))
            {
                _msgbox.ShowBox("Please reconnect USB cable...");
                // OpenSerialPort();
            }
            else if (excep.Contains("I/O operation has been aborted") || excep.Contains("device attached to the system is not functioning"))
            {
                _msgbox.ShowBox("Please check the PCB connections", "longmsg");
            }
            else if (excep.Contains("semaphore timeout") || excep.Contains("There is no row at position"))
            {
                _msgbox.ShowBox("Please reconnect USB cable...");
            }
            else if (excep.Contains("Object reference"))
            {
                _msgbox.ShowBox("Please again click on 'Show Report' button...", "longmsg");
            }
            else if (excep.Contains("The process cannot access the file"))
            {
                _msgbox.ShowBox("Please close the opened pdf,then open new pdf file", "longmsg");
            }
            else if (excep.Contains("Port is closed"))
            {
                // OpenSerialPort();
            }
            else if (excep.Contains("denied") || (excep.Contains("Object reference")))
            {
            }
            else { MessageBox.Show(excep); }
        }
        private void BindcmbUserRole()
        {
            try
            {
                //Get all user
                System.Data.DataTable dtRole = _objDal.GetUser();
                cmbUser.DisplayMember = "Name";
                cmbUser.ValueMember = "UserID";
                DataRow row = dtRole.NewRow();
                row["Name"] = "--All--";
                dtRole.Rows.InsertAt(row, 0);
                cmbUser.DataSource = dtRole;
            }
            catch (Exception ex)
            {
                log1.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show("Exception :" + ex.Message);
            }
        }
        //Get all pcb type
        private void BindcmbPCBType()
        {
            try
            {
                System.Data.DataTable dtpcb = _objDal.GetPCBType(GlobalInformation.ProductTypeID);
                cmbPcbType.DisplayMember = "PCBType";
                cmbPcbType.ValueMember = "PCBTypeID";
                DataRow row = dtpcb.NewRow();
                row["PCBType"] = "--All--";
                dtpcb.Rows.InsertAt(row, 0);
                cmbPcbType.DataSource = dtpcb;
            }
            catch (Exception ex)
            {
                log1.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
            }
        }
        private void PikboxClose_Click(object sender, EventArgs e)   //Close report page
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            this.Close();
            Login login = new Login();
            login.Show();
        }
        private void PikboxMinimize_Click(object sender, EventArgs e)    // Minimize the form
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void TxtFromDate_Enter(object sender, EventArgs e)    // dispaly fromdate calender
        {
            monthCalendar1.Visible = true;
            btnShowReport.Visible = false;
        }
        private void TxtFromDate_Leave(object sender, EventArgs e)     // Close from date calender
        {
            if (!monthCalendar1.Focused)
                monthCalendar1.Visible = false;
            btnShowReport.Visible = true;
        }
        private void MonthCalendar1_DateSelected(object sender, DateRangeEventArgs e)   //Select date from calender
        {
            var monthCalendar = sender as MonthCalendar;
            txtFromDate.Text = monthCalendar.SelectionStart.ToString("dd/MM/yyyy");
            btnShowReport.Visible = false;
            monthCalendar1.Visible = false;
            btnShowReport.Visible = true;
        }
        private void MonthCalendar1_Leave(object sender, EventArgs e)
        {
            var monthCalendar = sender as MonthCalendar;
            monthCalendar.Visible = false;
            btnShowReport.Visible = true;
        }
        private void TxtToDate_Enter(object sender, EventArgs e)
        {
            monthCalendar2.Visible = true;
            btnShowReport.Visible = false;
        }
        private void TxtToDate_Leave(object sender, EventArgs e)
        {
            if (!monthCalendar2.Focused)
                monthCalendar2.Visible = false;
            btnShowReport.Visible = true;
        }
        private void MonthCalendar2_DateSelected(object sender, DateRangeEventArgs e)     //Select date from calender
        {
            var monthCalendar1 = sender as MonthCalendar;
            txtToDate.Text = monthCalendar1.SelectionStart.ToString("dd/MM/yyyy");
            btnShowReport.Visible = false;
            monthCalendar2.Visible = false;
            btnShowReport.Visible = true;
        }
        private void MonthCalendar2_Leave(object sender, EventArgs e)
        {
            var monthCalendar1 = sender as MonthCalendar;
            monthCalendar1.Visible = false;
            btnShowReport.Visible = true;
        }
        private void TxtToDate_MouseUp(object sender, MouseEventArgs e)
        {
            monthCalendar2.Visible = true;
            btnShowReport.Visible = false;
        }
        //Show report
        private void BtnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                int pcbtypeid;
                string status;
                string repeatTest;
                int cmbuserid;
                txtSerachGrid.Text = "";
                if (cmbPcbType.Text == "--All--")       //select all pcb type
                {
                    pcbtypeid = 0;
                   // txtSerachGrid.Location = new Point(501, 119);
                    //lblSearchgrid.Location = new Point(369, 126);
                }
                else                                     //select specific pcb
                {
                    pcbtypeid = Convert.ToInt32(cmbPcbType.SelectedValue);
                    ////txtSerachGrid.Location = new Point(501, 119);
                    ////lblSearchgrid.Location = new Point(369, 126);
                }
                if (cmbStatus.Text == "--All--")        //select all status(Pass/Fail)
                {
                    status = "All";
                }
                else
                {                                  //Select specific status
                    status = cmbStatus.Text;
                }
                if (cmbRepeateTest.Text == "--All--")   //Select all repeat test(yes/no)
                {
                    repeatTest = "All";
                }
                else                                    //Select specific test yes or no
                {
                    repeatTest = cmbRepeateTest.Text;
                }
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    _msgbox.ShowBox("Please select date");
                }
                if (DateTime.Parse(txtFromDate.Text) > DateTime.Parse(txtToDate.Text))
                { 
                    _msgbox.ShowBox("From Date should not be greater than To Date", "longmsg");
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //DataTable dtUserRole = _objDal.GetUserRole(LoginUserId);
                    //string Role = dtUserRole.Rows[0]["Role"].ToString();
                    ////if (Role == "Admin" || Role == "admin")
                    if(LoginUserRoleID == 1)
                    {
                        if (cmbUser.Text == "--All--")    //for all user
                        {
                            cmbuserid = 0;
                            BindGridForAll(pcbtypeid, status, repeatTest, txtFromDate.Text, txtToDate.Text, cmbuserid);
                        }
                        else                            //for particular user
                        {
                            cmbuserid = Convert.ToInt32(cmbUser.SelectedValue);
                            BindGridForAll(pcbtypeid, status, repeatTest, txtFromDate.Text, txtToDate.Text, cmbuserid);
                        }
                    }
                    else
                    {
                        BindGridForAll(pcbtypeid, status, repeatTest, txtFromDate.Text, txtToDate.Text, LoginUserId);  // login user
                    }
                    grdReport.ClearSelection();
                    Cursor.Current = Cursors.Arrow;
                    txtSerachGrid.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowException(ex.Message);
            }
        }

        //Bind gridview
        private void BindGridForAll(int pcbtypeid, string status, string repeatTest, string Fromdate, string Todate, int LoginUserId)
        {
            DataTable dtSerialNo = new DataTable();
            dt = new DataTable();
            DataTable tblFiltered = new DataTable();
            //if (dt.Rows.Count != 0) { dt.Clear(); } else { }
            //if (tblFiltered.Rows.Count != 0) { tblFiltered.Clear(); } else { }
            //if (dtSerialNo.Rows.Count != 0) { dtSerialNo.Clear(); } else { }
            dt.Clear();
            tblFiltered.Clear();
            dtSerialNo.Clear();

            //Fetch Data
            tblFiltered = _objDal.RptGetPCBTypeWise(0, "All", Fromdate, Todate, LoginUserId);   // for admin 
            DataTable dtStatus = _objDal.RptGetPCBTestStatus(Fromdate, Todate, LoginUserId);
            dtStatus = UpdateTestCaseStatus(dtStatus, Fromdate, Todate);

            //Merge Data
            dtStatus.PrimaryKey = new DataColumn[] { dtStatus.Columns["ResultId"] };
            tblFiltered.PrimaryKey = new DataColumn[] { tblFiltered.Columns["ResultId"] };
            tblFiltered.Merge(dtStatus);


            //// Filtering Data
            //var filteredData = tblFiltered.AsEnumerable();

            //if (pcbtypeid != 0)
            //    filteredData = filteredData.Where(row => row.Field<int>("PCBTypeID") == pcbtypeid);
            //if (status != "All")
            //    filteredData = filteredData.Where(row => row.Field<string>("Status") == status);

            //dt = filteredData.Any() ? filteredData.CopyToDataTable() : new DataTable();

            int rowCount = tblFiltered.Rows.Count;
            if (rowCount > 0)
            {
                ////for (int i = tblFiltered.Rows.Count - 1; i >= 0; i--)
                ////{
                ////    if (tblFiltered.Rows[i]["PCBTypeID"].ToString() == "" || tblFiltered.Rows[i]["SerialNumber"].ToString() == "")
                ////        tblFiltered.Rows[i].Delete();
                ////}
                ////tblFiltered.AcceptChanges();
                ///
                              
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    DataRow currentRow = tblFiltered.Rows[i];
                    if (string.IsNullOrEmpty(currentRow["PCBTypeID"].ToString()) || string.IsNullOrEmpty(currentRow["SerialNumber"].ToString()))
                    {
                        currentRow.Delete();
                    }
                }

                //var filteredRows = tblFiltered.AsEnumerable();
                var filteredRows = tblFiltered.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted);

                if (pcbtypeid != 0)
                {
                    filteredRows = filteredRows.Where(row => row.Field<int>("PCBTypeID") == pcbtypeid);
                }

                if (status != "All")
                {
                    filteredRows = filteredRows.Where(row => row.Field<string>("Status") == status);
                }

                if (filteredRows.Any())
                {
                    dt = filteredRows.CopyToDataTable();
                }

                //if (pcbtypeid != 0 && status != "All")    //For particular pcb type and particular status.
                //{
                //    var vardt = tblFiltered.AsEnumerable()
                //      .Where(row => row.Field<int>("PCBTypeID") == pcbtypeid
                //       && row.Field<String>("Status") == status);
                //    int dtcount = vardt.Count();
                //    if (dtcount > 0)
                //    {
                //        dt = vardt.CopyToDataTable();
                //    }
                //    else { }
                //}

                //if (pcbtypeid != 0 && status != "All")    //For particular pcb type and particular status.
                //{
                //    var filteredRows = tblFiltered.AsEnumerable()
                //        .Where(row => row.Field<int>("PCBTypeID") == pcbtypeid
                //                    && row.Field<string>("Status") == status);

                //    if (filteredRows.Any())
                //    {
                //        dt = filteredRows.CopyToDataTable();
                //    }
                //}
                //else if (pcbtypeid == 0 && status != "All")   //For particular all pcb type and particular status.
                //{
                //    var filteredRows = tblFiltered.AsEnumerable()
                //     .Where(row => row.Field<string>("Status") == status);
                //   // int dtcount = vardt.Count();
                //    if (filteredRows.Any())
                //    {
                //        dt = filteredRows.CopyToDataTable();
                //    }
                //   // else { }
                //}
                //else if (pcbtypeid != 0 && status == "All")             //For particular pcb type and all status.
                //{
                //    var filteredRows = tblFiltered.AsEnumerable()
                //      .Where(row => row.Field<int>("PCBTypeID") == pcbtypeid);
                //   // int dtcount = vardt.Count();
                //    if (filteredRows.Any())
                //    {
                //        dt = filteredRows.CopyToDataTable();
                //    }

                //}
                //else if (pcbtypeid == 0 && status == "All")             //For all pcb type and all status.
                //{
                //    var filteredRows = tblFiltered.AsEnumerable();
                //   // int dtcount = vardt.Count();
                //    if (filteredRows.Any())
                //    {
                //        dt = filteredRows.CopyToDataTable();
                //    }                   
                //}
                if (dt.Rows.Count > 0)
                {
                    dtSerialNo = _objDal.GetRepeatTest(repeatTest, LoginUserId, Fromdate, Todate);
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["ResultId"] };
                    dtSerialNo.PrimaryKey = new DataColumn[] { dtSerialNo.Columns["ResultId"] };
                    string[] colsToRemove = { "RNUM", "Name", "SerialNo" };
                   
                    dt.Merge(dtSerialNo);
                    foreach (string col in colsToRemove)
                    {
                        if (dt.Columns.Contains(col))
                            dt.Columns.Remove(col);
                    }

                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        bool deleteRow = false;
                        if (repeatTest == "All")
                        {
                            deleteRow = string.IsNullOrEmpty(dt.Rows[i]["PCBTypeID"].ToString());
                        }
                        else
                        {
                            deleteRow = string.IsNullOrEmpty(dt.Rows[i]["yesno"].ToString()) || string.IsNullOrEmpty(dt.Rows[i]["PCBTypeID"].ToString());
                        }

                        if (deleteRow)
                        {
                            dt.Rows[i].Delete();
                        }
                    }

                    //if (repeatTest == "All")
                    //{
                    //    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    //    {
                    //        if (string.IsNullOrEmpty(dt.Rows[i]["PCBTypeID"].ToString()))
                    //            dt.Rows[i].Delete();
                    //    }
                    //    //dt.AcceptChanges();
                    //}
                    //else
                    //{
                    //    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    //    {
                    //        if ((string.IsNullOrEmpty(dt.Rows[i]["yesno"].ToString())) || string.IsNullOrEmpty(dt.Rows[i]["PCBTypeID"].ToString()))
                    //            dt.Rows[i].Delete();
                    //    }
                    //   // dt.AcceptChanges();
                    //}



                    if (NewDs.Tables.Contains("Table1"))
                    {
                        NewDs.Tables.Remove("Table1");
                    }
                    // Sort dt by descending order    

                    //DataView dv = dt.DefaultView;
                    //dv.Sort = "ResultId desc";
                    //dt = dv.ToTable();
                    //NewDs.Tables.Add(dt);

                   // dt.DefaultView.Sort = "ResultId DESC";
                    //DataTable  SortDt = dt.DefaultView.ToTable();
                    DataTable SortDt = dt.AsEnumerable()
                   .Where(row => !row.RowState.Equals(DataRowState.Deleted) && !string.IsNullOrEmpty(row["ResultId"].ToString()))
                   .OrderByDescending(row => Convert.ToInt32(row["ResultId"]))
                   .CopyToDataTable();
                    NewDs.Tables.Add(SortDt);
                    scr_val = 1;
                    grdReport.DataSource = null;
                                     

                    if (SortDt.Rows.Count > 0 && SortDt.Rows[0]["PCBTypeID"].ToString() != "")
                    {
                        i1 = 0;
                        SortDt = (NewDs.Tables[0].Select().Skip(i1).Take(12)).CopyToDataTable();
                        grdReport.AutoGenerateColumns = false;
                        grdReport.ColumnCount = 8;

                        grdReport.Columns[0].Name = "PCBTypeID";
                        grdReport.Columns[0].HeaderText = "PCBTypeID";
                        grdReport.Columns[0].DataPropertyName = "PCBTypeID";
                        grdReport.Columns[0].Visible = false;

                        grdReport.Columns[1].Name = "PCBType";
                        grdReport.Columns[1].HeaderText = "PCB Type";
                        grdReport.Columns[1].DataPropertyName = "PCBType";
                        grdReport.Columns[1].Width = 100;

                        grdReport.Columns[2].Name = "SerialNumber";
                        grdReport.Columns[2].HeaderText = "Serial No";
                        grdReport.Columns[2].DataPropertyName = "SerialNumber";
                        grdReport.Columns[2].DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11, FontStyle.Regular | FontStyle.Underline);
                        grdReport.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        grdReport.Columns[3].Name = "TestCaseID";
                        grdReport.Columns[3].HeaderText = "TestCaseID";
                        grdReport.Columns[3].DataPropertyName = "TestCaseID";
                        grdReport.Columns[3].Visible = false;

                        grdReport.Columns[4].Name = "TestCaseName";
                        grdReport.Columns[4].HeaderText = "Test Case";
                        grdReport.Columns[4].DataPropertyName = "TestCaseName";
                        grdReport.Columns[4].Visible = false;

                        grdReport.Columns[5].Name = "Status";
                        grdReport.Columns[5].HeaderText = "Status";
                        grdReport.Columns[5].DataPropertyName = "Status";

                        grdReport.Columns[6].HeaderText = "Date";
                        grdReport.Columns[6].DataPropertyName = "Date";
                        grdReport.Columns[6].Name = "Date";
                        grdReport.Columns[6].Visible = true;

                        grdReport.Columns[7].HeaderText = "Repeat Test";
                        grdReport.Columns[7].DataPropertyName = "yesno";
                        grdReport.Columns[7].Name = "yesno";
                        grdReport.Columns[7].Visible = true;
                        grdReport.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        DataGridViewLinkColumn dgvLink = new DataGridViewLinkColumn();
                        dgvLink.UseColumnTextForLinkValue = true;
                        dgvLink.LinkBehavior = LinkBehavior.SystemDefault;
                        dgvLink.HeaderText = "Details";
                        dgvLink.Name = "Details";
                        dgvLink.LinkColor = Color.Blue;
                        dgvLink.TrackVisitedState = true;
                        dgvLink.Text = "View";
                        dgvLink.UseColumnTextForLinkValue = true;
                        grdReport.Columns.Add(dgvLink);

                        grdReport.Columns[1].SortMode = DataGridViewColumnSortMode.Automatic;
                        grdReport.DataSource = SortDt;

                        if (NewDs.Tables[0].Rows.Count % 12 != 0)
                        {
                            lblTotalpages.Text = ((NewDs.Tables[0].Rows.Count / 12) + 1).ToString();
                        }
                        else
                        {
                            lblTotalpages.Text = (NewDs.Tables[0].Rows.Count / 12).ToString();
                        }

                        if (pcbtypeid == 0) { grdReport.Columns[1].Visible = true; } else { grdReport.Columns[1].Visible = false; }
                        lblpages.Text = (scr_val).ToString();
                        btnImportToExcel.Visible = true;
                        btnPreviousPage.Enabled = false;
                        btnPreviousPage.Visible = true;
                        if (Convert.ToInt32(lblpages.Text) == Convert.ToInt32(lblTotalpages.Text))
                        {
                            btnNextPage.Enabled = false;
                        }
                        else
                        {
                            btnNextPage.Enabled = true;
                        }
                        btnNextPage.Visible = true;
                        lblOf.Visible = true;
                        lblTotalpages.Visible = true;
                        lblpages.Visible = true;
                        lblNoRecords.Visible = false;
                        grdReport.Visible = true;
                        lblsorting.Visible = true;
                        lblSearchgrid.Visible = true;
                        txtSerachGrid.Visible = true;
                        grdReport.RowHeadersVisible = false;
                    }
                    else
                    {
                        BindVisibleFalse();
                    }
                }
                else
                {
                    BindVisibleFalse();
                }
            }
            else
            {
                BindVisibleFalse();
            }
        }

        private DataTable UpdateTestCaseStatus(DataTable dtStatus, string Fromdate, string Todate)
        {
            try
            {
                string SerialNumber = string.Empty;
                int pcbTypeId = 0;
                string FinalTestCaseStatus = string.Empty;

                foreach (DataRow dr in dtStatus.Rows)
                {
                    SerialNumber = Convert.ToString(dr["SerialNumber"]);
                    pcbTypeId = Convert.ToInt32(dr["pcbTypeID"]);

                    System.Data.DataTable dtpdf1 = _objDal.GetSerialWiseData(LoginUserId, SerialNumber, pcbTypeId, Fromdate, Todate);
                    System.Data.DataView view = new System.Data.DataView(dtpdf1);

                    FinalTestCaseStatus = GetTestCaseStatus(pcbTypeId, view);
                    dr["Status"] = FinalTestCaseStatus;
                }
            }
            catch
            {
                throw;
            }
            return dtStatus;
        }

        private void BindVisibleFalse()
        {
            btnImportToExcel.Visible = false;
            btnPreviousPage.Visible = false;
            btnNextPage.Visible = false;
            lblOf.Visible = false;
            lblTotalpages.Visible = false;
            lblpages.Visible = false;
            grdReport.Visible = false;
            lblNoRecords.Visible = true;
            lblsorting.Visible = false;
            lblSearchgrid.Visible = false;
            txtSerachGrid.Visible = false;
        }

        //Sign out
        private void LblSignOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
                this.Close();
                Login login = new Login();
                login.Show();
            }
            catch (Exception)
            { }
        }

        //Click on View link in grid
        private void GrdReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtSerachGrid.Focus();
                int rowIndex = e.RowIndex;
                DataGridViewCell cell = null;
                if (rowIndex != -1)
                {
                    cell = (DataGridViewCell)grdReport.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
                if (rowIndex == -1)
                {  // Column header row
                  
                }
                else if (cell.ColumnIndex == this.grdReport.Columns["Details"].Index)
                {
                    string SerialNo = grdReport.Rows[rowIndex].Cells["SerialNumber"].Value.ToString();
                    int pcbTypeId = Convert.ToInt32(grdReport.Rows[rowIndex].Cells["PCBTypeID"].Value.ToString());
                    int testcaseid = Convert.ToInt32(grdReport.Rows[rowIndex].Cells["TestCaseID"].Value.ToString());
                   // DataTable dtUserRole = _objDal.GetUserRole(LoginUserId);
                   // string Role = dtUserRole.Rows[0]["Role"].ToString();
                    int cmbuserid;
                   // if (Role == "Admin" || Role == "admin")
                        if (LoginUserRoleID == 1)
                        {
                        if (cmbUser.Text == "--All--")
                        {
                            cmbuserid = 0;
                            OpenTestCasesInPdf(cmbuserid, SerialNo, pcbTypeId, testcaseid, txtFromDate.Text, txtToDate.Text);
                        }
                        else
                        {
                            cmbuserid = Convert.ToInt32(cmbUser.SelectedValue);
                            OpenTestCasesInPdf(cmbuserid, SerialNo, pcbTypeId, testcaseid, txtFromDate.Text, txtToDate.Text);
                        }
                    }
                    else
                    {
                        OpenTestCasesInPdf(LoginUserId, SerialNo, pcbTypeId, testcaseid, txtFromDate.Text, txtToDate.Text);
                    }
                }
                Cursor.Current = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                ShowException(ex.Message);
            }
        }

        // Open data in pdf file     
        private void OpenTestCasesInPdf(int LoginUserId, string SerialNo, int pcbTypeId, int testcaseid, string Fromdate, string Todate)
        {
            try
            {
                System.Data.DataTable dtpdf1 = _objDal.GetSerialWiseData(LoginUserId, SerialNo, pcbTypeId, Fromdate, Todate);
                System.Data.DataView view = new System.Data.DataView(dtpdf1);
                System.Data.DataTable dtpdf;
                ////dtpdf = view.ToTable("dtpdf", false, "TestCaseName", "Comment", "Date", "Status");
                dtpdf = view.ToTable("dtpdf", false, "TestCaseName", "Date", "Status");

                int rowcount = dtpdf.Rows.Count;
                DataColumn Col = dtpdf.Columns.Add("SrNo", typeof(int));
                Col.SetOrdinal(0);
                for (int i = 1; i <= rowcount; i++)
                {
                    dtpdf.Rows[i - 1]["SrNo"] = i;
                }
                string folderPath = "ESD PDFs\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fname = "";
                string filename1 = "ESD Report PDFs" + System.DateTime.Now.ToString("mm:ss");
                int index1 = filename1.Trim().IndexOf(":");
                if (index1 != -1)
                {
                    fname = filename1.Remove(index1, 1);
                }

                string FileNm = string.Format(@"{0}.pdf", Guid.NewGuid());   //For random pdf file name
                string filename = folderPath + FileNm;                       

                Document document = new Document(PageSize.A4, 10f, 10f, 60f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
                document.Open();

                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 10);
                iTextSharp.text.Font Columnfont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                iTextSharp.text.Font Headerfont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                iTextSharp.text.Font Titlefont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                iTextSharp.text.Font datefont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 9);
                PdfPTable table = new PdfPTable(dtpdf.Columns.Count);

                ////table.SetWidths(new float[] { 0.7f, 2, 2, 2, 1.3f });
                table.SetWidths(new float[] { 0.7f, 2, 2, 1.3f });


                table.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY;

                string imgFile = "Images\\godrej-logo.png";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imgFile);
                //Resize image depend upon your need               
                jpg.ScaleToFit(80f, 60f);   //140,120
                jpg.SetAbsolutePosition(70, 750);

                //Header              
                PdfPCell headerCell = new PdfPCell(new Phrase(new Chunk("")));
                headerCell.Border = 0;
                headerCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                table.AddCell(headerCell);

                //Title
                string title = "Test Report";
                PdfPCell titleCell = new PdfPCell(new Phrase(new Chunk(title, Titlefont)));
                titleCell.Border = 0;
                titleCell.Colspan = 3;
                titleCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(titleCell);

                PdfPCell BlankCell1 = new PdfPCell(new Phrase(new Chunk(" ")));
                BlankCell1.Border = 0;
                BlankCell1.Colspan = 5;
                table.AddCell(BlankCell1);

                ////CurrentDate
                //string todaydate = "Date:" + (System.DateTime.Now).ToString("dd/MM/yyyy");
                //PdfPCell todaydateCell = new PdfPCell(new Phrase(new Chunk(todaydate, datefont)));
                //todaydateCell.Border = 0;
                //todaydateCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //table.AddCell(todaydateCell);

                string todaydate = (System.DateTime.Now).ToString("dd/MM/yyyy");
                PdfPCell todaydateCell = new PdfPCell(new Phrase(new Chunk("Date -  " + todaydate, font5)));
                todaydateCell.Colspan = 5;
                todaydateCell.Border = 0;
                todaydateCell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(todaydateCell);


                PdfPCell BlankCell = new PdfPCell(new Phrase(new Chunk(" ")));
                BlankCell.Border = 0;
                BlankCell.Colspan = 5;
                table.AddCell(BlankCell);

                string adr2 = dtpdf1.Rows[0]["PCBType"].ToString();
                PdfPCell addrchk = new PdfPCell(new Phrase(new Chunk(adr2, Headerfont)));
                addrchk.Colspan = 5;
                addrchk.BorderColor = BaseColor.LIGHT_GRAY;
                addrchk.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(addrchk);

                string adr1 = dtpdf1.Rows[0]["SerialNo"].ToString();
                PdfPCell SerialnoCell = new PdfPCell(new Phrase(new Chunk("SerialNo -  " + adr1, font5)));
                SerialnoCell.Colspan = 5;
                SerialnoCell.BorderColor = BaseColor.LIGHT_GRAY;
                SerialnoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(SerialnoCell);

                string usernm = dtpdf1.Rows[0]["Name"].ToString();
                PdfPCell NameCell = new PdfPCell(new Phrase(new Chunk("Username -  " + usernm, font5)));
                NameCell.Colspan = 5;
                NameCell.BorderColor = BaseColor.LIGHT_GRAY;
                NameCell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(NameCell);

                string testCaseStatus = string.Empty;
                testCaseStatus = GetTestCaseStatus(pcbTypeId, view);

                PdfPCell TestCaseStatus = new PdfPCell(new Phrase(new Chunk("Test Case Status  -  " + testCaseStatus, font5)));
                TestCaseStatus.Colspan = 5;
                TestCaseStatus.BorderColor = BaseColor.LIGHT_GRAY;
                TestCaseStatus.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(TestCaseStatus);


                foreach (DataColumn c in dtpdf.Columns)
                {
                    PdfPCell ColumnCell = new PdfPCell(new Phrase(c.ColumnName, Columnfont));
                    ColumnCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    ColumnCell.BorderColor = BaseColor.LIGHT_GRAY;
                    if (dtpdf.Columns[1].ColumnName != "Test Cases")
                    {
                        dtpdf.Columns["TestCaseName"].ColumnName = "Test Cases";
                    }
                    ////if (dtpdf.Columns[2].ColumnName != "Measured Value")
                    ////{
                    ////    dtpdf.Columns["Comment"].ColumnName = "Measured Value";
                    ////}
                    table.AddCell(ColumnCell);
                }

                foreach (DataRow r in dtpdf.Rows)
                {
                    if (dtpdf.Rows.Count > 0)
                    {
                        PdfPCell SrCell = new PdfPCell(new Phrase(r[0].ToString(), font5));
                        SrCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        SrCell.BorderColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(SrCell);
                        table.AddCell(new Phrase(r[1].ToString(), font5));
                        table.AddCell(new Phrase(r[2].ToString(), font5));
                        table.AddCell(new Phrase(r[3].ToString(), font5));
                       // table.AddCell(new Phrase(r[4].ToString(), font5));
                    }
                }
                document.Add(jpg);
                document.Add(table);
                document.Close();

                Process.Start(filename);
            }
            catch (Exception ex)
            {
                log1.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        private string GetTestCaseStatus(int pcbTypeId, DataView view)
        {
            string testCaseStatus = string.Empty;
            try
            {
                System.Data.DataTable dtpdf = view.ToTable("dtpdf", false, "TestCaseName", "Comment", "Date", "Status", "TestCaseID");

                DataTable dtmanual = _objDal.GetPCBTestCases(pcbTypeId, "Manual");
                DataTable dtAuto = _objDal.GetPCBTestCases(pcbTypeId, "Auto");
                DataTable dtHybrid = _objDal.GetPCBTestCases(pcbTypeId, "Hybrid");
                int keyPadTest = 0;
                if (pcbTypeId == 4 || pcbTypeId == 6)   // 4-is for hhd controller|| 6-keypad pcb
                    keyPadTest = 1;

                int totalPCBTestCaeses = dtmanual.Rows.Count + dtAuto.Rows.Count + dtHybrid.Rows.Count + keyPadTest;
                DataTable dtResultTable = new DataTable();

                if (dtpdf.Rows.Count > 0)
                {
                    dtResultTable = dtpdf.AsEnumerable()
                         .GroupBy(r => r["TestCaseID"])
                         .Select(g =>
                         {
                             var row = dtpdf.NewRow();
                             row["TestCaseID"] = g.Key;
                             row["Status"] = g.Select(R => R["Status"]).FirstOrDefault();
                             row["Date"] = g.Max(r => r["Date"]);
                             return row;
                         }).CopyToDataTable();
                }

                if (totalPCBTestCaeses <= dtResultTable.Rows.Count)
                {
                    bool isTimeout = false;
                    foreach (DataRow dr in dtResultTable.Rows)
                    {
                        if (Convert.ToString(dr["Status"]) != "Pass")
                        {
                            if (Convert.ToString(dr["Status"]) == "Timeout")
                            {
                                isTimeout = true;
                                testCaseStatus = "Timeout. One of the test case timeout.";
                            }
                            else
                            {
                                testCaseStatus = "Fail. One of the test case failed.";
                                break;
                            }
                        }
                        else
                            testCaseStatus = "Pass";
                    }

                    if (isTimeout)
                        testCaseStatus = "Timeout. One of the test case timeout.";
                }
                else
                {
                    bool isTimeout = false;
                    foreach (DataRow dr in dtResultTable.Rows)
                    {
                        if (Convert.ToString(dr["Status"]) != "Pass")
                        {
                            if (Convert.ToString(dr["Status"]) == "Timeout")
                            {
                                isTimeout = true;
                                testCaseStatus = "Timeout. One of the test case timeout.";
                            }
                            else
                            {
                                testCaseStatus = "Fail. All test cases not conducted.";
                                break;
                            }
                        }
                        else
                            testCaseStatus = "Fail. All test cases not conducted.";
                    }

                    if (isTimeout)
                        testCaseStatus = "Timeout. All test cases not conducted.";
                }
            }
            catch
            {
                throw;
            }
            return testCaseStatus;
        }

        private void BtnImportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //DataTable dtgrid = new DataTable();
                DataTable dtgrid = NewDs.Tables[0];
                dtgrid.PrimaryKey = null;
                //string[] cols = (from dc in dtgrid.Columns.Cast<DataColumn>()
                            //     select dc.ColumnName).ToArray();
                //if (dtgrid.Rows.Count > 0)
                //{
                //    if (dtgrid.Columns[5].ColumnName != "Repeat Test")
                //    {
                //        dtgrid.Columns["yesno"].ColumnName = "Repeat Test";
                //    }
                //    else { }
                //}

                // Create a new DataTable for PDF generation
                DataTable pdfDataTable = dtgrid.Copy();
                if (pdfDataTable.Rows.Count > 0)
                {
                    if (pdfDataTable.Columns[5].ColumnName != "Repeat Test")
                    {
                        pdfDataTable.Columns["yesno"].ColumnName = "Repeat Test";
                    }                   
                }
                // Remove unwanted columns from the new DataTable
                string[] colsToRemove = { "TestCaseID", "TestCaseName", "PcbTypeID", "ResultId", "RNUM", "Name", "Details", "SerialNo" };
                foreach (string col in colsToRemove)
                {
                    if (pdfDataTable.Columns.Contains(col))
                        pdfDataTable.Columns.Remove(col);
                }
                //foreach (string col in cols)
                //{
                //    if (col == "TestCaseID" || col == "TestCaseName" || col == "PcbTypeID" || col == "ResultId" || col == "RNUM" || col == "Name" || col == "Details" || col == "SerialNo")
                //    {
                //        pdfDataTable.Columns.Remove(col);
                //    }
                //}

                // Modify other operations to use pdfDataTable instead of dtgrid
                pdfDataTable.AcceptChanges();

                // Adding PdfPTable
               // PdfPTable pdfTable = new PdfPTable(pdfDataTable.Columns.Count);

                PdfPTable pdfTable = new PdfPTable(6);       //dtgrid.Columns.Count + 1
                pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.DefaultCell.BorderWidth = 1;
                pdfTable.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY;

                //if (pdfDataTable.Columns[0].ColumnName != "SrNo")
                //{
                //    int rowcount = pdfDataTable.Rows.Count;
                //    DataColumn Col = pdfDataTable.Columns.Add("SrNo", typeof(int));
                //    Col.SetOrdinal(0);
                //    for (int i = 1; i <= rowcount; i++)
                //    {
                //        pdfDataTable.Rows[i - 1]["SrNo"] = i;
                //    }
                //}

                if (!pdfDataTable.Columns.Contains("SrNo"))
                {
                   // pdfDataTable.Columns.Add("SrNo", typeof(int));
                    pdfDataTable.Columns.Add("SrNo", typeof(int)).SetOrdinal(0);
                    for (int i = 0; i < dtgrid.Rows.Count; i++)
                    {
                        pdfDataTable.Rows[i]["SrNo"] = i + 1;
                    }
                }

                // pdfTable.SetWidths(new float[] { 0.7f, 2, 2, 2, 1.3f, 2 });     //Set column width          

                string imgFile = "Images\\godrej-logo.png";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imgFile);
                //Resize image depend upon your need               
                jpg.ScaleToFit(80f, 60f);   //140,120
                jpg.SetAbsolutePosition(70, 750);

                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 10);
                iTextSharp.text.Font Columnfont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                iTextSharp.text.Font Headerfont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                iTextSharp.text.Font Titlefont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                iTextSharp.text.Font datefont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 9);

                //Header              
                PdfPCell headerCell = new PdfPCell(new Phrase(new Chunk("")));
                headerCell.Border = 0;
                headerCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                pdfTable.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY;
                pdfTable.AddCell(headerCell);

                //Title
                string title = "PCB Test Report";
                PdfPCell titleCell = new PdfPCell(new Phrase(new Chunk(title, Titlefont)));
                titleCell.Border = 0;
                titleCell.Colspan = 4;
                titleCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                pdfTable.AddCell(titleCell);

                //CurrentDate
                string todaydate = "Date:" + (System.DateTime.Now).ToString("dd/MM/yyyy");
                PdfPCell todaydateCell = new PdfPCell(new Phrase(new Chunk(todaydate, datefont)));
                todaydateCell.Border = 0;
                todaydateCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                pdfTable.AddCell(todaydateCell);

                PdfPCell BlankCell = new PdfPCell(new Phrase(new Chunk(" ")));
                BlankCell.Border = 0;
                BlankCell.Colspan = 6;
                pdfTable.AddCell(BlankCell);

                //Add user name
                string usrname = "User name: " + Username;
                PdfPCell usernmcell = new PdfPCell(new Phrase(new Chunk(usrname, Columnfont)));
                usernmcell.Border = 0;
                usernmcell.Colspan = 6;
                usernmcell.PaddingBottom = 5;
                usernmcell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                pdfTable.AddCell(usernmcell);

                // Adding Header row
                //for (int i = 0; i < pdfDataTable.Columns.Count; i++)
                //{
                //    string cellText = (pdfDataTable.Columns[i].ColumnName);
                //    PdfPCell cell = new PdfPCell();
                //    cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"))));
                //    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C8C8C8"));
                //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //    cell.PaddingBottom = 5;
                //    pdfTable.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY;
                //    pdfTable.AddCell(cell);
                //}
                // Adding Header row
                foreach (DataColumn column in pdfDataTable.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, 1, BaseColor.BLACK)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingBottom = 5;
                    pdfTable.AddCell(cell);
                }

                //// Writing table Data
                //for (int i = 0; i < pdfDataTable.Rows.Count; i++)
                //{
                //    for (int j = 0; j < pdfDataTable.Columns.Count; j++)
                //    {
                //        pdfTable.AddCell(new Phrase(pdfDataTable.Rows[i][j].ToString(), font5));
                //    }
                //}

                // Add data rows
                foreach (DataRow row in pdfDataTable.Rows)
                {
                    foreach (object item in row.ItemArray)
                    {
                        pdfTable.AddCell(new Phrase(item.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    }
                }

                //Exporting to PDF              
                string folderPath = "ESD Report PDFs\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fname = "";
                fname = "Report_" + System.DateTime.Now.ToString("ddMMyyyy_HHmmss");

                string filename = folderPath + fname + ".pdf";
                //dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 60f, 50f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(jpg);
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
                Process.Start(filename);
                Cursor.Current = Cursors.Arrow;
            }
            catch (Exception ex )
            { log1.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name); }            
        }

        // Gridview next page
        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                scr_val = scr_val + 1;
                if (scr_val <= Convert.ToInt32(lblTotalpages.Text))
                {
                    i1 = i1 + 12;
                    DataTable dtNext = (NewDs.Tables[0].Select().Skip(i1).Take(12)).CopyToDataTable();
                    lblpages.Text = (scr_val).ToString();
                    btnNextPage.Enabled = true;
                    btnPreviousPage.Enabled = true;
                    grdReport.DataSource = dtNext;
                    grdReport.ClearSelection();
                }
                else
                {
                    btnNextPage.Enabled = false;
                    btnPreviousPage.Enabled = true;
                }
                if (scr_val == Convert.ToInt32(lblTotalpages.Text))
                {
                    btnNextPage.Enabled = false;
                    btnPreviousPage.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log1.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        // Gridview previous page
        private void BtnPreviousPage_Click(object sender, EventArgs e)
        {
            try
            {
                scr_val = scr_val - 1;
                if (scr_val != 0)
                {
                    i1 = i1 - 12;
                    DataTable dtPrevious = (NewDs.Tables[0].Select().Skip(i1).Take(12)).CopyToDataTable();
                    grdReport.DataSource = dtPrevious;
                    lblpages.Text = (scr_val).ToString();
                    btnPreviousPage.Enabled = true;
                    btnNextPage.Enabled = true;
                    grdReport.ClearSelection();
                }
                else
                {
                    btnPreviousPage.Enabled = false;
                    btnNextPage.Enabled = true;
                }
                if (Convert.ToInt32(lblpages.Text) == 1)
                {
                    btnPreviousPage.Enabled = false;
                    btnNextPage.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log1.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }
        private void LblReportSignout_Click(object sender, EventArgs e)
        {
            this.Close();
            Login login = new Login();
            login.Show();
        }

        //Background process for USB auto detection
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Select connected devices
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(DeviceRemovedEvent);
            removeWatcher.Start();
        }

        //Connect USB cable (Only allowed NXP cable)
        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub")) // This line fetch all the connected device with windows query
            {
                collection = searcher.Get();
            }
            foreach (var device in collection)
            {
                PIdVIdDeviceID = (string)device.GetPropertyValue("PNPDeviceID");
                //if (PIdVIdDeviceID == "USB\\VID_1FC9&PID_0083\\NXP-77")
                if (PIdVIdDeviceID == "USB\\VID_067B&PID_2303\\5&521A615&0&9")
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

        //Disconnect usb cable
        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
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
                if (PIdVIdDeviceID == "USB\\VID_067B&PID_2303\\5&521A615&0&9")
                {
                    btnUsbStatus.BackColor = Color.Silver;
                    return;
                }
                else
                {
                    btnUsbStatus.BackColor = Color.Silver;
                }
            }
            collection.Dispose();
        }
        private void LblTestcases_Click(object sender, EventArgs e)
        {          
           // Cursor.Current = Cursors.WaitCursor;
            this.Close();
            PCBTest testform = new PCBTest(LoginUserId, Username);
            testform.Show();
           // Cursor.Current = Cursors.Arrow;
        }

        //Search record report gridview by serial number
        private void TxtSerachGrid_TextChanged(object sender, EventArgs e)
        {
            DataTable datatable = NewDs.Tables[0];
            DataView DV = new DataView(datatable);
            DV.RowFilter = string.Format("SerialNumber LIKE '%{0}%'", txtSerachGrid.Text);
            grdReport.DataSource = DV;
            grdReport.ClearSelection();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            string myPort = STMUSBCommunication.AlphaPort();
            //bool checkOpen = STMUSBCommunication.OpenAlphaPort(myPort);
            if (myPort != "")            //if (checkOpen)
            {
                btnUsbStatus.BackColor = Color.Green;
            }
            else
            {
                btnUsbStatus.BackColor = Color.Silver;
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

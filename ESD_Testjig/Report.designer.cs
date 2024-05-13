namespace ESD_Testjig
{
    partial class Report
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblUserNm = new System.Windows.Forms.Label();
            this.lblReportSignout = new System.Windows.Forms.Label();
            this.pikboxClose = new System.Windows.Forms.PictureBox();
            this.lblSignOut = new System.Windows.Forms.Label();
            this.pikboxMinimize = new System.Windows.Forms.PictureBox();
            this.monthCalendar2 = new System.Windows.Forms.MonthCalendar();
            this.btnShowReport = new System.Windows.Forms.Button();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.btnImportToExcel = new System.Windows.Forms.Button();
            this.txtFromDate = new System.Windows.Forms.TextBox();
            this.txtToDate = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbRepeateTest = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPcbType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grdReport = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.lbluser = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSearchgrid = new System.Windows.Forms.Label();
            this.txtSerachGrid = new System.Windows.Forms.TextBox();
            this.grdExcel = new System.Windows.Forms.DataGridView();
            this.lblsorting = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblReport = new System.Windows.Forms.Label();
            this.btnUsbStatus = new System.Windows.Forms.Button();
            this.lblTestcases = new System.Windows.Forms.Label();
            this.lblNoRecords = new System.Windows.Forms.Label();
            this.lblOf = new System.Windows.Forms.Label();
            this.lblpages = new System.Windows.Forms.Label();
            this.lblTotalpages = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.lblReportTitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pikboxClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pikboxMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdReport)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExcel)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlText;
            this.pnlHeader.Controls.Add(this.panel5);
            this.pnlHeader.Controls.Add(this.pictureBox1);
            this.pnlHeader.Controls.Add(this.label5);
            this.pnlHeader.Controls.Add(this.lblUserNm);
            this.pnlHeader.Controls.Add(this.lblReportSignout);
            this.pnlHeader.Controls.Add(this.pikboxClose);
            this.pnlHeader.Controls.Add(this.lblSignOut);
            this.pnlHeader.Controls.Add(this.pikboxMinimize);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1370, 81);
            this.pnlHeader.TabIndex = 9;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.ForeColor = System.Drawing.SystemColors.Control;
            this.panel5.Location = new System.Drawing.Point(347, 21);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(2, 40);
            this.panel5.TabIndex = 30;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ESD_Testjig.Properties.Resources.Logo_small;
            this.pictureBox1.Location = new System.Drawing.Point(1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(10, 20, 0, 0);
            this.pictureBox1.Size = new System.Drawing.Size(335, 83);
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(356, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 21);
            this.label5.TabIndex = 28;
            this.label5.Text = "ESD Test jig Software V0.2";
            // 
            // lblUserNm
            // 
            this.lblUserNm.AutoSize = true;
            this.lblUserNm.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserNm.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblUserNm.Location = new System.Drawing.Point(1238, 48);
            this.lblUserNm.Name = "lblUserNm";
            this.lblUserNm.Size = new System.Drawing.Size(37, 18);
            this.lblUserNm.TabIndex = 27;
            this.lblUserNm.Text = "user";
            this.lblUserNm.Visible = false;
            // 
            // lblReportSignout
            // 
            this.lblReportSignout.AutoSize = true;
            this.lblReportSignout.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportSignout.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblReportSignout.Location = new System.Drawing.Point(1298, 48);
            this.lblReportSignout.Name = "lblReportSignout";
            this.lblReportSignout.Size = new System.Drawing.Size(0, 18);
            this.lblReportSignout.TabIndex = 22;
            this.lblReportSignout.Click += new System.EventHandler(this.LblReportSignout_Click);
            // 
            // pikboxClose
            // 
            this.pikboxClose.Image = global::ESD_Testjig.Properties.Resources.close;
            this.pikboxClose.Location = new System.Drawing.Point(1335, 4);
            this.pikboxClose.Name = "pikboxClose";
            this.pikboxClose.Size = new System.Drawing.Size(29, 29);
            this.pikboxClose.TabIndex = 20;
            this.pikboxClose.TabStop = false;
            this.pikboxClose.Click += new System.EventHandler(this.PikboxClose_Click);
            // 
            // lblSignOut
            // 
            this.lblSignOut.AutoSize = true;
            this.lblSignOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignOut.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblSignOut.Location = new System.Drawing.Point(1220, -19);
            this.lblSignOut.Name = "lblSignOut";
            this.lblSignOut.Size = new System.Drawing.Size(65, 18);
            this.lblSignOut.TabIndex = 11;
            this.lblSignOut.Text = "Sign Out";
            this.lblSignOut.Click += new System.EventHandler(this.LblSignOut_Click);
            // 
            // pikboxMinimize
            // 
            this.pikboxMinimize.Image = global::ESD_Testjig.Properties.Resources.minimize;
            this.pikboxMinimize.Location = new System.Drawing.Point(1307, 5);
            this.pikboxMinimize.Name = "pikboxMinimize";
            this.pikboxMinimize.Size = new System.Drawing.Size(27, 28);
            this.pikboxMinimize.TabIndex = 21;
            this.pikboxMinimize.TabStop = false;
            this.pikboxMinimize.Click += new System.EventHandler(this.PikboxMinimize_Click);
            // 
            // monthCalendar2
            // 
            this.monthCalendar2.Location = new System.Drawing.Point(12, 390);
            this.monthCalendar2.Name = "monthCalendar2";
            this.monthCalendar2.ShowTodayCircle = false;
            this.monthCalendar2.TabIndex = 22;
            this.monthCalendar2.Visible = false;
            this.monthCalendar2.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.MonthCalendar2_DateSelected);
            this.monthCalendar2.Leave += new System.EventHandler(this.MonthCalendar2_Leave);
            // 
            // btnShowReport
            // 
            this.btnShowReport.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnShowReport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowReport.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnShowReport.Location = new System.Drawing.Point(12, 493);
            this.btnShowReport.Name = "btnShowReport";
            this.btnShowReport.Size = new System.Drawing.Size(215, 33);
            this.btnShowReport.TabIndex = 6;
            this.btnShowReport.Text = "Show Report";
            this.btnShowReport.UseVisualStyleBackColor = false;
            this.btnShowReport.Click += new System.EventHandler(this.BtnShowReport_Click);
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Cursor = System.Windows.Forms.Cursors.Default;
            this.monthCalendar1.Location = new System.Drawing.Point(9, 331);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.ShowToday = false;
            this.monthCalendar1.ShowTodayCircle = false;
            this.monthCalendar1.TabIndex = 21;
            this.monthCalendar1.Visible = false;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.MonthCalendar1_DateSelected);
            this.monthCalendar1.Leave += new System.EventHandler(this.MonthCalendar1_Leave);
            // 
            // btnImportToExcel
            // 
            this.btnImportToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportToExcel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnImportToExcel.Location = new System.Drawing.Point(815, 86);
            this.btnImportToExcel.Name = "btnImportToExcel";
            this.btnImportToExcel.Size = new System.Drawing.Size(143, 33);
            this.btnImportToExcel.TabIndex = 15;
            this.btnImportToExcel.Text = "Export to Pdf";
            this.btnImportToExcel.UseVisualStyleBackColor = true;
            this.btnImportToExcel.Visible = false;
            this.btnImportToExcel.Click += new System.EventHandler(this.BtnImportToExcel_Click);
            // 
            // txtFromDate
            // 
            this.txtFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromDate.Location = new System.Drawing.Point(12, 313);
            this.txtFromDate.MaxLength = 10;
            this.txtFromDate.Multiline = true;
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Size = new System.Drawing.Size(215, 30);
            this.txtFromDate.TabIndex = 4;
            this.txtFromDate.Enter += new System.EventHandler(this.TxtFromDate_Enter);
            this.txtFromDate.Leave += new System.EventHandler(this.TxtFromDate_Leave);
            // 
            // txtToDate
            // 
            this.txtToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToDate.Location = new System.Drawing.Point(12, 372);
            this.txtToDate.Multiline = true;
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Size = new System.Drawing.Size(215, 30);
            this.txtToDate.TabIndex = 5;
            this.txtToDate.Enter += new System.EventHandler(this.TxtToDate_Enter);
            this.txtToDate.Leave += new System.EventHandler(this.TxtToDate_Leave);
            this.txtToDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TxtToDate_MouseUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 349);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 18);
            this.label10.TabIndex = 8;
            this.label10.Text = "To Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 289);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 18);
            this.label8.TabIndex = 6;
            this.label8.Text = "From Date";
            // 
            // cmbRepeateTest
            // 
            this.cmbRepeateTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRepeateTest.FormattingEnabled = true;
            this.cmbRepeateTest.ItemHeight = 18;
            this.cmbRepeateTest.Items.AddRange(new object[] {
            "--All--",
            "Yes",
            "No"});
            this.cmbRepeateTest.Location = new System.Drawing.Point(12, 257);
            this.cmbRepeateTest.Name = "cmbRepeateTest";
            this.cmbRepeateTest.Size = new System.Drawing.Size(215, 26);
            this.cmbRepeateTest.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Repeat Test";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.ItemHeight = 18;
            this.cmbStatus.Items.AddRange(new object[] {
            "--All--",
            "Pass",
            "Fail"});
            this.cmbStatus.Location = new System.Drawing.Point(12, 197);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(215, 26);
            this.cmbStatus.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Status";
            // 
            // cmbPcbType
            // 
            this.cmbPcbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPcbType.FormattingEnabled = true;
            this.cmbPcbType.ItemHeight = 18;
            this.cmbPcbType.Location = new System.Drawing.Point(12, 140);
            this.cmbPcbType.Name = "cmbPcbType";
            this.cmbPcbType.Size = new System.Drawing.Size(215, 26);
            this.cmbPcbType.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "PCB Type";
            // 
            // grdReport
            // 
            this.grdReport.AllowUserToAddRows = false;
            this.grdReport.AllowUserToDeleteRows = false;
            this.grdReport.AllowUserToResizeColumns = false;
            this.grdReport.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdReport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grdReport.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.grdReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.InfoText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdReport.ColumnHeadersHeight = 24;
            this.grdReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdReport.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdReport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdReport.EnableHeadersVisualStyles = false;
            this.grdReport.Location = new System.Drawing.Point(123, 156);
            this.grdReport.Name = "grdReport";
            this.grdReport.ReadOnly = true;
            this.grdReport.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LimeGreen;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdReport.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.grdReport.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdReport.RowTemplate.Height = 28;
            this.grdReport.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grdReport.Size = new System.Drawing.Size(946, 445);
            this.grdReport.TabIndex = 20;
            this.grdReport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdReport_CellContentClick);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.panel4.Controls.Add(this.cmbUser);
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.lbluser);
            this.panel4.Controls.Add(this.btnShowReport);
            this.panel4.Controls.Add(this.monthCalendar1);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.cmbPcbType);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.monthCalendar2);
            this.panel4.Controls.Add(this.cmbStatus);
            this.panel4.Controls.Add(this.txtToDate);
            this.panel4.Controls.Add(this.txtFromDate);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.cmbRepeateTest);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 81);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(245, 645);
            this.panel4.TabIndex = 21;
            // 
            // cmbUser
            // 
            this.cmbUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.ItemHeight = 18;
            this.cmbUser.Location = new System.Drawing.Point(12, 82);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(215, 26);
            this.cmbUser.TabIndex = 24;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(16)))), ((int)(((byte)(96)))));
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(245, 50);
            this.panel7.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(76, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "PCB Type";
            // 
            // lbluser
            // 
            this.lbluser.AutoSize = true;
            this.lbluser.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbluser.Location = new System.Drawing.Point(9, 57);
            this.lbluser.Name = "lbluser";
            this.lbluser.Size = new System.Drawing.Size(40, 18);
            this.lbluser.TabIndex = 23;
            this.lbluser.Text = "User";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblSearchgrid);
            this.panel1.Controls.Add(this.txtSerachGrid);
            this.panel1.Controls.Add(this.grdExcel);
            this.panel1.Controls.Add(this.lblsorting);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.lblNoRecords);
            this.panel1.Controls.Add(this.lblOf);
            this.panel1.Controls.Add(this.lblpages);
            this.panel1.Controls.Add(this.lblTotalpages);
            this.panel1.Controls.Add(this.btnNextPage);
            this.panel1.Controls.Add(this.btnPreviousPage);
            this.panel1.Controls.Add(this.lblReportTitle);
            this.panel1.Controls.Add(this.grdReport);
            this.panel1.Controls.Add(this.btnImportToExcel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(245, 81);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1125, 645);
            this.panel1.TabIndex = 22;
            // 
            // lblSearchgrid
            // 
            this.lblSearchgrid.AutoSize = true;
            this.lblSearchgrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchgrid.Location = new System.Drawing.Point(369, 133);
            this.lblSearchgrid.Name = "lblSearchgrid";
            this.lblSearchgrid.Size = new System.Drawing.Size(131, 16);
            this.lblSearchgrid.TabIndex = 33;
            this.lblSearchgrid.Text = "Search by Serial No.";
            this.lblSearchgrid.Visible = false;
            // 
            // txtSerachGrid
            // 
            this.txtSerachGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerachGrid.Location = new System.Drawing.Point(501, 126);
            this.txtSerachGrid.MaxLength = 11;
            this.txtSerachGrid.Name = "txtSerachGrid";
            this.txtSerachGrid.Size = new System.Drawing.Size(119, 24);
            this.txtSerachGrid.TabIndex = 32;
            this.txtSerachGrid.Visible = false;
            this.txtSerachGrid.TextChanged += new System.EventHandler(this.TxtSerachGrid_TextChanged);
            // 
            // grdExcel
            // 
            this.grdExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdExcel.Location = new System.Drawing.Point(5, 568);
            this.grdExcel.Name = "grdExcel";
            this.grdExcel.Size = new System.Drawing.Size(31, 18);
            this.grdExcel.TabIndex = 31;
            this.grdExcel.Visible = false;
            // 
            // lblsorting
            // 
            this.lblsorting.AutoSize = true;
            this.lblsorting.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsorting.Location = new System.Drawing.Point(115, 133);
            this.lblsorting.Name = "lblsorting";
            this.lblsorting.Size = new System.Drawing.Size(222, 16);
            this.lblsorting.TabIndex = 30;
            this.lblsorting.Text = "* Click on column header for sorting .";
            this.lblsorting.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(182)))), ((int)(((byte)(54)))));
            this.panel3.Controls.Add(this.lblReport);
            this.panel3.Controls.Add(this.btnUsbStatus);
            this.panel3.Controls.Add(this.lblTestcases);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1125, 50);
            this.panel3.TabIndex = 11;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // lblReport
            // 
            this.lblReport.AutoSize = true;
            this.lblReport.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReport.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblReport.Location = new System.Drawing.Point(141, 16);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(62, 18);
            this.lblReport.TabIndex = 24;
            this.lblReport.Text = "Report";
            // 
            // btnUsbStatus
            // 
            this.btnUsbStatus.BackColor = System.Drawing.Color.Silver;
            this.btnUsbStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUsbStatus.Enabled = false;
            this.btnUsbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUsbStatus.Location = new System.Drawing.Point(1031, 11);
            this.btnUsbStatus.Name = "btnUsbStatus";
            this.btnUsbStatus.Size = new System.Drawing.Size(63, 29);
            this.btnUsbStatus.TabIndex = 28;
            this.btnUsbStatus.Text = "USB";
            this.btnUsbStatus.UseVisualStyleBackColor = false;
            this.btnUsbStatus.Visible = false;
            // 
            // lblTestcases
            // 
            this.lblTestcases.AutoSize = true;
            this.lblTestcases.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestcases.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTestcases.Location = new System.Drawing.Point(15, 16);
            this.lblTestcases.Name = "lblTestcases";
            this.lblTestcases.Size = new System.Drawing.Size(97, 18);
            this.lblTestcases.TabIndex = 23;
            this.lblTestcases.Text = "Test Cases";
            this.lblTestcases.Click += new System.EventHandler(this.LblTestcases_Click);
            // 
            // lblNoRecords
            // 
            this.lblNoRecords.AutoSize = true;
            this.lblNoRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoRecords.ForeColor = System.Drawing.Color.Red;
            this.lblNoRecords.Location = new System.Drawing.Point(362, 269);
            this.lblNoRecords.Name = "lblNoRecords";
            this.lblNoRecords.Size = new System.Drawing.Size(143, 18);
            this.lblNoRecords.TabIndex = 29;
            this.lblNoRecords.Text = "Records not found...";
            this.lblNoRecords.Visible = false;
            // 
            // lblOf
            // 
            this.lblOf.AutoSize = true;
            this.lblOf.Location = new System.Drawing.Point(424, 548);
            this.lblOf.Name = "lblOf";
            this.lblOf.Size = new System.Drawing.Size(16, 13);
            this.lblOf.TabIndex = 28;
            this.lblOf.Text = "of";
            this.lblOf.Visible = false;
            // 
            // lblpages
            // 
            this.lblpages.AutoSize = true;
            this.lblpages.Location = new System.Drawing.Point(406, 549);
            this.lblpages.Name = "lblpages";
            this.lblpages.Size = new System.Drawing.Size(10, 13);
            this.lblpages.TabIndex = 27;
            this.lblpages.Text = ".";
            this.lblpages.Visible = false;
            // 
            // lblTotalpages
            // 
            this.lblTotalpages.AutoSize = true;
            this.lblTotalpages.Location = new System.Drawing.Point(439, 548);
            this.lblTotalpages.Name = "lblTotalpages";
            this.lblTotalpages.Size = new System.Drawing.Size(10, 13);
            this.lblTotalpages.TabIndex = 26;
            this.lblTotalpages.Text = ".";
            this.lblTotalpages.Visible = false;
            // 
            // btnNextPage
            // 
            this.btnNextPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextPage.Location = new System.Drawing.Point(459, 543);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(27, 23);
            this.btnNextPage.TabIndex = 25;
            this.btnNextPage.Text = ">";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Visible = false;
            this.btnNextPage.Click += new System.EventHandler(this.BtnNextPage_Click);
            // 
            // btnPreviousPage
            // 
            this.btnPreviousPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreviousPage.Location = new System.Drawing.Point(375, 545);
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.Size = new System.Drawing.Size(27, 23);
            this.btnPreviousPage.TabIndex = 24;
            this.btnPreviousPage.Text = "<";
            this.btnPreviousPage.UseVisualStyleBackColor = true;
            this.btnPreviousPage.Visible = false;
            this.btnPreviousPage.Click += new System.EventHandler(this.BtnPreviousPage_Click);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.AutoSize = true;
            this.lblReportTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.Location = new System.Drawing.Point(43, 94);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Size = new System.Drawing.Size(282, 29);
            this.lblReportTitle.TabIndex = 21;
            this.lblReportTitle.Text = "PCB Type Details Report";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pnlFooter.Location = new System.Drawing.Point(0, 726);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1370, 42);
            this.pnlFooter.TabIndex = 12;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ESD_Testjig.Properties.Resources.inner_bg;
            this.ClientSize = new System.Drawing.Size(1370, 768);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Report_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pikboxClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pikboxMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdReport)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExcel)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.ComboBox cmbPcbType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnImportToExcel;
        private System.Windows.Forms.Button btnShowReport;
        private System.Windows.Forms.TextBox txtFromDate;
        private System.Windows.Forms.TextBox txtToDate;
        private System.Windows.Forms.ComboBox cmbRepeateTest;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblSignOut;
        private System.Windows.Forms.PictureBox pikboxClose;
        private System.Windows.Forms.PictureBox pikboxMinimize;
        private System.Windows.Forms.MonthCalendar monthCalendar2;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.DataGridView grdReport;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblReportTitle;
        private System.Windows.Forms.Label lblpages;
        private System.Windows.Forms.Label lblTotalpages;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Label lblOf;
        private System.Windows.Forms.Label lblNoRecords;
        private System.Windows.Forms.Label lblsorting;
        private System.Windows.Forms.Label lblReportSignout;
        private System.Windows.Forms.DataGridView grdExcel;
        private System.Windows.Forms.Label lblUserNm;
        private System.Windows.Forms.ComboBox cmbUser;
        private System.Windows.Forms.Label lbluser;
        private System.Windows.Forms.Panel pnlFooter;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSerachGrid;
        private System.Windows.Forms.Label lblSearchgrid;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblTestcases;
        private System.Windows.Forms.Button btnUsbStatus;
        private System.Windows.Forms.Label lblReport;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label6;
    }
}
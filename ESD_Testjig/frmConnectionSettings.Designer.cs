namespace ESD_Testjig
{
    partial class frmConnectionSettings
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtPassword = new System.Windows.Forms.TextBox();
            this.TxtUserID = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.rbSQLServerAuth = new System.Windows.Forms.RadioButton();
            this.rbWindowAuth = new System.Windows.Forms.RadioButton();
            this.lblAuthMode = new System.Windows.Forms.Label();
            this.cmbServerList = new System.Windows.Forms.ComboBox();
            this.lblSelectServer = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbCentralized = new System.Windows.Forms.RadioButton();
            this.rbStandAlone = new System.Windows.Forms.RadioButton();
            this.tblPanal = new System.Windows.Forms.TableLayoutPanel();
            this.pnlAction = new System.Windows.Forms.Panel();
            this.btnTestCon = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.rdbNewDB = new System.Windows.Forms.RadioButton();
            this.rdbExistingDB = new System.Windows.Forms.RadioButton();
            this.cmbDatabases = new System.Windows.Forms.ComboBox();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.lblSelectDB = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tblPanal.SuspendLayout();
            this.pnlAction.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.TxtPassword);
            this.panel1.Controls.Add(this.TxtUserID);
            this.panel1.Controls.Add(this.lblPassword);
            this.panel1.Controls.Add(this.lblUser);
            this.panel1.Controls.Add(this.rbSQLServerAuth);
            this.panel1.Controls.Add(this.rbWindowAuth);
            this.panel1.Controls.Add(this.lblAuthMode);
            this.panel1.Controls.Add(this.cmbServerList);
            this.panel1.Controls.Add(this.lblSelectServer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(398, 202);
            this.panel1.TabIndex = 3;
            // 
            // TxtPassword
            // 
            this.TxtPassword.Location = new System.Drawing.Point(115, 177);
            this.TxtPassword.Name = "TxtPassword";
            this.TxtPassword.PasswordChar = '*';
            this.TxtPassword.Size = new System.Drawing.Size(256, 21);
            this.TxtPassword.TabIndex = 8;
            // 
            // TxtUserID
            // 
            this.TxtUserID.Location = new System.Drawing.Point(115, 141);
            this.TxtUserID.Name = "TxtUserID";
            this.TxtUserID.Size = new System.Drawing.Size(256, 21);
            this.TxtUserID.TabIndex = 7;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(3, 177);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(63, 15);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(3, 141);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(69, 15);
            this.lblUser.TabIndex = 5;
            this.lblUser.Text = "User name";
            // 
            // rbSQLServerAuth
            // 
            this.rbSQLServerAuth.AutoSize = true;
            this.rbSQLServerAuth.Location = new System.Drawing.Point(115, 104);
            this.rbSQLServerAuth.Name = "rbSQLServerAuth";
            this.rbSQLServerAuth.Size = new System.Drawing.Size(172, 19);
            this.rbSQLServerAuth.TabIndex = 6;
            this.rbSQLServerAuth.Text = "SQL Server Authentification";
            this.rbSQLServerAuth.UseVisualStyleBackColor = true;
            // 
            // rbWindowAuth
            // 
            this.rbWindowAuth.AutoSize = true;
            this.rbWindowAuth.Checked = true;
            this.rbWindowAuth.Location = new System.Drawing.Point(115, 79);
            this.rbWindowAuth.Name = "rbWindowAuth";
            this.rbWindowAuth.Size = new System.Drawing.Size(161, 19);
            this.rbWindowAuth.TabIndex = 5;
            this.rbWindowAuth.TabStop = true;
            this.rbWindowAuth.Text = "Windows Authentification";
            this.rbWindowAuth.UseVisualStyleBackColor = true;
            // 
            // lblAuthMode
            // 
            this.lblAuthMode.AutoSize = true;
            this.lblAuthMode.Location = new System.Drawing.Point(3, 61);
            this.lblAuthMode.Name = "lblAuthMode";
            this.lblAuthMode.Size = new System.Drawing.Size(123, 15);
            this.lblAuthMode.TabIndex = 2;
            this.lblAuthMode.Text = "Authentification Mode";
            // 
            // cmbServerList
            // 
            this.cmbServerList.FormattingEnabled = true;
            this.cmbServerList.Location = new System.Drawing.Point(115, 16);
            this.cmbServerList.Name = "cmbServerList";
            this.cmbServerList.Size = new System.Drawing.Size(256, 23);
            this.cmbServerList.TabIndex = 4;
            // 
            // lblSelectServer
            // 
            this.lblSelectServer.AutoSize = true;
            this.lblSelectServer.Location = new System.Drawing.Point(3, 19);
            this.lblSelectServer.Name = "lblSelectServer";
            this.lblSelectServer.Size = new System.Drawing.Size(78, 15);
            this.lblSelectServer.TabIndex = 0;
            this.lblSelectServer.Text = "Select server";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.rbCentralized);
            this.panel2.Controls.Add(this.rbStandAlone);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(398, 36);
            this.panel2.TabIndex = 0;
            // 
            // rbCentralized
            // 
            this.rbCentralized.AutoSize = true;
            this.rbCentralized.Location = new System.Drawing.Point(189, 10);
            this.rbCentralized.Name = "rbCentralized";
            this.rbCentralized.Size = new System.Drawing.Size(131, 19);
            this.rbCentralized.TabIndex = 2;
            this.rbCentralized.TabStop = true;
            this.rbCentralized.Text = "Centralized System";
            this.rbCentralized.UseVisualStyleBackColor = true;
            // 
            // rbStandAlone
            // 
            this.rbStandAlone.AutoSize = true;
            this.rbStandAlone.Location = new System.Drawing.Point(15, 10);
            this.rbStandAlone.Name = "rbStandAlone";
            this.rbStandAlone.Size = new System.Drawing.Size(134, 19);
            this.rbStandAlone.TabIndex = 1;
            this.rbStandAlone.TabStop = true;
            this.rbStandAlone.Text = "Stand Alone System";
            this.rbStandAlone.UseVisualStyleBackColor = true;
            // 
            // tblPanal
            // 
            this.tblPanal.BackColor = System.Drawing.Color.White;
            this.tblPanal.ColumnCount = 1;
            this.tblPanal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPanal.Controls.Add(this.pnlAction, 0, 3);
            this.tblPanal.Controls.Add(this.panel2, 0, 0);
            this.tblPanal.Controls.Add(this.panel1, 0, 1);
            this.tblPanal.Controls.Add(this.panel4, 0, 2);
            this.tblPanal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPanal.Location = new System.Drawing.Point(0, 0);
            this.tblPanal.Name = "tblPanal";
            this.tblPanal.RowCount = 4;
            this.tblPanal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.42262F));
            this.tblPanal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.18988F));
            this.tblPanal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.00726F));
            this.tblPanal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.38025F));
            this.tblPanal.Size = new System.Drawing.Size(404, 408);
            this.tblPanal.TabIndex = 1;
            // 
            // pnlAction
            // 
            this.pnlAction.BackColor = System.Drawing.Color.White;
            this.pnlAction.Controls.Add(this.btnTestCon);
            this.pnlAction.Controls.Add(this.btnClose);
            this.pnlAction.Controls.Add(this.btnSave);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAction.ImeMode = System.Windows.Forms.ImeMode.On;
            this.pnlAction.Location = new System.Drawing.Point(3, 363);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(398, 42);
            this.pnlAction.TabIndex = 2;
            // 
            // btnTestCon
            // 
            this.btnTestCon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(150)))), ((int)(((byte)(206)))));
            this.btnTestCon.FlatAppearance.BorderSize = 0;
            this.btnTestCon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestCon.ForeColor = System.Drawing.Color.White;
            this.btnTestCon.Location = new System.Drawing.Point(23, 6);
            this.btnTestCon.Name = "btnTestCon";
            this.btnTestCon.Size = new System.Drawing.Size(139, 30);
            this.btnTestCon.TabIndex = 9;
            this.btnTestCon.Text = "TEST CONNECTION";
            this.btnTestCon.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(151)))), ((int)(((byte)(151)))));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(268, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(150)))), ((int)(((byte)(206)))));
            this.btnSave.Enabled = false;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(167, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.cmbDatabases);
            this.panel4.Controls.Add(this.txtDatabaseName);
            this.panel4.Controls.Add(this.lblSelectDB);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 253);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(398, 104);
            this.panel4.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Location = new System.Drawing.Point(-3, 364);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(401, 51);
            this.panel5.TabIndex = 15;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.rdbNewDB);
            this.panel6.Controls.Add(this.rdbExistingDB);
            this.panel6.Location = new System.Drawing.Point(115, 7);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(279, 42);
            this.panel6.TabIndex = 14;
            // 
            // rdbNewDB
            // 
            this.rdbNewDB.AutoSize = true;
            this.rdbNewDB.Checked = true;
            this.rdbNewDB.Location = new System.Drawing.Point(3, 9);
            this.rdbNewDB.Name = "rdbNewDB";
            this.rdbNewDB.Size = new System.Drawing.Size(119, 19);
            this.rdbNewDB.TabIndex = 11;
            this.rdbNewDB.TabStop = true;
            this.rdbNewDB.Text = "Create Database";
            this.rdbNewDB.UseVisualStyleBackColor = true;
            // 
            // rdbExistingDB
            // 
            this.rdbExistingDB.AutoSize = true;
            this.rdbExistingDB.Location = new System.Drawing.Point(137, 9);
            this.rdbExistingDB.Name = "rdbExistingDB";
            this.rdbExistingDB.Size = new System.Drawing.Size(125, 19);
            this.rdbExistingDB.TabIndex = 11;
            this.rdbExistingDB.Text = "Existing Database";
            this.rdbExistingDB.UseVisualStyleBackColor = true;
            // 
            // cmbDatabases
            // 
            this.cmbDatabases.FormattingEnabled = true;
            this.cmbDatabases.Location = new System.Drawing.Point(115, 57);
            this.cmbDatabases.Name = "cmbDatabases";
            this.cmbDatabases.Size = new System.Drawing.Size(256, 23);
            this.cmbDatabases.TabIndex = 13;
            this.cmbDatabases.Visible = false;
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.Location = new System.Drawing.Point(115, 57);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(256, 21);
            this.txtDatabaseName.TabIndex = 12;
            // 
            // lblSelectDB
            // 
            this.lblSelectDB.AutoSize = true;
            this.lblSelectDB.Location = new System.Drawing.Point(3, 61);
            this.lblSelectDB.Name = "lblSelectDB";
            this.lblSelectDB.Size = new System.Drawing.Size(73, 15);
            this.lblSelectDB.TabIndex = 9;
            this.lblSelectDB.Text = "Enter Name";
            // 
            // frmConnectionSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(404, 408);
            this.Controls.Add(this.tblPanal);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmConnectionSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CONNECTION SETTING";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tblPanal.ResumeLayout(false);
            this.pnlAction.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TxtPassword;
        private System.Windows.Forms.TextBox TxtUserID;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.RadioButton rbSQLServerAuth;
        private System.Windows.Forms.RadioButton rbWindowAuth;
        private System.Windows.Forms.Label lblAuthMode;
        private System.Windows.Forms.ComboBox cmbServerList;
        private System.Windows.Forms.Label lblSelectServer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbCentralized;
        private System.Windows.Forms.RadioButton rbStandAlone;
        private System.Windows.Forms.TableLayoutPanel tblPanal;
        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Button btnTestCon;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton rdbNewDB;
        private System.Windows.Forms.RadioButton rdbExistingDB;
        private System.Windows.Forms.ComboBox cmbDatabases;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.Label lblSelectDB;
    }
}
namespace ESD_Testjig
{
    partial class AddUser
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.CmbRole = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Txtemail = new System.Windows.Forms.TextBox();
            this.TxtMobileNo = new System.Windows.Forms.TextBox();
            this.Btncancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblUserNm = new System.Windows.Forms.Label();
            this.lblReportSignout = new System.Windows.Forms.Label();
            this.pikboxClose = new System.Windows.Forms.PictureBox();
            this.lblSignOut = new System.Windows.Forms.Label();
            this.pikboxMinimize = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pikboxClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pikboxMinimize)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(182)))), ((int)(((byte)(54)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.CmbRole);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.Txtemail);
            this.panel1.Controls.Add(this.TxtMobileNo);
            this.panel1.Controls.Add(this.Btncancel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.txtUserName);
            this.panel1.Location = new System.Drawing.Point(478, 205);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(453, 403);
            this.panel1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(44, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "UserRole*";
            // 
            // CmbRole
            // 
            this.CmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbRole.FormattingEnabled = true;
            this.CmbRole.Location = new System.Drawing.Point(142, 248);
            this.CmbRole.Name = "CmbRole";
            this.CmbRole.Size = new System.Drawing.Size(243, 28);
            this.CmbRole.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(44, 209);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 11;
            this.label1.Text = "Email";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(44, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "Mobile No*";
            // 
            // Txtemail
            // 
            this.Txtemail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txtemail.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.Txtemail.Location = new System.Drawing.Point(142, 205);
            this.Txtemail.Name = "Txtemail";
            this.Txtemail.Size = new System.Drawing.Size(243, 26);
            this.Txtemail.TabIndex = 4;
            this.Txtemail.Validating += new System.ComponentModel.CancelEventHandler(this.Txtemail_Validating);
            // 
            // TxtMobileNo
            // 
            this.TxtMobileNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtMobileNo.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.TxtMobileNo.Location = new System.Drawing.Point(142, 164);
            this.TxtMobileNo.MaxLength = 10;
            this.TxtMobileNo.Name = "TxtMobileNo";
            this.TxtMobileNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TxtMobileNo.Size = new System.Drawing.Size(243, 26);
            this.TxtMobileNo.TabIndex = 3;
            this.TxtMobileNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtMobileNo_KeyPress);
            this.TxtMobileNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMobileNo_Validating);
            // 
            // Btncancel
            // 
            this.Btncancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(36)))));
            this.Btncancel.FlatAppearance.BorderSize = 0;
            this.Btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btncancel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btncancel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Btncancel.Location = new System.Drawing.Point(231, 309);
            this.Btncancel.Name = "Btncancel";
            this.Btncancel.Size = new System.Drawing.Size(159, 40);
            this.Btncancel.TabIndex = 7;
            this.Btncancel.Text = "Cancel";
            this.Btncancel.UseVisualStyleBackColor = false;
            this.Btncancel.Click += new System.EventHandler(this.Btncancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(44, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(44, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Username*";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(36)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAdd.Location = new System.Drawing.Point(55, 309);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(164, 40);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtPassword.Location = new System.Drawing.Point(142, 126);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(243, 26);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtPassword_Validating);
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtUserName.Location = new System.Drawing.Point(142, 84);
            this.txtUserName.MaxLength = 12;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtUserName.Size = new System.Drawing.Size(243, 26);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserName_Validating);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlText;
            this.pnlHeader.Controls.Add(this.panel5);
            this.pnlHeader.Controls.Add(this.pictureBox2);
            this.pnlHeader.Controls.Add(this.label6);
            this.pnlHeader.Controls.Add(this.lblUserNm);
            this.pnlHeader.Controls.Add(this.lblReportSignout);
            this.pnlHeader.Controls.Add(this.pikboxClose);
            this.pnlHeader.Controls.Add(this.lblSignOut);
            this.pnlHeader.Controls.Add(this.pikboxMinimize);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1386, 81);
            this.pnlHeader.TabIndex = 10;
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
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ESD_Testjig.Properties.Resources.Logo_small;
            this.pictureBox2.Location = new System.Drawing.Point(1, -1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Padding = new System.Windows.Forms.Padding(10, 20, 0, 0);
            this.pictureBox2.Size = new System.Drawing.Size(335, 83);
            this.pictureBox2.TabIndex = 29;
            this.pictureBox2.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(356, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(190, 21);
            this.label6.TabIndex = 28;
            this.label6.Text = "ESD Test jig Software V0.2";
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
            // 
            // pikboxClose
            // 
            this.pikboxClose.Image = global::ESD_Testjig.Properties.Resources.close;
            this.pikboxClose.Location = new System.Drawing.Point(1335, 4);
            this.pikboxClose.Name = "pikboxClose";
            this.pikboxClose.Size = new System.Drawing.Size(29, 29);
            this.pikboxClose.TabIndex = 20;
            this.pikboxClose.TabStop = false;
            this.pikboxClose.Click += new System.EventHandler(this.pikboxClose_Click);
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
            // 
            // pikboxMinimize
            // 
            this.pikboxMinimize.Image = global::ESD_Testjig.Properties.Resources.minimize;
            this.pikboxMinimize.Location = new System.Drawing.Point(1307, 5);
            this.pikboxMinimize.Name = "pikboxMinimize";
            this.pikboxMinimize.Size = new System.Drawing.Size(27, 28);
            this.pikboxMinimize.TabIndex = 21;
            this.pikboxMinimize.TabStop = false;
            this.pikboxMinimize.Click += new System.EventHandler(this.pikboxMinimize_Click);
            // 
            // AddUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::ESD_Testjig.Properties.Resources.inner_bg;
            this.ClientSize = new System.Drawing.Size(1386, 788);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddUser";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pikboxClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pikboxMinimize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Txtemail;
        private System.Windows.Forms.TextBox TxtMobileNo;
        private System.Windows.Forms.Button Btncancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CmbRole;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblUserNm;
        private System.Windows.Forms.Label lblReportSignout;
        private System.Windows.Forms.PictureBox pikboxClose;
        private System.Windows.Forms.Label lblSignOut;
        private System.Windows.Forms.PictureBox pikboxMinimize;
    }
}
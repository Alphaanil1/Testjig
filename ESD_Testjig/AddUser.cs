using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESD_Testjig
{
    public partial class AddUser : Form
    {
        LogFile log = new LogFile();
        MyMessageBox _msgbox = new MyMessageBox();
        ESD_TestjigProperties _objDal = new ESD_TestjigProperties();
        int LoginUserId;
        string Username = string.Empty;
        public AddUser(int UserId, string UserName)
        {
            InitializeComponent();
            LoginUserId = UserId;
            Username = UserName;
            DataTable dtUserRole = _objDal.GetUserRole();
            CmbRole.DataSource = dtUserRole;
            CmbRole.DisplayMember = "Role";
            CmbRole.ValueMember = "RoleID";
            lblSignOut.Text = Username;

            const int margin = 0;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
            Screen.PrimaryScreen.WorkingArea.X + margin,
            Screen.PrimaryScreen.WorkingArea.Y + margin,
            Screen.PrimaryScreen.WorkingArea.Width,
            Screen.PrimaryScreen.WorkingArea.Height);
            this.Bounds = rect;

            pikboxClose.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 40, 4);
            pikboxMinimize.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 70, 5);
            btnAdd.MouseEnter += BtnAdd_MouseEnter;
            Btncancel.Cursor = Cursors.Hand;
            USBCommunication.showProductSelection = false;
        }

        private void BtnAdd_MouseEnter(object sender, EventArgs e)
        {
            btnAdd.Cursor = Cursors.Hand;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                    if (string.IsNullOrWhiteSpace(txtUserName.Text))
                    {                       
                        txtUserName.Focus();
                        errorProvider1.SetError(txtUserName, "User name should not be empty!");
                    }
                   else if(string.IsNullOrWhiteSpace(TxtMobileNo.Text) || TxtMobileNo.Text.Length < 10)
                    {
                        TxtMobileNo.Focus();
                        errorProvider1.SetError(TxtMobileNo, "Mobile number should not be empty!");
                    }
                   else if(string.IsNullOrWhiteSpace(txtPassword.Text))
                    {                      
                        errorProvider1.SetError(txtPassword, "Password should not be empty!");
                    }
                    else if (txtPassword.Text.Length < 6)
                    {
                    errorProvider1.SetError(txtPassword, "Password must be at least 6 characters long");
                    }
                     else
                    {
                    int Status = _objDal.AddUSer(txtUserName.Text, txtPassword.Text, (int)CmbRole.SelectedValue, LoginUserId, Txtemail.Text, TxtMobileNo.Text);

                    if (Status == 2)
                    {
                        _msgbox.ShowBox("User already exist.");
                    }
                    else if (Status > 0)
                    {
                        _msgbox.ShowBox("User added successfully.");   
                        clearControls();
                    }
                    else
                    {
                        _msgbox.ShowBox("Failed to add user.");
                    }
                }                
            }
            catch (Exception ex)
            {
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message);
            }
        }

        private void clearControls()
        {
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            TxtMobileNo.Text = string.Empty;
            Txtemail.Text = string.Empty;
        }

        private void Btncancel_Click(object sender, EventArgs e)
        {       
            PCBTest frmpcbtest = new PCBTest(LoginUserId, Username);
            frmpcbtest.Show();
            this.Hide();
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
               // e.Cancel = true;
                //txtUserName.Focus();
                errorProvider1.SetError(txtUserName, "User name should not be empty!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
            }
            CmbRole.Enabled = true;
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                //e.Cancel = true;
                //txtPassword.Focus();
                errorProvider1.SetError(txtPassword, "Password should not be empty!");
            }
            else if (txtPassword.Text.Length < 6)
            {
                errorProvider1.SetError(txtPassword, "Password must be at least 6 characters long");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void pikboxClose_Click(object sender, EventArgs e)
        {           
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void pikboxMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void TxtMobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                e.Handled = true;
                _msgbox.ShowBox("Please enter correct number");    
                }          
                if (char.IsDigit(e.KeyChar))
                {
                    e.Handled = false;
                    return;
                }
                if (char.IsLetter(e.KeyChar))
                {
                    e.Handled = true;
                _msgbox.ShowBox("Please enter correct number");
                }            
        }
       

        private void Txtemail_Validating(object sender, CancelEventArgs e)
        {
            string email = Txtemail.Text.Trim();

            // Define the regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Check if the entered text matches the email pattern
            if (!Regex.IsMatch(email, pattern))
            {
                errorProvider1.SetError(Txtemail, "Please enter a valid email address");
            }
            else
                e.Cancel = false;
        }

        private void TxtMobileNo_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(TxtMobileNo.Text))
            {
                //e.Cancel = true;
                //TxtMobileNo.Focus();
                errorProvider1.SetError(TxtMobileNo, "Mobile No. should not be empty!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(TxtMobileNo, "");
            }
        }
    }
    }

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ESD_Testjig;

namespace ESD_Testjig
{
    public partial class Login : Form
    {
        ESD_TestjigProperties _objDal = new ESD_TestjigProperties();       
        LogFile log = new LogFile();
        MyMessageBox _msgbox = new MyMessageBox();
        public Login()
        {
            InitializeComponent();
            btnLogin.Cursor = Cursors.Hand;
            DBAccess.ConnectionString = GlobalInformation.ConnectionString;
            DBAccess.ConnectionStringMaster = GlobalInformation.ConnectionStringMaster;
        }
        private void Form_load(object sender, EventArgs e)
        {
            //For clear testboxes
            txtUserName.Text = "";
            txtPassword.Text = "";   
            //For login form,close button and minimize button location on any resolution PC.      
            const int margin = 0;
            Rectangle rect = new Rectangle(
                Screen.PrimaryScreen.WorkingArea.X + margin,
                Screen.PrimaryScreen.WorkingArea.Y + margin,
                Screen.PrimaryScreen.WorkingArea.Width ,
                Screen.PrimaryScreen.WorkingArea.Height );
            this.Bounds = rect;
            pikboxClose.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 40, 4);
            pikboxMinimize.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 70, 5);          
        }      
        private void pikboxMinimize_Click(object sender, EventArgs e)
        {
            //Minimize login form
            this.WindowState = FormWindowState.Minimized;
        }
        private void pikboxClose_Click(object sender, EventArgs e)
        {
           //Close login form
           Application.Exit();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
            DataTable dtlogin = _objDal.Login(txtUserName.Text, txtPassword.Text);
            if (dtlogin.Rows.Count > 0)
            {
                int Userid;
                Userid = Convert.ToInt32(dtlogin.Rows[0]["UserID"].ToString());
                string username = dtlogin.Rows[0]["UserName"].ToString();
                    USBCommunication.showProductSelection = true;
                    GlobalInformation.ProductTypeID = 0;
                    PCBTest pcbtestpage = new PCBTest(Userid, username);
                    pcbtestpage.Show();
                    this.Hide();
                   
                   // this.Close();
            }
            else
            {
                    _msgbox.ShowBox("Please enter correct username and password","longmsg");
            }
            }
            catch (Exception ex)
            {               
                log.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show(ex.Message); 
            }
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            //Press Enter in password textbox.
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }
    }
}

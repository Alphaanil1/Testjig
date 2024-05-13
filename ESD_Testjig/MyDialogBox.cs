using System;
using System.Windows.Forms;

namespace ESD_Testjig
{
    public partial class MyDialogBox : Form
    {
        MyDialogBox newMessageBox;     
        static string msg;
        public MyDialogBox()
        {
            InitializeComponent();
        }

        public  string ShowBox()
        {           
            newMessageBox = new MyDialogBox();
            newMessageBox.lbllongmsg.Text = "Do you want to continue test?";
            newMessageBox.ShowDialog();
            return msg;
        }      

        public string ShowMsg()
        {           
            newMessageBox = new MyDialogBox();
            newMessageBox.lbllongmsg.Text = "Please, Dropdown battery voltage";
            newMessageBox.ShowDialog();           
            return msg;
        }
        //For Yes
        private void btnOK_Click_1(object sender, EventArgs e)
        {
            msg = "Yes";
            this.Close();           
        }

        //For No button
        private void button1_Click(object sender, EventArgs e)
        {
            msg = "No";
            this.Close();                  
        }
        
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ESD_Testjig
{
    public partial class PleaseWaitMsg : Form
    {
        static PleaseWaitMsg newMessageBox;
        public PleaseWaitMsg()
        {
            InitializeComponent();
            //timer start
            timer1.Start();
        }
        public static void ShowBox()
        {
            newMessageBox = new PleaseWaitMsg();
            newMessageBox.lblloading.Text = "Auto test is in progress,Please wait...";
            newMessageBox.ShowDialog();
        }
                
        // Loading messages for auto test cases
        public static void ShowBox(string testname)
        {           
            newMessageBox = new PleaseWaitMsg(); 
            newMessageBox.lblloading.Text = testname +" test is in progress,Please wait...";
            newMessageBox.SetMessage(newMessageBox.lblloading.Text);
            newMessageBox.ShowDialog();
        }

        public static void ShowMSg()
        {
            newMessageBox = new PleaseWaitMsg();
            newMessageBox.lblloading.Text = "Please wait...";
            newMessageBox.ShowDialog();
        }
        public static void ShowDBMSg()
        {
            newMessageBox = new PleaseWaitMsg();
            newMessageBox.lblloading.Text = "Generating database, please wait for few minutes";
            newMessageBox.SetMessage("Generating database, please wait for few minutes");
            newMessageBox.ShowDialog();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Close loading meassage after 1 sec
            this.Close();
        }

        public void SetMessage(string message)
        {
            lblloading.Text = message;

            // Calculate center position
            int x = (ClientSize.Width - lblloading.Width) / 2;
            int y = (ClientSize.Height - lblloading.Height) / 2;

            // Set label position
            lblloading.Location = new Point(x, y);
        }
    }
}

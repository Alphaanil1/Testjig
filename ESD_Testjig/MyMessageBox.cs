using System;
using System.Drawing;
using System.Windows.Forms;

namespace ESD_Testjig
{
    public partial class MyMessageBox : Form
    {
        MyMessageBox newMessageBox;
        static string msg;
        public MyMessageBox()
        {
            InitializeComponent();
        }


        //For short message
        public  string ShowBox(string txtMessage)
        {
            newMessageBox = new MyMessageBox();
            newMessageBox.lbllongmsg.Visible = false;
            newMessageBox.lblmsgbox.Visible = true;
            newMessageBox.lblMSG.Visible = false;
            newMessageBox.lblmsgbox.Text = txtMessage;
            newMessageBox.ShowDialog();
            return msg;
        }
        // For long message
        public  string ShowBox(string txtMessage,string lenght)
        {
            //newMessageBox = new MyMessageBox();
            //newMessageBox.lblmsgbox.Visible = false;
            //newMessageBox.lblMSG.Visible = false;
            //newMessageBox.lbllongmsg.Visible = true;
            //newMessageBox.lbllongmsg.AutoSize = false; 
            //newMessageBox.lbllongmsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //newMessageBox.lbllongmsg.Text = txtMessage;
            //newMessageBox.SetMessage(txtMessage);
            //newMessageBox.ShowDialog();
            //return msg;

            newMessageBox = new MyMessageBox();
            newMessageBox.lbllongmsg.Visible = true;
            newMessageBox.lblmsgbox.Visible = false;
            newMessageBox.lblMSG.Visible = false;
            newMessageBox.lbllongmsg.Text = txtMessage;
            newMessageBox.lbllongmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            newMessageBox.SetMessage(txtMessage);
            newMessageBox.ShowDialog();
            return msg;
        }

        public void SetMessage(string message)
        {
            lbllongmsg.Text = message;

            // Calculate center position
            int x = (ClientSize.Width - lbllongmsg.Width) / 2;
            int y = (ClientSize.Height - lbllongmsg.Height) / 2;

            // Set label position
            lbllongmsg.Location = new Point(x, y);
        }

        public string ShowMSg()
        {
            newMessageBox = new MyMessageBox();           
            newMessageBox.lblmsgbox.Visible = false;
            newMessageBox.lblMSG.Text = "Set battery voltage between 4.40V to 4.80V";
            newMessageBox.lbllongmsg.Text = "and then click on OK.";
            newMessageBox.ShowDialog();
            return msg;
        }


        public string ShowRFIDMsg()
        {
            newMessageBox = new MyMessageBox();
            newMessageBox.lblmsgbox.Visible = false;
            newMessageBox.lblMSG.Text = "Keep RFID card in front of encoder.";           
            newMessageBox.ShowDialog();
            return msg;
        }
        //For ok button
        private void btnOK_Click(object sender, EventArgs e)
        {
            msg = "Yes";
            this.Close();
            //newMessageBox.Dispose();
        }
       public  void  CloseMsgBox()
        {
            newMessageBox.Opacity = 0;           
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            msg = "No";
            this.Close();
        }
        public string ShowMsgVBATReadTest()
        {
            newMessageBox = new MyMessageBox();
            newMessageBox.btnCancel.Visible = false;
            newMessageBox.lblMSG.Visible = false;
            newMessageBox.btnOK.Location = new System.Drawing.Point(242, 106);
            newMessageBox.lbllongmsg.Text = "Power off the test jig and then power on";
            newMessageBox.ShowDialog();
            return msg;
        }
      }
}

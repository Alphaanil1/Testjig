using System;
using System.Windows.Forms;

namespace ESD_Testjig
{
    public partial class SerialNoMsgBox : Form
    {
        static SerialNoMsgBox newMessageBox;
        readonly static string msg = string.Empty;
      
        public SerialNoMsgBox()
        {
            InitializeComponent();
        }
        //Show Serial number validation meassage
        public static string ShowBox()
        {
            newMessageBox = new SerialNoMsgBox();         
            newMessageBox.ShowDialog();
            return msg;

        }      
        private void BtnOK_Click_1(object sender, EventArgs e)
        {           
            //Close the msgbox
            newMessageBox.Dispose();
        }                   
    }
}

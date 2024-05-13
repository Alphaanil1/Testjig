
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESD_Testjig
{
    public partial class frmMessageForm : Form
    {
        private readonly string StrOK = "OK";
        private readonly string StrHotelLock = "Testjig";
        public frmMessageForm(string message,bool IsError)
        {
            InitializeComponent();

            this.ControlBox = false;
            //if (GlobalInformation.ResourceManager != null)
            //    btnOK.Text = "OK";      //GlobalInformation.ResourceManager.GetString("LBL_OK", GlobalInformation.MachinCulture);
            //else
            //    btnOK.Text = StrOK;

            lblMessage.Text = message;
            pic_info.Visible = !IsError;
            pic_alert.Visible = IsError;
            btnOK.Click += BtnOK_Click;

           // SetLabelText();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        
        public void SetLabelText()
        {
            if (GlobalInformation.ResourceManager != null)
            {
                CommonFunction.ControlSetting(this);
                this.Text = GlobalInformation.ResourceManager.GetString("GBL_PROJECTNAME", GlobalInformation.MachinCulture);
                btnOK.Text = GlobalInformation.ResourceManager.GetString("LBL_OK", GlobalInformation.MachinCulture);
            }
            else
            {
                this.Text = StrHotelLock;
                btnOK.Text = StrOK;
            }
        }

    }
}

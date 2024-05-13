using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESD_Testjig
{
   public class CommonFunction
    {
        public static void DisplayMessage(string _message, bool IsAlert = false)
        {
            using (frmMessageForm _messageForm = new frmMessageForm(_message, IsAlert))
            {
                _messageForm.ShowDialog();
            }
        }

        public static void ControlSetting(Form frm)
        {
            foreach (Control ctrl in frm.Controls)
            {
                ChangeLabelSize(ctrl);
            }
        }

        public static void ChangeLabelSize(Control plnctrl)
        {
            ContentAlignment alignMent = ContentAlignment.MiddleCenter;

            if (GlobalInformation.MachinCulture.Name != "en-US")
                alignMent = ContentAlignment.BottomCenter;


            if (plnctrl.HasChildren)
            {
                foreach (Control cctrl in plnctrl.Controls)
                {
                    if (cctrl is Label)
                        ((Label)cctrl).TextAlign = alignMent;
                    else if (cctrl is Button)
                        ((Button)cctrl).TextAlign = alignMent;
                    else if (cctrl is LinkLabel)
                        ((LinkLabel)cctrl).TextAlign = alignMent;
                    else if (cctrl is CheckBox)
                        ((CheckBox)cctrl).TextAlign = alignMent;
                    else if (cctrl is RadioButton)
                        ((RadioButton)cctrl).TextAlign = alignMent;
                    else if ((cctrl is TabControl || cctrl is Panel || cctrl is TabPage) && cctrl.HasChildren)
                        ChangeLabelSize(cctrl);
                }
            }
            if (plnctrl is Label)
                ((Label)plnctrl).TextAlign = alignMent;
            else if (plnctrl is Button)
                ((Button)plnctrl).TextAlign = alignMent;
            else if (plnctrl is LinkLabel)
                ((LinkLabel)plnctrl).TextAlign = alignMent;
            else if (plnctrl is CheckBox)
                ((CheckBox)plnctrl).TextAlign = alignMent;
            else if (plnctrl is RadioButton)
                ((RadioButton)plnctrl).TextAlign = alignMent;
        }

        public static DataSet GetList(string strQueryToGetList)
        {
            SqlCommand com = new SqlCommand();//strQueryToGetList, con
            SqlConnection con = ConnectionManager.SetConnection();
            DataSet ds = new DataSet();
            try
            {
                // Read specific values in the table.                    
                using (SqlDataAdapter da = new SqlDataAdapter(com))
                {
                    com.CommandText = strQueryToGetList;
                    com.CommandTimeout = 0;
                    com.Connection = con;
                    da.Fill(ds);
                    return ds;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                com.Dispose();
                con.Dispose();
            }
        }
    }
}

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
    public partial class frmProductType : Form
    {
        readonly LogFile log1 = new LogFile();
        readonly ESD_TestjigProperties _objDal = new ESD_TestjigProperties();
        public static bool frmProductStatus = true;
        public event EventHandler OKButtonClicked;
        private PCBTest parentForm;
        public frmProductType(PCBTest parent)
        {
            InitializeComponent();
            parentForm = parent;
            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            frmProductStatus = false;
            this.Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            GlobalInformation.ProductTypeID = Convert.ToInt32(cmbProductTypes.SelectedValue);
            frmProductStatus = true;
            //OKButtonClicked?.Invoke(this, EventArgs.Empty);
            parentForm.BindData();
            this.Close();   
            
        }

        private void frmProductType_Load(object sender, EventArgs e)
        {
            BindProductTypes();
        }

        private void BindProductTypes()
        {
            try
            {
                DataTable dtProducts = _objDal.GetProductTypes();
                cmbProductTypes.DisplayMember = "ProductType";
                cmbProductTypes.ValueMember = "ProductTypeID";               
                cmbProductTypes.DataSource = dtProducts;
            }
            catch (Exception ex)
            {
                log1.CreateLog(ex.Message, ex.ToString(), this.FindForm().Name);
                MessageBox.Show("Exception :" + ex.Message);
            }
        }

        private void cmbProductTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
           // GlobalInformation.ProductTypeID = Convert.ToInt32(cmbProductTypes.SelectedValue);
        }
    }
}

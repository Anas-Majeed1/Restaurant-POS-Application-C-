using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace RMS.Model
{
    public partial class frmAddCustomer : Form
    {
        public frmAddCustomer()
        {
            InitializeComponent();
        }

        public string orderType="";
        public int driverID = 0;
        public string cusName = "";
        public int mainID = 0;

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void frmAddCustomer_Load(object sender, EventArgs e)
        {
            if (orderType== "Take Away")
            {
                lblDriver.Visible = false;
                cbDriver.Visible = false;
            }
            string qry = "Select staffID 'id', sName 'name' from Staff where sRole ='Driver'";
            MainClass.CBFill(qry, cbDriver);
            if (mainID>0)
            {
                cbDriver.SelectedValue = driverID;
            }

        }

        private void lblDriver_Click(object sender, EventArgs e)
        {

        }

        private void cbDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            driverID = Convert.ToInt32(cbDriver.SelectedIndex);
        }
    }
}

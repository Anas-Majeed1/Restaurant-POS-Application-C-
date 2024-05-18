using RMS.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMS.Reports
{
    public partial class frmSaleByCategory : Form
    {
        public frmSaleByCategory()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            string qry = @"Select * from tblMain m
                         inner join tblDetails d on m.MainID = d.MainID
                         inner join products p on p.pID = d.proID
                         where m.aDate between @sdate  and @edate ";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            cmd.Parameters.AddWithValue("@sdate", Convert.ToDateTime(dateTimePicker1.Value).Date);
            cmd.Parameters.AddWithValue("edate", Convert.ToDateTime(dateTimePicker2.Value).Date);
            MainClass.con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            MainClass.con.Close();
            frmPrint frm = new frmPrint();

            rptSaleByCategory cr = new rptSaleByCategory();
            cr.SetDataSource(dt);
            frm.crystalReportViewer1.ReportSource = cr;
            frm.crystalReportViewer1.Refresh();
            Show();

        }

        private void frmSaleByCategory_Load(object sender, EventArgs e)
        {

        }
    }
}

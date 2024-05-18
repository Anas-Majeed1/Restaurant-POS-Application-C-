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


namespace RMS.View
{
    public partial class frmReports : Form
    {
        public frmReports()
        {
            InitializeComponent();
        }

       

        

        private void btnStaff_Click(object sender, EventArgs e)
        {
            string qry = @"Select * from Staff  ";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            MainClass.con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            MainClass.con.Close();
            frmPrint frm = new frmPrint();
            rptStaffList cr = new rptStaffList();
            cr.SetDatabaseLogon("sa", "123");
            cr.SetDataSource(dt);
            frm.crystalReportViewer1.ReportSource = cr;
            frm.crystalReportViewer1.Refresh();
            Show();
        }

        private void btnSaleCat_Click(object sender, EventArgs e)
        {
            frmSaleByCategory frm = new frmSaleByCategory();
            frm.ShowDialog();

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            string qry = @"Select * from products  ";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            MainClass.con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            MainClass.con.Close();
            frmPrint frm = new frmPrint();
           // rptMenu cr = new rptMenu();
            // cr.SetDatabaseLogon("sa", "123");
           // cr.SetDataSource(dt);
            //frm.crystalReportViewer1.ReportSource = cr;
            // frm.crystalReportViewer1.Refresh();
            Show();
        }
    }
}

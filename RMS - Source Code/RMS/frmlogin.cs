using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMS
{
    public partial class frmlogin : Form
    {
        public frmlogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(MainClass.IsValidUser(txtUser.Text, txtPass.Text) == false)
            {
                bxerror.Show("Invalid Username or Password");
                txtUser.Clear();
                txtPass.Clear();
                return;
            }
            else
            {
                this.Hide();
                frmMain main = new frmMain();
                main.Show();
            }
        }

        private void frmlogin_Load_1(object sender, EventArgs e)
        {

        }
    }
}

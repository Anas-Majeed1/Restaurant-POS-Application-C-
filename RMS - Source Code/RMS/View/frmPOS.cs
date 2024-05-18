using RMS.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RMS.View
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }
        public int MainID = 0;
        public string OrderType = "";
        public int driverID = 0;
        public string customerName = "";
        public string customerPhone = "";
        private void guna2TileButton6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnWaiter_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTable_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;

            AddCategory();

            ProductPanel.Controls.Clear();
            loadProduct();
        }

        private void AddCategory()
        {
            string qry = "SELECT * FROM category";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryPanel.Controls.Clear();

            Guna.UI2.WinForms.Guna2Button b2 = new Guna.UI2.WinForms.Guna2Button();
            b2.FillColor = Color.FromArgb(50, 55, 89);
            b2.Size = new Size(134, 45);
            b2.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            b2.Text = "All Categories";
            b2.CheckedState.FillColor = Color.FromArgb(241, 85, 126);
            b2.Click += new EventHandler(b_Click);
            CategoryPanel.Controls.Add(b2);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(50, 55, 89);
                    b.Size = new Size(134, 45);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = dr["catName"].ToString();
                    b.CheckedState.FillColor = Color.FromArgb(241, 85, 126);


                    //event for click
                    b.Click += new EventHandler(b_Click);
                    CategoryPanel.Controls.Add(b);
                }
            }

        }

        private void b_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            if (b.Text == "All Categories")
            {
                txtSearch.Text = "1";
                txtSearch.Text = "";
                return;
            }

            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }

        }

        private void AddItems(string id,String proID, string name, string cat, string price, Image pimage)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(proID),
            };

            ProductPanel.Controls.Add(w);

            w.onSelect += (Ss, ee) =>
            {
                var wdg = (ucProduct)Ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    //this line check if product is already there then a one to quantitiy and update price
                    if (Convert.ToInt32(item.Cells["dgvPID"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = (int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1).ToString();
                        item.Cells["dgvAmout"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                   double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        return;
                    }

                }
                //this line add new product
                guna2DataGridView1.Rows.Add(new object[] {0,0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                GetTotal();

            };


        }

        //Getting product from database
        private void loadProduct()
        {
            string qry = "SELECT * FROM products inner join  category on catID = categoryID  ";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (Byte[])item["pImage"];
                byte[] immagbytearray = imagearray;

                AddItems("0",item["pID"].ToString(), item["pName"].ToString(), item["catName"].ToString(),
                    item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)));
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //for serial number

            int count = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }
        private void GetTotal()
        {
            double tot = 0;
            lblTotal.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmout"].Value.ToString());
            }
            lblTotal.Text = tot.ToString("N2");
        }




        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTalbe.Text = " ";
            lblWaiter.Text = " ";
            lblTalbe.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "0.00";
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTalbe.Text = " ";
            lblWaiter.Text = " ";
            lblTalbe.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Delivery";




            frmAddCustomer frm = new frmAddCustomer();
            frm.mainID = MainID;
            frm.orderType = OrderType;
            MainClass.BlurBackground(frm);
            if (frm.txtName.Text != "")
            {
                driverID = frm.driverID;
                lbDriverName.Text = "Customer name :" + frm.txtName.Text + "Phone: " + frm.txtPhone.Text;
                lbDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }

        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            lblTalbe.Text = " ";
            lblWaiter.Text = " ";
            lblTalbe.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Take Away";




            frmAddCustomer frm = new frmAddCustomer();
            frm.mainID = MainID;
            frm.orderType = OrderType;
            MainClass.BlurBackground(frm);
            if (frm.txtName.Text != "")
            {
                driverID = frm.driverID;
                lbDriverName.Text = "Customer name :" + frm.txtName.Text + "Phone: " + frm.txtPhone.Text + "Driver: " + frm.cbDriver.Text;
                lbDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }

        }

        private void btnDin_Click(object sender, EventArgs e)
        {
            OrderType = "Dine In";
            lbDriverName.Visible = false;
            //need to create form table selection and waiter selection
            frmTableSelect frm = new frmTableSelect();
            MainClass.BlurBackground(frm);
            if (frm.TableName != "")
            {
                lblTalbe.Text = frm.TableName;
                lblTalbe.Visible = true;

            }
            else
            {
                lblTalbe.Text = "";
                lblTalbe.Visible = false;
            }

            frmWaiterSelect frm2 = new frmWaiterSelect();
            MainClass.BlurBackground(frm2);
            if (frm2.waiterName != "")
            {
                lblWaiter.Text = frm2.waiterName;
                lblWaiter.Visible = true;

            }
            else
            {
                lblWaiter.Text = "";
                lblWaiter.Visible = false;
            }
        }

        private void btnKot_Click(object sender, EventArgs e)
        {

            //save the data in database
            //create tables
            string qry1 = "";
            string qry2 = "";

            int detailID = 0;

            if (MainID == 0) //insert
            {
                qry1 = @"Insert into tblMain Values(@aDate,@aTime,@TableName,@WaiterName,@status,@orderType,
                                                   @total,@recived,@change,@driverID,@CustName,@CustPhone);
                                                   Select SCOPE_IDENTITY()";
                //This line will get recent add id value
            }
            else  //update
            {
                qry1 = @"Update tblMain Set status=@status,total=@total,received=@received,change=@change where
                                            MainID=@ID";
            }

            SqlCommand cmd = new SqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", MainID);
            cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@TableName", lblTalbe.Text);
            cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Pending");
            cmd.Parameters.AddWithValue("@orderType", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text));//as we only saving data for kitchen value will update when payment recieved
            cmd.Parameters.AddWithValue("@recived", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@driverID", driverID);
            cmd.Parameters.AddWithValue("@CustName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);


            if (MainClass.con.State == ConnectionState.Closed)
            {
                MainClass.con.Open();
            }

            if (MainID == 0)
            {
                MainID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else
            {
                cmd.ExecuteNonQuery();
            }

            if (MainClass.con.State == ConnectionState.Open)
            {
                MainClass.con.Close();
            }


            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                if (detailID == 0) //insert
                {
                    qry2 = @"Insert into TblDetails Values(@MainID,@proID,@qty,@price,@amount)";
                }

                else //update
                {
                    qry2 = @"Update TblDetails Set proID=@proID,qty=@qty,price=@price,amount=@amount
                                              where DetailID=@ID";
                }


                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvPID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmout"].Value));


                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }

                cmd2.ExecuteNonQuery();


                if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }



                guna2MessageDialog1.Show("Saved Successfully");
                MainID = 0;
                detailID = 0;
                guna2DataGridView1.Rows.Clear();
                lblTalbe.Text = " ";
                lblWaiter.Text = " ";
                lblTalbe.Visible = false;
                lblWaiter.Visible = false;
                lblTotal.Text = "0.00";
                lbDriverName.Text = " ";


            }





        }
        public int id = 0;
        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frm = new frmBillList();
            MainClass.BlurBackground(frm);

            if (frm.MainID > 0)
            {
                id = frm.MainID;
                MainID = frm.MainID;
                LoadEntries();
            }
        }

        private void LoadEntries()
        {
            string qry = @"Select * from tblMain m
                                  inner join tblDetails d on m.MainID = d.MainID
                                  inner join products p on p.PID = d.proID
                                  where m.MainID = " + id + "";
            SqlCommand cmd2 = new SqlCommand(qry, MainClass.con);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);


            if (dt2.Rows[0]["orderType"].ToString() == "Delivery")
            {

                btnDelivery.Checked = true;
                lblWaiter.Visible = false;
                lblTalbe.Visible = false;
            }
            else if (dt2.Rows[0]["orderType"].ToString() == "Take Away")
            {

                btnTake.Checked = true;
                lblWaiter.Visible = false;
                lblTalbe.Visible = false;

            }

            else
            {
                btnDin.Checked = true;
                lblWaiter.Visible = true;
                lblTalbe.Visible = true;

            }

            guna2DataGridView1.Rows.Clear();

            foreach (DataRow item in dt2.Rows)
            {

                lblTalbe.Text = item["TableName"].ToString();
                lblWaiter.Text = item["WaiterName"].ToString();



                string detailid = item["DetailID"].ToString();
                string proName = item["pName"].ToString();
                string proid = item["proID"].ToString();
                string qty = item["qty"].ToString();
                string price = item["price"].ToString();
                string amount = item["amount"].ToString();


                object[] obj = { 0, detailid, proid, proName, qty, price, amount };
                guna2DataGridView1.Rows.Add(obj);

            }
            GetTotal();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            frmCheckout frm = new frmCheckout();
            frm.MainID = id;
            frm.amt = Convert.ToDouble(lblTotal.Text);
            MainClass.BlurBackground(frm);

            MainID = 0;
            guna2DataGridView1.Rows.Clear();
            lblTalbe.Text = " ";
            lblWaiter.Text = " ";
            lblTalbe.Visible = false;
            lblWaiter.Visible = false;
            lblTotal.Text = "0.00";

        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            string qry1 = "";
            string qry2 = "";

            int detailID = 0;
            if (OrderType == "")
            {
                guna2MessageDialog1.Show("Please select order type");
                return;
            }
            if (MainID == 0) //insert
            {
                qry1 = @"Insert into tblMain Values(@aDate,@aTime,@TableName,@WaiterName,@status,@orderType,@total,@recived,@change,@DriverID,@CustName,@CustPhone);
                         Select SCOPE_IDENTITY()";
                //This line will get recent add id value
            }
            else  //update
            {
                qry1 = @"Update tblMain Set status=@status,total=@total,received=@received,change=@change where
                                 MainID=@ID";
            }

            SqlCommand cmd = new SqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", MainID);
            cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@TableName", lblTalbe.Text);
            cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Hold");
            cmd.Parameters.AddWithValue("@orderType", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text));//as we only saving data for kitchen value will update when payment recieved
            cmd.Parameters.AddWithValue("@recived", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@driverID", driverID);
            cmd.Parameters.AddWithValue("@CustName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);



            if (MainClass.con.State == ConnectionState.Closed)
            {
                MainClass.con.Open();
            }

            if (MainID == 0)
            {
                MainID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else
            {
                cmd.ExecuteNonQuery();
            }

            if (MainClass.con.State == ConnectionState.Open)
            {
                MainClass.con.Close();
            }


            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                if (detailID == 0) //insert
                {
                    qry2 = @"Insert into TblDetails Values(@MainID,@proID,@qty,@price,@amount)";
                }

                else //update
                {
                    qry2 = @"Update TblDetails Set proID=@proID,qty=@qty,price=@price,amount=@amount
                                   where DetailID=@ID";
                }


                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvPID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmout"].Value));


                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }

                cmd2.ExecuteNonQuery();
                if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }

                if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }



                guna2MessageDialog1.Show("Saved Successfully");
                MainID = 0;
                detailID = 0;
                guna2DataGridView1.Rows.Clear();
                lblTalbe.Text = " ";
                lblWaiter.Text = " ";
                lblTalbe.Visible = false;
                lblWaiter.Visible = false;
                lblTotal.Text = "0.00";
                lbDriverName.Text = " ";

            }
        }

    }
}




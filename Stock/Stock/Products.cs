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

namespace Stock
{
    public partial class Products : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LENOVO-PC\\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True");
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            //insert 
            con.Open();
            bool status = false;
            if (comboBox1.SelectedIndex==0)
            {
                status = true;

            }
            else
            {
                status = false;
            }

            var sqlQuery ="";

            if (IfProductsExists(textBox1.Text))
            {
                sqlQuery = @"UPDATE [Products] SET [ProductName] = '" + textBox2.Text + "' ,[ProductStatus] = '" + status + "' WHERE  [ProductCode]='" + textBox1.Text + "'";
            }
            else
            {
                sqlQuery = @"INSERT INTO[Stock].[dbo].[Products]  ([ProductCode],[ProductName],[ProductStatus]) VALUES
                              ('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";

            }



            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.ExecuteNonQuery();
            con.Close();

            //Reading date
            LoadData();
        }

        private bool IfProductsExists(string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("select 1 from Products where ProductCode='"+ productCode + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)

                return true;
            else
                return false;
        }


        public void LoadData()
        {

                SqlDataAdapter sda = new SqlDataAdapter("select * from Products", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dgvProducts.Rows.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvProducts.Rows.Add();
                    dgvProducts.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                    dgvProducts.Rows[n].Cells[1].Value = item["ProductName"].ToString();

                    if ((bool)item["ProductStatus"])
                    {
                        dgvProducts.Rows[n].Cells[2].Value = "Active";

                    }
                    else
                    {
                        dgvProducts.Rows[n].Cells[2].Value = "Deactive";
                    }


                }

        }

        private void dgvProducts_MouseClick(object sender, MouseEventArgs e)
        {
         

           
        }

        private void dgvProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dgvProducts.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dgvProducts.SelectedRows[0].Cells[1].Value.ToString();

            if (dgvProducts.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                comboBox1.SelectedIndex = 0;

            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sqlQuery = "";

            if (IfProductsExists(textBox1.Text))
            {
                con.Open();
                sqlQuery = @"DELETE FROM [Products]  WHERE  [ProductCode]='" + textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                MessageBox.Show("Record Not Exidt..!");

            }
           //Reading date
            LoadData();



        }
    }
}

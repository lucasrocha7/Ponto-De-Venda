using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    public partial class DailySale : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public string solduser;
        double Preco = 0;
        int PQtd = 0;
        double PrecoCompra = 0;
        double TotalVenda = 0;
        double LucroVenda = 0;

        MainForm main;
        public DailySale(MainForm mn)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            main = mn;
            LoadCashier();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCashier()
        {
            cboCashier.Items.Clear();
            cboCashier.Items.Add("All Cashier");
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tbUser WHERE role LIKE 'Cashier'", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                cboCashier.Items.Add(dr["username"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        public void LoadSold()
        {
            int i = 0;
            int Qty = 0;
            double total = 0;
            double LucroVenda = 0;           
            double lucro = 0;
            double lucrototal = 0;
            double buyprice = 0;
            
            dgvSold.Rows.Clear();
            cn.Open();
            if(cboCashier.Text=="All Cashier")
            {
                cm = new SqlCommand("select c.id, c.transno, c.pcode, p.pdesc, c.price,p.buyprice, c.qty, c.disc, c.total, c.lucro, c.lucrototal, c.fpagamento from tbCart as c inner join tbProduct as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dtFrom.Value+ "' and '" + dtTo.Value + "'", cn);
            }
            else
            {
                cm = new SqlCommand("select c.id, c.transno, c.pcode, p.pdesc, c.price,p.buyprice, c.qty, c.disc, c.total, c.lucro, c.lucrototal, c.fpagamento from tbCart as c inner join tbProduct as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dtFrom.Value + "' and '" + dtTo.Value + "' and cashier like '" + cboCashier.Text + "'", cn);
            }
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                total += double.Parse(dr["total"].ToString());               
                buyprice += double.Parse(dr["buyprice"].ToString());
                dgvSold.Rows.Add(i, dr["id"].ToString(), dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["buyprice"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString(), dr["lucro"].ToString(), dr["fpagamento"].ToString());
             
            }
            dr.Close();
            cn.Close();
                   
            
           /* for (i = 0; i <dgvSold.Rows.Count; i++)
            {
                Lucro = buyprice + Convert.ToDouble(dgvSold.Rows.Count[i].Cells.[9].value);
            }*/


            
            lblTotal.Text = total.ToString("R$ #,##0.00");
            lblLucro.Text = lucro.ToString("R$ #,##0.00");

          foreach (DataGridViewRow row in dgvSold.Rows)
            {             
                int PQtd = Convert.ToInt32(row.Cells[7].Value);
                double PrecoCompra = Convert.ToDouble(row.Cells[6].Value);             
                double TotalVenda = Convert.ToDouble(row.Cells[9].Value);
                          
                if (PQtd != 0)
                {
                    PrecoCompra = (PrecoCompra * PQtd);
                    

                    LucroVenda = (TotalVenda - PrecoCompra);
                    row.Cells[10].Value = LucroVenda.ToString("#,##0.00");

                 /* lucro+= Convert.ToInt32(row.Cells[10].Value);

                    lblLucro.Text = Convert.ToDouble(lucro).ToString("C"); */



                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
            }

           foreach(DataGridViewRow col in dgvSold.Rows)
            {
                lucro = lucro + Convert.ToDouble(col.Cells[10].Value);

                lblLucro.Text = lucro.ToString("R$ #,##0.00");
            }



        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void DailySale_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void dgvSold_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSold.Columns[e.ColumnIndex].Name;
            if(colName=="Cancel")
            {
                CancelOrder cancelOrder = new CancelOrder(this);
                cancelOrder.txtId.Text = dgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancelOrder.txtTransno.Text = dgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancelOrder.txtPcode.Text = dgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancelOrder.txtDesc.Text = dgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancelOrder.txtPrice.Text = dgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancelOrder.txtQty.Text = dgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancelOrder.txtDisc.Text = dgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancelOrder.txtTotal.Text = dgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();               
                cancelOrder.txtCancelBy.Text = solduser;
                cancelOrder.ShowDialog();

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            string param = "Date From: " + dtFrom.Value.ToShortDateString() + " To: " + dtTo.Value.ToShortDateString();
               
            if (cboCashier.Text == "Todos")
            {
                report.LoadDailyReport("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc as discount, c.total from tbCart as c inner join tbProduct as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dtFrom.Value + "' and '" + dtTo.Value + "'", param, cboCashier.Text);
            }
            else
            {
                report.LoadDailyReport("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc as discount, c.total from tbCart as c inner join tbProduct as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dtFrom.Value + "' and '" + dtTo.Value + "' and cashier like '" + cboCashier.Text + "'", param, cboCashier.Text);
            }
            report.ShowDialog();
        }

        private void dgvSold_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSold.Columns[e.ColumnIndex].Name;
            if(colName=="Cancel")
            {
                CancelOrder cancel = new CancelOrder(this);
                cancel.txtId.Text = dgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancel.txtTransno.Text = dgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancel.txtPcode.Text = dgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancel.txtDesc.Text = dgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancel.txtPrice.Text = dgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancel.txtQty.Text = dgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancel.txtDisc.Text = dgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancel.txtTotal.Text = dgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();
                if(lblTitle.Visible==false)
                    cancel.txtCancelBy.Text = main.lblUsername.Text;
                else
                    cancel.txtCancelBy.Text = solduser;
                cancel.ShowDialog();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    public partial class ProductModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string stitle = "Point Of Sales";
        Product product;
        public ProductModule(Product pd)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            product = pd;
            LoadBrand();
            LoadCategory();
            txtPercent.Enabled = false;
            UDReOrder.Enabled = false;
            txtPrice.Enabled = false;
        }

        public void LoadCategory()
        {
            cboCategory.Items.Clear();
            cboCategory.DataSource = dbcon.getTable("SELECT * FROM tbCategory");
            cboCategory.DisplayMember = "category";
            cboCategory.ValueMember = "id";
        }

        public void LoadBrand()
        {
            cboBrand.Items.Clear();
            cboBrand.DataSource = dbcon.getTable("SELECT * FROM tbBrand");
            cboBrand.DisplayMember = "brand";
            cboBrand.ValueMember = "id";
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtPcode.Clear();
            txtBarcode.Clear();
            txtPdesc.Clear();
            txtPrice.Clear();
            txtbuyprice.Clear();
            txtPrice.Clear();
            txtPercent.Clear();
            cboBrand.SelectedIndex = 0;
            cboCategory.SelectedIndex = 0;
            UDReOrder.Value = 1;

            txtPcode.Enabled = true;
            txtPcode.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try          
            {
                if (MessageBox.Show("Tem certeza de que deseja salvar este produto?", "Salvar Produto", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbProduct(pcode, barcode, pdesc, bid, cid, price, buyprice, reorder)VALUES (@pcode,@barcode,@pdesc,@bid,@cid,@price,@buyprice,@reorder)", cn);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@bid", cboBrand.SelectedValue);
                    cm.Parameters.AddWithValue("@cid", cboCategory.SelectedValue);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@buyprice", double.Parse(txtbuyprice.Text));
                    cm.Parameters.AddWithValue("@reorder", UDReOrder.Value);                    
                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("O produto foi salvo com sucesso.", stitle);
                    Clear();
                    product.LoadProduct();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Tem certeza de que deseja atualizar este produto?", "Update Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbProduct SET barcode=@barcode,pdesc=@pdesc,bid=@bid,cid=@cid,price=@price,buyprice=@buyprice,reorder=@reorder  WHERE pcode LIKE @pcode", cn);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@bid", cboBrand.SelectedValue);
                    cm.Parameters.AddWithValue("@cid", cboCategory.SelectedValue);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@buyprice", double.Parse(txtbuyprice.Text));
                    cm.Parameters.AddWithValue("@reorder", UDReOrder.Value);                  
                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("O produto foi atualizado com sucesso.", stitle);
                    Clear();
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ProductModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void ProductModule_Validating(object sender, CancelEventArgs e)
        {

        }

        
        private void txtPercent_TextChanged(object sender, EventArgs e)
        {
            if(txtPercent.Text != "")
            {
                double percent = double.Parse(txtbuyprice.Text) * double.Parse(txtPercent.Text)* 0.01;

                double custo = double.Parse(txtbuyprice.Text);

                double Pprice = custo + percent;

                txtPrice.Text = Pprice.ToString();
            }
        }

        private void txtbuyprice_TextChanged(object sender, EventArgs e)
        {
            if( txtbuyprice.Text != "")
            {
                txtPercent.Enabled = true;
                UDReOrder.Enabled = true;
                txtPrice.Enabled = true;
            }
            else
            {
                txtPercent.Enabled = false;
                UDReOrder.Enabled = false;
                txtPrice.Enabled = false;
            }
        }
        private void TxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals((char)Keys.Back))
            {

                TextBox t = (TextBox)sender;
                string w = Regex.Replace(t.Text, "[^0-9]", string.Empty);
                if (w == string.Empty) w = "00";

                if(e.KeyChar.Equals((char)Keys.Back))
                    w = w.Substring(0, w.Length - 1);

                w += e.KeyChar;

                t.Text = string.Format("{0:#,##0.00}", double.Parse(w) / 100);

                t.Select(t.Text.Length, 0);
            }
            e.Handled = true;
           
        }
        private void txtPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals((char)Keys.Back))
            {
                return;
            }
            e.Handled = true;

        }
        private void txtbuyprice_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                TextBox t = (TextBox)sender;
                t.Text = String.Format("{0:#,##0.00}", 0d);
                t.Select(t.Text.Length, 0);
                e.Handled = true;
            }

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void UDReOrder_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboBrand_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtPdesc_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPcode_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}



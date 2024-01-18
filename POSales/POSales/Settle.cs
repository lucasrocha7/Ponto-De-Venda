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

namespace POSales
{
    public partial class Settle : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();        
        Cashier cashier;
        string MeioDePagamento = "Nenhum";
        public Settle(Cashier cash)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            this.KeyPreview = true;
            cashier = cash;
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnOne.Text;
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnTwo.Text;
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnThree.Text;
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnFour.Text;
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnFive.Text;
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnSix.Text;
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnSeven.Text;
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnEight.Text;
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnNine.Text;
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnZero.Text;
        }

        private void btnDZero_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnDZero.Text;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCash.Clear();
            txtCash.Focus();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if ((double.Parse(txtChange.Text) < 0) || (txtCash.Text.Equals("")))
                {
                    MessageBox.Show("Valor insuficiente. Por favor, insira o valor correto!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((double.Parse(txtChange.Text) > 0))
                {
                    MessageBox.Show("O Valor ultrapassou o valor da compra. Por favor, insira o valor correto!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    for(int i=0; i< cashier.dgvCash.Rows.Count; i++ )
                    {
                        cn.Open();
                        cm = new SqlCommand("UPDATE tbProduct SET qty = qty - " + int.Parse(cashier.dgvCash.Rows[i].Cells[6].Value.ToString()) + "WHERE pcode= '" + cashier.dgvCash.Rows[i].Cells[2].Value.ToString() + "'", cn);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        cm = new SqlCommand("UPDATE tbCart SET status = 'Sold' WHERE id= '" + cashier.dgvCash.Rows[i].Cells[1].Value.ToString() + "'", cn);
                        cm.ExecuteNonQuery();
                        cm = new SqlCommand("UPDATE tbCart SET fpagamento = '"+ MeioDePagamento + "' WHERE id= '" + cashier.dgvCash.Rows[i].Cells[1].Value.ToString() + "'", cn);
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    Recept recept = new Recept(cashier);
                    recept.LoadRecept(txtCash.Text, txtChange.Text);
                    recept.ShowDialog();

                    MessageBox.Show("Pagamento salvo com sucesso!", "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cashier.GetTranNo();
                    cashier.LoadCart();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Valor incorreto. Por favor, insira o valor correto!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            

            try
            {
                double sale = double.Parse(txtSale.Text);
                double cash = double.Parse(txtCash.Text);
                double charge = cash - sale;
                txtChange.Text = charge.ToString("#,##0.00");

                if (double.Parse(txtCash.Text) > sale)
                {
                    txtChange.Text = ("Troco: " + txtChange.Text);                   
                    return;
                }

                if (double.Parse(txtCash.Text) < sale)
                {
                    txtChange.Text = ("Faltou: " + txtChange.Text);                  
                    return;
                }
            }
            catch (Exception)
            {
                txtChange.Text = "0.00";
            }
        }

        private void Settle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Dispose();
            else if (e.KeyCode == Keys.Enter) btnEnter.PerformClick();            
        }

         private void RadMoney_CheckedChanged(object sender, EventArgs e)
        {
            MeioDePagamento = RadMoney.Text.ToString();
        }

        private void RadCard_CheckedChanged(object sender, EventArgs e)
        {
            MeioDePagamento = RadCard.Text.ToString();
        }

        private void RadPix_CheckedChanged(object sender, EventArgs e)
        {
            MeioDePagamento = RadPix.Text.ToString();
        }
    }
}

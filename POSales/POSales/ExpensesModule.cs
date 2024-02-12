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
    public partial class ExpensesModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        public ExpensesModule()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            btnUpdate.Enabled = false;
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (MessageBox.Show("Tem certeza de que deseja salvar esta despesa?", "Salvar Despesa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbExpenses(Descricao, Valor, Data)VALUES (@Descricao, @Valor, @Data)", cn);
                    cm.Parameters.AddWithValue("@Descricao", txtDesc.Text);
                    cm.Parameters.AddWithValue("@Valor", decimal.Parse(txtVal.Text));                  
                    cm.Parameters.AddWithValue("@Data", DateTime.Now.ToString());
                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("A Despesa foi salva com sucesso.");
                    Clear();
                    
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        public void Clear()
        {
            txtDesc.Clear();
            txtVal.Clear();          
           
            
            txtDesc.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
        public void RandomId()
        {
            Random rnd = new Random();
            txtId.Clear();
            txtId.Text += rnd.Next();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();          
        }

        private void ExpensesModule_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (MessageBox.Show("Tem certeza de que deseja atualizar esta despesa?", "Atualizar Despesa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("UPDATE tbExpenses SET Descricao=@Descricao,Valor=@Valor,Data=@Data WHERE Id like '"+lblId.Text+"' ", cn);                   
                    cm.Parameters.AddWithValue("@Descricao", txtDesc.Text);
                    cm.Parameters.AddWithValue("@Valor", decimal.Parse(txtVal.Text));                                  
                    cm.Parameters.AddWithValue("@Data", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));                   
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("A Despesa foi atualizada com sucesso.");
                    Clear();
                    this.Dispose();
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtRefNo2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRefNo2_Click(object sender, EventArgs e)
        {
            RandomId();
            
        }
    }
}

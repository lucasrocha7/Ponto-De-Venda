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
                    cm = new SqlCommand("INSERT INTO tbExpenses(Conta_luz, Conta_Agua, Funcionarios, internet, Outros, Data)VALUES (@Conta_luz,@Conta_Agua,@Funcionarios,@internet,@Outros, @Data)", cn);
                    cm.Parameters.AddWithValue("@Conta_luz", decimal.Parse(txtLuz.Text));
                    cm.Parameters.AddWithValue("@Conta_Agua", decimal.Parse(txtAgua.Text));
                    cm.Parameters.AddWithValue("@Funcionarios", decimal.Parse(txtFunc.Text));
                    cm.Parameters.AddWithValue("@internet", decimal.Parse(txtNet.Text));
                    cm.Parameters.AddWithValue("@Outros", txtOutros.Text);
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
            txtLuz.Clear();
            txtAgua.Clear();
            txtFunc.Clear();   
            txtNet.Clear();
            txtOutros.Clear();
            txtId.Clear();    
            
            txtLuz.Focus();
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
                    cm = new SqlCommand("UPDATE tbExpenses SET Conta_luz=@Conta_luz,Conta_Agua=@Conta_Agua,Funcionarios=@Funcionarios,internet=@internet,Outros=@Outros,Data=@Data WHERE Id = @Id", cn);
                    cm.Parameters.AddWithValue("@Id", txtId.Text);
                    cm.Parameters.AddWithValue("@Conta_luz", decimal.Parse(txtLuz.Text));
                    cm.Parameters.AddWithValue("@Conta_Agua", decimal.Parse(txtAgua.Text));
                    cm.Parameters.AddWithValue("@Funcionarios", decimal.Parse(txtFunc.Text));
                    cm.Parameters.AddWithValue("@internet", decimal.Parse(txtNet.Text));
                    cm.Parameters.AddWithValue("@Outros", txtOutros.Text);
                    cm.Parameters.AddWithValue("@Data", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("A Despesa foi atualizada com sucesso.");                   
                    this.Dispose();
                    
                }
            }
            catch
            {

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

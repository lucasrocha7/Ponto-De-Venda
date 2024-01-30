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
    public partial class Expenses : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        public Expenses()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadExpenses();
        }


        public void LoadExpenses()
        {
            int i = 0;
            dgvExpenses.Rows.Clear();
            cm = new SqlCommand("SELECT Conta_Luz, Conta_Agua, Funcionarios, Internet, Outros, Data FROM tbExpenses WHERE Data BETWEEN '" + dtFrom.Value.ToString() +"' and '"+ dtTo.Value.ToString() + "' GROUP BY Conta_Luz, Conta_Agua, Funcionarios, Outros, Internet, Data ", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvExpenses.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ExpensesModule expensesModule = new ExpensesModule();
            expensesModule.ShowDialog();
        }

        private void dgvExpenses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvExpenses.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ExpensesModule expenses = new ExpensesModule();
                expenses.txtId.Text = dgvExpenses.Rows[e.RowIndex].Cells[0].Value.ToString();
                expenses.txtLuz.Text = dgvExpenses.Rows[e.RowIndex].Cells[1].Value.ToString();
                expenses.txtAgua.Text = dgvExpenses.Rows[e.RowIndex].Cells[2].Value.ToString();
                expenses.txtFunc.Text = dgvExpenses.Rows[e.RowIndex].Cells[3].Value.ToString();
                expenses.txtNet.Text = dgvExpenses.Rows[e.RowIndex].Cells[4].Value.ToString();
                expenses.txtOutros.Text = dgvExpenses.Rows[e.RowIndex].Cells[5].Value.ToString();

                expenses.txtId.Enabled = false;
                expenses.btnSave.Enabled = false;
                expenses.btnUpdate.Enabled = true;
                expenses.ShowDialog();
            }

            else if (colName == "Delete")
            {
                if (MessageBox.Show("Tem certeza de que deseja excluir este registro?", "Apagar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbExpenses WHERE Id LIKE '" + dgvExpenses[0, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("O produto foi excluído com sucesso.", "Point Of Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadExpenses();

        }

        private void btnLoadSoldItems_Click(object sender, EventArgs e)
        {
            LoadExpenses();
        }
    }
}

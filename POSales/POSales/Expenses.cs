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
            cn.Open();
            cm = new SqlCommand("SELECT Id, Descricao, Valor, Data FROM tbExpenses ", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvExpenses.Rows.Add(i, dr["Id"].ToString(), dr["Descricao"].ToString(), dr["Valor"].ToString(), dr["Data"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ExpensesModule expensesModule = new ExpensesModule(this);
            expensesModule.ShowDialog();
        }

        private void dgvExpenses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvExpenses.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ExpensesModule expenses = new ExpensesModule(this);
                expenses.lblId.Text = dgvExpenses.Rows[e.RowIndex].Cells[1].Value.ToString();
                expenses.txtDesc.Text = dgvExpenses.Rows[e.RowIndex].Cells[2].Value.ToString();
                expenses.txtVal.Text = dgvExpenses.Rows[e.RowIndex].Cells[3].Value.ToString();               
                expenses.btnSave.Enabled = false;
                expenses.btnUpdate.Enabled = true;
                expenses.ShowDialog();
            }

            else if (colName == "Delete")
            {
                if (MessageBox.Show("Tem certeza de que deseja excluir este registro?", "Apagar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbExpenses WHERE Id LIKE '" + dgvExpenses[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("O produto foi excluído com sucesso.", "Point Of Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadExpenses();

        }

        private void btnLoadSoldItems_Click(object sender, EventArgs e)
        {
            int i = 0;
            dgvExpenses.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT Id, Descricao, Valor, Data FROM tbExpenses WHERE Data BETWEEN '" + dtFrom.Value.ToString() + "' AND '" + dtTo.Value.ToString() + "' ", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvExpenses.Rows.Add(i, dr["Id"].ToString(), dr["Descricao"].ToString(), dr["Valor"].ToString(), dr["Data"].ToString());
            }
            dr.Close();
            cn.Close();
        }
    }
}

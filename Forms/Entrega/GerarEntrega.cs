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

namespace Ação_Social_PIB
{
    public partial class GerarEntrega : Form
    {

        Dictionary<string, string> config;
        public string IdToAdd { get; set; }
        public DataTable EntregaDataTable { get; set; }
        DataBaseHandler dataBase;
        int totalDeCestas;
        int cestasRestantes = 0;

        public GerarEntrega(Dictionary<string, string> config)
        {
            InitializeComponent();
            this.config = config;
            this.dataBase = new DataBaseHandler(config["DataBasePath"].ToString());

            string queryDefaultDataTable = "SELECT id_cadastro, nome AS NOME, telefone AS TELEFONE, endereco AS ENDEREÇO, cidade AS CIDADE," +
                " numPessoas AS 'NUMERO DE PESSOAS' FROM tb_cadastro WHERE id_cadastro NOT IN (SELECT id_cadastro FROM tb_entrega_cadastro WHERE" +
                " id_entrega IN (SELECT id_entrega FROM tb_entrega WHERE dataEntrega >= DATE('now', '-' || (SELECT mesesMax FROM settings_entrega LIMIT 1) || 'months')))" +
                " OR id_cadastro NOT IN (SELECT id_cadastro FROM tb_entrega_cadastro) AND (SELECT voltarRodizio FROM settings_entrega LIMIT 1) = 'true'" +
                " ORDER BY CASE WHEN id_cadastro IN (SELECT id_cadastro FROM tb_entrega_cadastro WHERE id_entrega IN (SELECT id_entrega FROM tb_entrega" +
                " WHERE dataEntrega >= DATE('now', '-' || (SELECT mesesMax FROM settings_entrega LIMIT 1) || ' months'))) THEN 1 ELSE 0 END DESC LIMIT" +
                " (SELECT numFamiliasContempladas FROM settings_entrega LIMIT 1);";

            EntregaDataTable = dataBase.BuscarDados(queryDefaultDataTable);

            EntregaDataTable.Columns.Add("QTD CESTAS", typeof(int));
            this.totalDeCestas = int.Parse(dataBase.BuscarDados("SELECT numCestasPorEntrega FROM settings_entrega LIMIT 1").Rows[0][0].ToString());
            int totalDeFamilias = int.Parse(dataBase.BuscarDados("SELECT numFamiliasContempladas FROM settings_entrega LIMIT 1").Rows[0][0].ToString());

            if (totalDeCestas - totalDeFamilias != 0)
                MessageBox.Show($"O número de cestas a serem distribuídas são {totalDeCestas}, já o de famílias a serem beneficiadas {totalDeFamilias}. Estão sobrando {totalDeCestas - totalDeFamilias} cestas para serem distribuídas!");

            cestasRestantes = totalDeCestas - totalDeFamilias;
            label3.Text = cestasRestantes.ToString();

            foreach (DataRow row in EntregaDataTable.Rows)
            {
                row["QTD CESTAS"] = 1;
            }

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = EntregaDataTable;
            dataGridView1.Columns[0].Visible = false;
        }

        // Evento para manipular a alteração de células no DataGridView
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se a alteração ocorreu na coluna de valores inteiros
            if (e.ColumnIndex == 6)
            {
                // Atualiza a soma dos valores na coluna
                int sum = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[6].Value != null)
                    {
                        sum += Convert.ToInt32(row.Cells[6].Value);
                    }
                }

                // Verifica se a soma excede o limite
                if (sum > this.totalDeCestas)
                {
                    // Atualiza a soma dos valores na coluna
                    int sumReplace = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[6].Value != null && row != dataGridView1.Rows[e.RowIndex])
                        {
                            sumReplace += Convert.ToInt32(row.Cells[6].Value);
                        }
                    }
                    // Reverta para o valor anterior ou impeça a alteração
                    // Exemplo de revertendo para o valor anterior
                    MessageBox.Show($"O número de cestas a serem distribuídas são {this.totalDeCestas}, o valor da célula foi alterado para {this.totalDeCestas - sumReplace}!");
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.totalDeCestas - sumReplace;
                    label3.Text = "0";
                }
                else
                {
                    this.cestasRestantes = this.totalDeCestas - sum;
                    label3.Text = (cestasRestantes).ToString();
                }
            }
        }

        // Evento para remover uma linha selecionada
        private void buttonRemover_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    int numDeCestas = int.Parse(EntregaDataTable.Rows[row.Index]["QTD CESTAS"].ToString());
                    EntregaDataTable.Rows.Remove(EntregaDataTable.Rows[row.Index]);
                    this.cestasRestantes += numDeCestas;
                    label3.Text = (this.cestasRestantes).ToString();
                }
            }
        }

        private void buttonAddBeneficiario_Click(object sender, EventArgs e)
        {
            int sum = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[6].Value != null)
                {
                    sum += Convert.ToInt32(row.Cells[6].Value);
                }
            }

            // Verifica se a soma excede o limite
            if (sum == this.totalDeCestas)
            {
                MessageBox.Show($"O número de cestas a serem distribuídas são {this.totalDeCestas}, não é possível adicionar mais um beneficiário!");
                return;
            }

            BuscarCadastro buscar = new BuscarCadastro(config, this);
            buscar.ShowDialog();

            string queryToAdd = "SELECT id_cadastro, nome AS NOME, telefone AS TELEFONE, endereco AS ENDEREÇO, cidade AS CIDADE," +
                $" numPessoas AS 'NUMERO DE PESSOAS' FROM tb_cadastro WHERE id_cadastro = {this.IdToAdd}";

            DataTable DataTableToAdd = dataBase.BuscarDados(queryToAdd);
            DataTableToAdd.Columns.Add("QTD CESTAS", typeof(int));

            DataTableToAdd.Rows[0]["QTD CESTAS"] = 1;

            DataRow newRow = EntregaDataTable.NewRow();

            foreach (DataColumn column in EntregaDataTable.Columns)
            {
                newRow[column.ColumnName] = DataTableToAdd.Rows[0][column.ColumnName];
            }

            EntregaDataTable.Rows.Add(newRow);

            this.cestasRestantes--;
            label3.Text = (this.cestasRestantes).ToString();
        }

        private void buttonGerarEntrega_Click(object sender, EventArgs e)
        {
            string insertEntrega = $"INSERT INTO tb_entrega (dataEntrega, quantidadeCestas) VALUES ('{DateTime.Now.ToString("yyyy-MM-dd")}', {this.totalDeCestas - this.cestasRestantes})";
            
            bool inseriuEntrega = dataBase.ExecutarNonQuery(insertEntrega);

            if (inseriuEntrega) 
            {
                string selectMaxIdEntrega = "SELECT MAX(id_entrega) FROM tb_entrega";
                int idEntrega = int.Parse(dataBase.BuscarDados(selectMaxIdEntrega).Rows[0][0].ToString());

                string insertEntregaCadastro = "";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string idCadastro = row.Cells["id_cadastro"].Value.ToString();
                        string qtdDeCestas = row.Cells["QTD CESTAS"].Value.ToString();

                        insertEntregaCadastro += $"INSERT INTO tb_entrega_cadastro (id_cadastro, id_entrega, quantidadeCestasCadastro) VALUES ('{idCadastro}', {idEntrega}, {qtdDeCestas});";
                    }
                }

                bool inseriuEntregasCadastro = dataBase.ExecutarNonQuery(insertEntregaCadastro);

                if (inseriuEntregasCadastro)
                {
                    MessageBox.Show("A entrega foi salva com sucesso!");
                    this.Close();
                }
                else
                    MessageBox.Show("Houve um erro ao tentar salvar a entrega!");
            }
            else
                MessageBox.Show("Houve um erro ao tentar salvar a entrega!");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ação_Social_PIB
{
    public partial class BuscarCadastro : Form
    {
        Dictionary<string, string> config;
        Form sender;
        string idBusca = "";

        public BuscarCadastro(Dictionary<string, string> config, Form sender)
        {
            InitializeComponent();
            this.config = config;
            this.sender = sender;
        }

        private void BuscarCadastros()
        {
            DataBaseHandler database = new DataBaseHandler(config["DataBasePath"]);
            string nome = textBoxNome.Text;

            string query = $"SELECT id_cadastro AS ID, nome AS NOME, telefone AS TELEFONE, endereco AS ENDEREÇO, cidade AS CIDADE, uf AS UF " +
                $"FROM tb_cadastro WHERE nome LIKE '%{nome}%'";

            DataTable CadastrosDataDable = database.BuscarDados(query);

            // Definir o DataTable como a fonte de dados do DataGridView
            dataGridViewBusca.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBusca.ReadOnly = true;
            dataGridViewBusca.AllowUserToAddRows = false;
            dataGridViewBusca.DataSource = CadastrosDataDable;
            dataGridViewBusca.Columns[0].Visible = false;
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            BuscarCadastros();
        }

        private void dataGridViewBusca_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar se uma célula foi clicada e se é uma célula de dados
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Obter o ID do registro selecionado (supondo que o ID seja a primeira coluna)
                string id = dataGridViewBusca.Rows[e.RowIndex].Cells[0].Value.ToString();

                // Exibir um formulário de edição para o registro selecionado
                if (this.sender.GetType() == typeof(PaginaInicial))
                {
                    ModificaApagarCadastro modfApagCadastro = new ModificaApagarCadastro(config, id);
                    modfApagCadastro.ShowDialog();
                }
                else
                {
                    if (this.sender.GetType() == typeof(GerarEntrega))
                    {
                        GerarEntrega senderDownCasted = (GerarEntrega)this.sender;
                        senderDownCasted.IdToAdd = id;
                        this.Close();
                    }
                        
                }
                
            }
            dataGridViewBusca.DataSource = null;
        }
    }
}

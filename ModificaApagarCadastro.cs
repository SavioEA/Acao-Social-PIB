using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ação_Social_PIB
{
    public partial class ModificaApagarCadastro : Form
    {

        Dictionary<string, string> config;
        string id;
        DataBaseHandler dataBase;

        public ModificaApagarCadastro(Dictionary<string, string> config, string id)
        {
            InitializeComponent();
            this.config = config;
            this.id = id;
            this.dataBase = new DataBaseHandler(config["DataBasePath"]);

            DataBaseHandler database = new DataBaseHandler(config["DataBasePath"]);
            var selectStatement = $"SELECT * FROM tb_cadastro WHERE id_cadastro = '{id}';";

            DataRow row = database.BuscarDados(selectStatement).Rows[0];

            textBoxNome.Text = row["nome"].ToString().Trim();

            string rua = row["endereco"].ToString().Split(',')[0].Trim();
            string numCasa = row["endereco"].ToString().Split(',')[1].Trim();
            string bairro = row["endereco"].ToString().Split(',')[2].Trim();

            textBoxRua.Text = rua;
            textBoxNumeroCasa.Text = numCasa;
            textBoxBairro.Text = bairro;

            textBoxTelefone.Text = row["telefone"].ToString();
            textBoxCidade.Text = row["cidade"].ToString().Trim();
            textBoxNumPessoas.Text = row["numPessoas"].ToString();
            comboBoxUF.Text = row["uf"].ToString();
            dateTimePickerDataNasc.Value = DateTime.ParseExact(row["dataNasc"].ToString(), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            // Fechar a forma atual (Pagina1) para voltar para a forma anterior (PaginaInicial)
            this.Close();
        }

        private void textBoxTelefone_TextChanged(object sender, EventArgs e)
        {
            string telefone = new string(textBoxTelefone.Text.Where(char.IsDigit).ToArray());

            // Se o CPF tiver mais de 11 dígitos, limita ao máximo de 11 dígitos
            if (telefone.Length > 11)
                telefone = telefone.Substring(0, 11);
        }

        private void textBoxTelefone_Leave(object sender, EventArgs e)
        {
            string telefone = new string(textBoxTelefone.Text.Where(char.IsDigit).ToArray());

            // Aplica a formatação desejada
            if (telefone.Length == 11)
            {
                telefone = $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 5)}-{telefone.Substring(7, 4)}";
            }

            // Atualiza o texto no TextBox
            textBoxTelefone.Text = telefone;
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            string nome = textBoxNome.Text.Trim();
            string dataNasc = dateTimePickerDataNasc.Value.ToString("yyyy-MM-dd");
            string telefone = textBoxTelefone.Text;
            string endereco = textBoxRua.Text.Trim() + ", " + textBoxNumeroCasa.Text.Trim() + ", " + textBoxBairro.Text.Trim();
            string cidade = textBoxCidade.Text.Trim();
            string uf = comboBoxUF.Text;
            int? numPessoas;

            if (textBoxNumPessoas.Text != "")
                numPessoas = Convert.ToInt32(textBoxNumPessoas.Text);
            else
                numPessoas = null;

            bool dadosModificados = this.dataBase.AtualizarCadastro(this.id, nome, dataNasc, telefone, endereco, cidade, uf, numPessoas);

            if (dadosModificados)
            {
                MessageBox.Show("Dados salvos com sucesso!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Ocorreu um erro ao salvar os dados, revise o fomulário!");
            }
        }

        private void buttonApagar_Click(object sender, EventArgs e)
        {
            // Mensagem de confirmação
            DialogResult result = MessageBox.Show("Tem certeza de que deseja excluir este registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verificar se o usuário confirmou a exclusão
            if (result == DialogResult.Yes)
            {
                bool dadosApagados = this.dataBase.ApagarCadastro(this.id);

                if (dadosApagados)
                {
                    MessageBox.Show("Dados apagados com sucesso!");
                    this.Close();
                }
            }
        }
    }
}

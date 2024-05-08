using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ação_Social_PIB
{
    public partial class NovoCadastro : Form
    {

        Dictionary<string, string> config;

        public NovoCadastro(Dictionary<string, string> config)
        {
            InitializeComponent();
            this.config = config;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            // Fechar a forma atual (Pagina1) para voltar para a forma anterior (PaginaInicial)
            this.Close();
        }

        private void textBoxCPF_TextChanged(object sender, EventArgs e)
        {
            // Remove qualquer caractere que não seja um dígito
            string cpf = new string(textBoxCPF.Text.Where(char.IsDigit).ToArray());

            // Se o CPF tiver mais de 11 dígitos, limita ao máximo de 11 dígitos
            if (cpf.Length > 11)
                cpf = cpf.Substring(0, 11);
        }

        private void textBoxCPF_Leave(object sender, EventArgs e)
        {
            // Remove qualquer caractere que não seja um dígito
            string cpf = new string(textBoxCPF.Text.Where(char.IsDigit).ToArray());

            // Aplica a formatação desejada
            if (cpf.Length == 11)
            {
                cpf = $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9)}";
            }

            // Atualiza o texto no TextBox
            textBoxCPF.Text = cpf;
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

        private void buttonRegistrar_Click(object sender, EventArgs e)
        {
            DataBaseHandler dataBase = new DataBaseHandler(config["DataBasePath"]);

            string cpf = textBoxCPF.Text;
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

            bool dadosInseridos = dataBase.InserirCadastro(cpf, nome, dataNasc, telefone, endereco, cidade, uf, numPessoas);

            if (dadosInseridos)
            {
                MessageBox.Show("Dados salvos com sucesso!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Ocorreu um erro ao salvar os dados, revise o fomulário!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

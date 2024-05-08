using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ação_Social_PIB
{
    public partial class OpcoesEntrega : Form
    {
        Dictionary<string, string> config;
        DataBaseHandler dataBase;

        public OpcoesEntrega(Dictionary<string, string> config)
        {
            InitializeComponent();
            this.config = config;

            this.dataBase = new DataBaseHandler(config["DataBasePath"]);

            DataBaseHandler database = new DataBaseHandler(config["DataBasePath"]);
            var selectStatement = $"SELECT * FROM settings_entrega WHERE id_setting = 1;";

            DataRow row = database.BuscarDados(selectStatement).Rows[0];

            textBox1.Text = row["mesesMax"].ToString();
            bool voltaRodizio = Convert.ToBoolean(row["voltarRodizio"].ToString());

            if (voltaRodizio)
                comboBox1.Text = "Sim";
            else
                comboBox1.Text = "Não";

            textBox2.Text = row["numCestasPorEntrega"].ToString();
            textBox3.Text = row["numFamiliasContempladas"].ToString();
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            // Mensagem de confirmação
            DialogResult result = MessageBox.Show("Tem certeza de que deseja alterar essas opções?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verificar se o usuário confirmou a exclusão
            if (result == DialogResult.Yes)
            {
                string voltarRodizio;
                if (comboBox1.SelectedIndex == 0)
                {
                    voltarRodizio = "true";
                }
                else
                {
                    voltarRodizio = "false";
                }

                if(int.Parse(textBox2.Text) < int.Parse(textBox3.Text))
                {
                    MessageBox.Show("O número de cestas informado foi menor que o número de famílias! Reduzimos o número de famílias para o mesmo número de cestas!");
                    textBox3.Text = textBox2.Text;
                }

                string statement = $"UPDATE settings_entrega SET mesesMax = {textBox1.Text}, voltarRodizio = '{voltarRodizio}', " +
                    $"numCestasPorEntrega = {textBox2.Text}, numFamiliasContempladas = {textBox3.Text} " +
                    $"WHERE id_setting = 1";

                bool atualizou = this.dataBase.ExecutarNonQuery(statement);

                if (atualizou)
                {
                    MessageBox.Show("Opções atualizadas com sucesso!");
                    this.Close();
                }
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

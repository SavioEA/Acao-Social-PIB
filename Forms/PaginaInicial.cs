using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;

namespace Ação_Social_PIB
{
    public partial class PaginaInicial : Form
    {
        Dictionary<string, string> config;

        public PaginaInicial(Dictionary<string, string> config)
        {
            InitializeComponent();
            this.config = config;
        }

        private void buttonCadastro_Click(object sender, EventArgs e)
        {
            NovoCadastro cadastro = new NovoCadastro(config);
            cadastro.ShowDialog();
        }

        private void buttonModificarApagar_Click(object sender, EventArgs e)
        {
            BuscarCadastro modificarApagar = new BuscarCadastro(config, this);
            modificarApagar.ShowDialog();
        }

        private void buttonOpEntrega_Click(object sender, EventArgs e)
        {
            OpcoesEntrega opcoes = new OpcoesEntrega(config);
            opcoes.ShowDialog();
        }

        private void buttonGerarEntrega_Click(object sender, EventArgs e)
        {
            GerarEntrega gerarEntrega = new GerarEntrega(config);
            gerarEntrega.ShowDialog();
        }

        private void buttonRelatorio_Click(object sender, EventArgs e)
        {
            // Cria uma instância do SaveFileDialog
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                // Define as propriedades do SaveFileDialog
                saveDialog.Filter = "Arquivos Excel (*.xlsx)|*.xlsx|Todos os arquivos (*.*)|*.*";
                saveDialog.Title = "Salvar Relatório Excel";
                saveDialog.FileName = $"Relatório_Ação_Social_{DateTime.Now.ToString("ddMMyyyy")}.xlsx"; // Nome padrão do arquivo

                // Abre o diálogo de salvamento e verifica se o usuário clicou em "OK"
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Obtém o caminho completo do arquivo selecionado pelo usuário
                    string filePath = saveDialog.FileName;

                    // Agora você pode usar o 'filePath' para salvar o arquivo Excel
                    // Exemplo: Gere o relatório Excel e salve-o no 'filePath'
                    GerarArquivoExcel(filePath);

                    MessageBox.Show("Relatório salvo com sucesso em: " + filePath);
                }
            }
        }

        private void GerarArquivoExcel(string filePath)
        {
            DataBaseHandler dataBase = new DataBaseHandler(config["DataBasePath"].ToString());

            // Cria um novo arquivo Excel usando o EPPlus
            FileInfo file = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                DataTable relatorioDataTable;
                // Loop pelos últimos 12 meses
                for (int i = 0; i < 12; i++)
                {
                    // Cria uma nova aba com o nome do mês (por exemplo, "Janeiro 2024")
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now.AddMonths(-i).ToString("MMMM yyyy"));

                    string selectStatement = $"SELECT c.nome AS NOME,c.telefone AS TELEFONE,c.endereco AS ENDEREÇO,c.cidade AS CIDADE,c.uf AS UF,ec.quantidadeCestasCadastro AS 'QTD CESTAS' FROM tb_cadastro c JOIN tb_entrega_cadastro ec ON c.id_cadastro = ec.id_cadastro JOIN tb_entrega e ON ec.id_entrega = e.id_entrega WHERE e.dataEntrega  BETWEEN DATE('now', '-{i+1} months') AND DATE('now','-{i} months');";

                    relatorioDataTable = dataBase.BuscarDados(selectStatement);

                    foreach (DataRow row in relatorioDataTable.Rows) 
                    {
                        if(row["TELEFONE"].ToString().Length == 10)
                            row["TELEFONE"] = $"({row["TELEFONE"].ToString().Substring(0,2)}) {row["TELEFONE"].ToString().Substring(2, 4)}-{row["TELEFONE"].ToString().Substring(6)}";
                        else
                            row["TELEFONE"] = $"({row["TELEFONE"].ToString().Substring(0, 2)}) {row["TELEFONE"].ToString().Substring(2, 5)}-{row["TELEFONE"].ToString().Substring(7)}";
                    }

                    worksheet.Cells.LoadFromDataTable(relatorioDataTable, true);

                    // Formatar cabeçalhos de coluna (negrito, cor de fundo)
                    using (ExcelRange headerRange = worksheet.Cells[1, 1, 1, relatorioDataTable.Columns.Count])
                    {
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    }

                    // Ajustar largura das colunas automaticamente
                    worksheet.Cells.AutoFitColumns();

                    relatorioDataTable.Clear();
                }
                // Salva o arquivo Excel
                package.Save();
            }
        }
    }
}

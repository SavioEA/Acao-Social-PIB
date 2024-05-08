using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Entity;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Security.Cryptography;

namespace Ação_Social_PIB
{
    internal class DataBaseHandler
    {
        string connectionString;
        string sqlCreationFilePath;

        public DataBaseHandler(string databasePath)
        {
            this.connectionString = $"Data Source={databasePath};Version=3;foreign keys=true;";
            this.sqlCreationFilePath = "Database/criacaoTabelas.sql";

            if (!File.Exists(databasePath))
            {
                // Se o arquivo não existir, cria o arquivo e executa os comandos SQL do arquivo
                SQLiteConnection.CreateFile(databasePath);
                InitDataBase(databasePath, this.sqlCreationFilePath);
            }
        }

        public bool InserirCadastro(string cpf, string nome, string dataNasc, string telefone, string endereco, string cidade, string uf, int? numPessoas, decimal? rendaTotal = null)
        {
            if (cpf == "" || nome == "" || dataNasc == "" || telefone == "" || endereco == "" || cidade == "" || uf == "" || numPessoas == null)
            {
                MessageBox.Show("Existem dados obrigatórios nulos! Por favor, preencha!");
                return false;
            }

            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                connection.Open();

                string query;

                var cfpCriptografado = CriptografarDado(cpf);

                query = "INSERT INTO tb_cadastro (id_cadastro, nome, dataNasc, telefone, endereco, cidade, uf, numPessoas)" +
                    $"VALUES('{cfpCriptografado}', '{nome}', '{dataNasc}', '{telefone}', '{endereco}', '{cidade}', '{uf}', {numPessoas.ToString()});";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (SQLiteException ex)
                    {
                        if (ex.ErrorCode == (int)SQLiteErrorCode.Constraint && ex.Message.Contains("id_cadastro"))
                        {
                            MessageBox.Show($"Erro: o CPF '{cpf}' já existe na base de dados!");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show($"Erro ao inserir cadastro: {ex.Message}");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocorreu um erro ao salvar os dados: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool AtualizarCadastro(string id, string nome, string dataNasc, string telefone, string endereco, string cidade, string uf, int? numPessoas)
        {
            if (nome == "" || dataNasc == "" || telefone == "" || endereco == "" || cidade == "" || uf == "" || numPessoas == null)
            {
                MessageBox.Show("Existem dados obrigatórios nulos! Por favor, preencha!");
                return false;
            }

            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                connection.Open();

                string statement;

                statement = $"UPDATE tb_cadastro SET nome = '{nome}', dataNasc = '{dataNasc}', telefone = '{telefone}'," +
                    $" endereco = '{endereco}', cidade = '{cidade}', uf = '{uf}', numPessoas = {numPessoas.ToString()}" +
                    $" WHERE id_cadastro = '{id}';";

                using (SQLiteCommand command = new SQLiteCommand(statement, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocorreu um erro ao salvar os dados: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool ApagarCadastro(string id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                connection.Open();

                string query = $"DELETE FROM tb_cadastro WHERE id_cadastro = '{id}'";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocorreu um erro ao tentar excluir o registro: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public DataTable BuscarDados(string selectStatement)
        {
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                connection.Open();

                // Criar um adaptador de dados para preencher um DataTable
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectStatement, connection))
                {
                    DataTable selectDataTable = new DataTable();
                    adapter.Fill(selectDataTable);
                    return selectDataTable;
                }

            }
        }

        public bool ExecutarNonQuery(string nonQueryStatement)
        {
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(nonQueryStatement, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (SQLiteException ex)
                    {
                        if (ex.ErrorCode == (int)SQLiteErrorCode.Constraint)
                        {
                            MessageBox.Show($"Erro: o registro já existe na base de dados!");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show($"Erro ao salvar os dados: {ex.Message}");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocorreu um erro ao salvar os dados: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        private void InitDataBase(string databasePath, string sqlCreationFilePath)
        {
            // Lê todo o conteúdo do arquivo SQL
            string script = File.ReadAllText(sqlCreationFilePath);

            // Divide o script em comandos individuais (delimitados por ponto e vírgula)
            string[] commands = script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Cria uma conexão com o banco de dados SQLite
            using (SQLiteConnection connection = new SQLiteConnection(this.connectionString))
            {
                // Abre a conexão
                connection.Open();

                // Executa cada comando SQL no arquivo
                foreach (string commandText in commands)
                {
                    // Cria um comando SQLite
                    using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                    {
                        // Executa o comando SQL
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        static string CriptografarDado(string dado)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Converter a string para array de bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(dado);

                // Calcular o hash MD5 dos bytes
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Converter o hash MD5 de bytes para uma string hexadecimal
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("X2"));
                }
                return stringBuilder.ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using OfficeOpenXml;

namespace Ação_Social_PIB
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string enviroment = "PROD"; //Define se a base será criada num path do cliente ou do projeto
           
            string configFilePath = "config.json";  // Caminho do arquivo JSON de configuração
            string jsonContent = File.ReadAllText(configFilePath); // Lê o conteúdo do arquivo 
            Dictionary<string, Dictionary<string, string>> Generalconfig = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonContent); // Deserializa o JSON para um dicionário de string para um dicionário de string
            Dictionary<string, string> config = Generalconfig[enviroment];
            if (enviroment == "PROD")
                config["DataBasePath"] = String.Format(config["DataBasePath"].ToString(), Environment.UserName);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PaginaInicial(config));
        }
    }
}

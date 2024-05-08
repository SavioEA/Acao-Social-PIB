namespace Ação_Social_PIB
{
    partial class PaginaInicial
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>


        private System.Windows.Forms.Button btnPagina1;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaginaInicial));
            this.btnPagina1 = new System.Windows.Forms.Button();
            this.buttonCadastro = new System.Windows.Forms.Button();
            this.buttonRelatorio = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonModificarApagar = new System.Windows.Forms.Button();
            this.buttonGerarEntrega = new System.Windows.Forms.Button();
            this.buttonOpEntrega = new System.Windows.Forms.Button();
            this.buttonMudarSenha = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPagina1
            // 
            this.btnPagina1.Location = new System.Drawing.Point(0, 0);
            this.btnPagina1.Name = "btnPagina1";
            this.btnPagina1.Size = new System.Drawing.Size(75, 23);
            this.btnPagina1.TabIndex = 0;
            // 
            // buttonCadastro
            // 
            this.buttonCadastro.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCadastro.Location = new System.Drawing.Point(12, 159);
            this.buttonCadastro.Name = "buttonCadastro";
            this.buttonCadastro.Size = new System.Drawing.Size(370, 92);
            this.buttonCadastro.TabIndex = 0;
            this.buttonCadastro.Text = "Cadastro\n";
            this.buttonCadastro.UseVisualStyleBackColor = true;
            this.buttonCadastro.Click += new System.EventHandler(this.buttonCadastro_Click);
            // 
            // buttonRelatorio
            // 
            this.buttonRelatorio.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRelatorio.Location = new System.Drawing.Point(12, 355);
            this.buttonRelatorio.Name = "buttonRelatorio";
            this.buttonRelatorio.Size = new System.Drawing.Size(370, 92);
            this.buttonRelatorio.TabIndex = 1;
            this.buttonRelatorio.Text = "Relatório\r\n de Entregas";
            this.buttonRelatorio.UseVisualStyleBackColor = true;
            this.buttonRelatorio.Click += new System.EventHandler(this.buttonRelatorio_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(328, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // buttonModificarApagar
            // 
            this.buttonModificarApagar.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonModificarApagar.Location = new System.Drawing.Point(400, 159);
            this.buttonModificarApagar.Name = "buttonModificarApagar";
            this.buttonModificarApagar.Size = new System.Drawing.Size(370, 92);
            this.buttonModificarApagar.TabIndex = 3;
            this.buttonModificarApagar.Text = "Apagar / Modificar \r\nCadastro";
            this.buttonModificarApagar.UseVisualStyleBackColor = true;
            this.buttonModificarApagar.Click += new System.EventHandler(this.buttonModificarApagar_Click);
            // 
            // buttonGerarEntrega
            // 
            this.buttonGerarEntrega.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGerarEntrega.Location = new System.Drawing.Point(400, 257);
            this.buttonGerarEntrega.Name = "buttonGerarEntrega";
            this.buttonGerarEntrega.Size = new System.Drawing.Size(370, 92);
            this.buttonGerarEntrega.TabIndex = 4;
            this.buttonGerarEntrega.Text = "Gerar Entrega";
            this.buttonGerarEntrega.UseVisualStyleBackColor = true;
            this.buttonGerarEntrega.Click += new System.EventHandler(this.buttonGerarEntrega_Click);
            // 
            // buttonOpEntrega
            // 
            this.buttonOpEntrega.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpEntrega.Location = new System.Drawing.Point(12, 257);
            this.buttonOpEntrega.Name = "buttonOpEntrega";
            this.buttonOpEntrega.Size = new System.Drawing.Size(370, 92);
            this.buttonOpEntrega.TabIndex = 5;
            this.buttonOpEntrega.Text = "Opções de Entrega";
            this.buttonOpEntrega.UseVisualStyleBackColor = true;
            this.buttonOpEntrega.Click += new System.EventHandler(this.buttonOpEntrega_Click);
            // 
            // buttonMudarSenha
            // 
            this.buttonMudarSenha.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMudarSenha.Location = new System.Drawing.Point(400, 355);
            this.buttonMudarSenha.Name = "buttonMudarSenha";
            this.buttonMudarSenha.Size = new System.Drawing.Size(370, 92);
            this.buttonMudarSenha.TabIndex = 6;
            this.buttonMudarSenha.Text = "Mudar Senha";
            this.buttonMudarSenha.UseVisualStyleBackColor = true;
            // 
            // PaginaInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(782, 457);
            this.Controls.Add(this.buttonMudarSenha);
            this.Controls.Add(this.buttonOpEntrega);
            this.Controls.Add(this.buttonGerarEntrega);
            this.Controls.Add(this.buttonModificarApagar);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonRelatorio);
            this.Controls.Add(this.buttonCadastro);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PaginaInicial";
            this.Text = "Ação Social PIB";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        //private System.Windows.Forms.Button btnPagina2;

        #endregion

        private System.Windows.Forms.Button buttonCadastro;
        private System.Windows.Forms.Button buttonRelatorio;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonModificarApagar;
        private System.Windows.Forms.Button buttonGerarEntrega;
        private System.Windows.Forms.Button buttonOpEntrega;
        private System.Windows.Forms.Button buttonMudarSenha;
    }
}


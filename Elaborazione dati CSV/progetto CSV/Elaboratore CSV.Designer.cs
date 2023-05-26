using System.Windows.Forms;

namespace Elaborazione_dati_CSV
{
    partial class Elaboratore_CSV
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.GraphicTitle = new System.Windows.Forms.Label();
            this.Lista = new System.Windows.Forms.ListView();
            this.NameList = new System.Windows.Forms.TextBox();
            this.ChiudiFormButton = new System.Windows.Forms.Button();
            this.TotFieldBox = new System.Windows.Forms.TextBox();
            this.MaxLengthBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // GraphicTitle
            // 
            this.GraphicTitle.BackColor = System.Drawing.Color.Black;
            this.GraphicTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.30189F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GraphicTitle.ForeColor = System.Drawing.Color.White;
            this.GraphicTitle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.GraphicTitle.Location = new System.Drawing.Point(424, 9);
            this.GraphicTitle.Name = "GraphicTitle";
            this.GraphicTitle.Size = new System.Drawing.Size(400, 40);
            this.GraphicTitle.TabIndex = 15;
            this.GraphicTitle.Text = "ELABORATORE DATI CSV";
            this.GraphicTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lista
            // 
            this.Lista.AllowColumnReorder = true;
            this.Lista.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lista.FullRowSelect = true;
            this.Lista.GridLines = true;
            this.Lista.HideSelection = false;
            this.Lista.Location = new System.Drawing.Point(830, 75);
            this.Lista.Name = "Lista";
            this.Lista.Size = new System.Drawing.Size(930, 870);
            this.Lista.TabIndex = 16;
            this.Lista.UseCompatibleStateImageBehavior = false;
            this.Lista.View = System.Windows.Forms.View.Details;
            // 
            // NameList
            // 
            this.NameList.BackColor = System.Drawing.Color.White;
            this.NameList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NameList.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.NameList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameList.ForeColor = System.Drawing.Color.Black;
            this.NameList.Location = new System.Drawing.Point(830, 46);
            this.NameList.MaxLength = 100;
            this.NameList.Name = "NameList";
            this.NameList.ReadOnly = true;
            this.NameList.Size = new System.Drawing.Size(930, 23);
            this.NameList.TabIndex = 25;
            this.NameList.Text = "Non è stata selezionata nessuna linea";
            // 
            // ChiudiFormButton
            // 
            this.ChiudiFormButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ChiudiFormButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChiudiFormButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChiudiFormButton.Location = new System.Drawing.Point(1758, 1);
            this.ChiudiFormButton.Name = "ChiudiFormButton";
            this.ChiudiFormButton.Size = new System.Drawing.Size(25, 25);
            this.ChiudiFormButton.TabIndex = 27;
            this.ChiudiFormButton.UseVisualStyleBackColor = false;
            // 
            // TotFieldBox
            // 
            this.TotFieldBox.BackColor = System.Drawing.Color.White;
            this.TotFieldBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TotFieldBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TotFieldBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotFieldBox.ForeColor = System.Drawing.Color.Black;
            this.TotFieldBox.Location = new System.Drawing.Point(566, 75);
            this.TotFieldBox.MaxLength = 100;
            this.TotFieldBox.Name = "TotFieldBox";
            this.TotFieldBox.ReadOnly = true;
            this.TotFieldBox.Size = new System.Drawing.Size(258, 23);
            this.TotFieldBox.TabIndex = 28;
            this.TotFieldBox.Text = "Numero di campi: ";
            // 
            // MaxLengthBox
            // 
            this.MaxLengthBox.BackColor = System.Drawing.Color.White;
            this.MaxLengthBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MaxLengthBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MaxLengthBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxLengthBox.ForeColor = System.Drawing.Color.Black;
            this.MaxLengthBox.Location = new System.Drawing.Point(566, 104);
            this.MaxLengthBox.MaxLength = 100;
            this.MaxLengthBox.Name = "MaxLengthBox";
            this.MaxLengthBox.ReadOnly = true;
            this.MaxLengthBox.Size = new System.Drawing.Size(258, 23);
            this.MaxLengthBox.TabIndex = 29;
            this.MaxLengthBox.Text = "lunghezza massima dei record: ";
            // 
            // Elaboratore_CSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1784, 959);
            this.Controls.Add(this.MaxLengthBox);
            this.Controls.Add(this.TotFieldBox);
            this.Controls.Add(this.ChiudiFormButton);
            this.Controls.Add(this.NameList);
            this.Controls.Add(this.Lista);
            this.Controls.Add(this.GraphicTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Elaboratore_CSV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Lista CRUD";
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Shortcut);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label GraphicTitle;
        private System.Windows.Forms.ListView Lista;
        private System.Windows.Forms.TextBox NameList;
        private System.Windows.Forms.Button ChiudiFormButton;
        private TextBox TotFieldBox;
        private TextBox MaxLengthBox;
    }
}


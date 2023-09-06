using System;
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
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Elaboratore_CSV));
			this.GraphicTitle = new System.Windows.Forms.Label();
			this.Lista = new System.Windows.Forms.ListView();
			this.NameList = new System.Windows.Forms.TextBox();
			this.TotFieldBox = new System.Windows.Forms.TextBox();
			this.MaxLengthBox = new System.Windows.Forms.TextBox();
			this.SearchBox = new System.Windows.Forms.TextBox();
			this.SearchLabel = new System.Windows.Forms.Label();
			this.FieldLengthButton = new System.Windows.Forms.Button();
			this.FieldLengthBox = new System.Windows.Forms.TextBox();
			this.AddBox = new System.Windows.Forms.TextBox();
			this.AddLabel = new System.Windows.Forms.Label();
			this.AddButton = new System.Windows.Forms.Button();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.labResearch = new System.Windows.Forms.Label();
			this.BtnSearch = new System.Windows.Forms.Button();
			this.BtnReload = new System.Windows.Forms.Button();
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
			this.NameList.Size = new System.Drawing.Size(930, 24);
			this.NameList.TabIndex = 25;
			this.NameList.Text = "Non è stata selezionata nessuna linea";
			this.NameList.Visible = false;
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
			this.TotFieldBox.Size = new System.Drawing.Size(258, 24);
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
			this.MaxLengthBox.Size = new System.Drawing.Size(258, 24);
			this.MaxLengthBox.TabIndex = 29;
			this.MaxLengthBox.Text = "lunghezza massima dei record: ";
			// 
			// SearchBox
			// 
			this.SearchBox.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13"});
			this.SearchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.SearchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.SearchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SearchBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.SearchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SearchBox.Location = new System.Drawing.Point(12, 101);
			this.SearchBox.MaxLength = 20;
			this.SearchBox.Name = "SearchBox";
			this.SearchBox.Size = new System.Drawing.Size(125, 24);
			this.SearchBox.TabIndex = 4;
			// 
			// SearchLabel
			// 
			this.SearchLabel.AutoSize = true;
			this.SearchLabel.Location = new System.Drawing.Point(16, 85);
			this.SearchLabel.Name = "SearchLabel";
			this.SearchLabel.Size = new System.Drawing.Size(41, 13);
			this.SearchLabel.TabIndex = 10;
			this.SearchLabel.Text = "Search";
			// 
			// FieldLengthButton
			// 
			this.FieldLengthButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.FieldLengthButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.FieldLengthButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FieldLengthButton.Location = new System.Drawing.Point(12, 146);
			this.FieldLengthButton.Name = "FieldLengthButton";
			this.FieldLengthButton.Size = new System.Drawing.Size(120, 47);
			this.FieldLengthButton.TabIndex = 7;
			this.FieldLengthButton.Text = "Get Field Length";
			this.FieldLengthButton.UseVisualStyleBackColor = false;
			this.FieldLengthButton.Click += new System.EventHandler(this.FieldLengthButton_Click);
			// 
			// FieldLengthBox
			// 
			this.FieldLengthBox.BackColor = System.Drawing.Color.White;
			this.FieldLengthBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.FieldLengthBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.FieldLengthBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FieldLengthBox.ForeColor = System.Drawing.Color.Black;
			this.FieldLengthBox.Location = new System.Drawing.Point(143, 101);
			this.FieldLengthBox.MaxLength = 100;
			this.FieldLengthBox.Name = "FieldLengthBox";
			this.FieldLengthBox.ReadOnly = true;
			this.FieldLengthBox.Size = new System.Drawing.Size(34, 24);
			this.FieldLengthBox.TabIndex = 30;
			// 
			// AddBox
			// 
			this.AddBox.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13"});
			this.AddBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.AddBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.AddBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.AddBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.AddBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AddBox.Location = new System.Drawing.Point(12, 247);
			this.AddBox.MaxLength = 20;
			this.AddBox.Name = "AddBox";
			this.AddBox.Size = new System.Drawing.Size(125, 24);
			this.AddBox.TabIndex = 31;
			// 
			// AddLabel
			// 
			this.AddLabel.AutoSize = true;
			this.AddLabel.Location = new System.Drawing.Point(16, 231);
			this.AddLabel.Name = "AddLabel";
			this.AddLabel.Size = new System.Drawing.Size(26, 13);
			this.AddLabel.TabIndex = 33;
			this.AddLabel.Text = "Add";
			// 
			// AddButton
			// 
			this.AddButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.AddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AddButton.Location = new System.Drawing.Point(12, 292);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(120, 47);
			this.AddButton.TabIndex = 32;
			this.AddButton.Text = "Add";
			this.AddButton.UseVisualStyleBackColor = false;
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			// 
			// txtSearch
			// 
			this.txtSearch.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13"});
			this.txtSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSearch.Location = new System.Drawing.Point(297, 247);
			this.txtSearch.MaxLength = 20;
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(164, 24);
			this.txtSearch.TabIndex = 34;
			// 
			// labResearch
			// 
			this.labResearch.AutoSize = true;
			this.labResearch.Location = new System.Drawing.Point(301, 231);
			this.labResearch.Name = "labResearch";
			this.labResearch.Size = new System.Drawing.Size(53, 13);
			this.labResearch.TabIndex = 36;
			this.labResearch.Text = "Research";
			// 
			// BtnSearch
			// 
			this.BtnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.BtnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnSearch.Location = new System.Drawing.Point(297, 292);
			this.BtnSearch.Name = "BtnSearch";
			this.BtnSearch.Size = new System.Drawing.Size(111, 47);
			this.BtnSearch.TabIndex = 35;
			this.BtnSearch.Text = "Search";
			this.BtnSearch.UseVisualStyleBackColor = false;
			this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
			// 
			// BtnReload
			// 
			this.BtnReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.BtnReload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnReload.BackgroundImage")));
			this.BtnReload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.BtnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnReload.Location = new System.Drawing.Point(414, 292);
			this.BtnReload.Name = "BtnReload";
			this.BtnReload.Size = new System.Drawing.Size(47, 47);
			this.BtnReload.TabIndex = 37;
			this.BtnReload.UseVisualStyleBackColor = false;
			// 
			// Elaboratore_CSV
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(1784, 959);
			this.Controls.Add(this.BtnReload);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.labResearch);
			this.Controls.Add(this.BtnSearch);
			this.Controls.Add(this.AddBox);
			this.Controls.Add(this.AddLabel);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this.FieldLengthBox);
			this.Controls.Add(this.MaxLengthBox);
			this.Controls.Add(this.TotFieldBox);
			this.Controls.Add(this.SearchBox);
			this.Controls.Add(this.SearchLabel);
			this.Controls.Add(this.FieldLengthButton);
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

		private System.Windows.Forms.Label GraphicTitle;
		private System.Windows.Forms.ListView Lista;
		private System.Windows.Forms.TextBox NameList;
		private TextBox TotFieldBox;
		private TextBox MaxLengthBox;
		private System.Windows.Forms.TextBox SearchBox;
		private System.Windows.Forms.Label SearchLabel;
		private System.Windows.Forms.Button FieldLengthButton;
		private TextBox FieldLengthBox;
		private TextBox AddBox;
		private Label AddLabel;
		private Button AddButton;
		private TextBox txtSearch;
		private Label labResearch;
		private Button BtnSearch;
		private Button BtnReload;
	}
}


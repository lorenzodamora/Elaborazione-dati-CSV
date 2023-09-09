using System;
/*
 * using System.Collections.Generic;
 * using System.Drawing;
 * using System.Dynamic;
 * using System.Net.NetworkInformation;
 * using System.ComponentModel;
 * using System.Data;
 * using System.Linq; //ha .ToArray();
 * using System.Net.Http;
 * using System.Threading;
 */
//using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text;
//using Elaboratore_CSV_Libreria;
using static Elaboratore_CSV_Libreria.Gestore_Lines;
using static Elaboratore_CSV_Libreria.AutoStartedFun;
using static Elaboratore_CSV_Libreria.InternalFun;

namespace Elaborazione_dati_CSV_ripasso_pre_rientro
{
	public partial class Elaboratore_CSV : Form
	{
		/*
		 * 1x Implementare le funzionalità richieste nell'esercitazione "Elaborazione dati CSV", utilizzando esclusivamente l'accesso diretto al file.
		 * 2x Il programma non dovrà fare uso di array di record
		 * 3x Sviluppare le funzioni di servizio creando una libreria dll, della quale deve essere pubblicato anche il codice.
		 * 4x Sviluppare sia la versione con form, che la versione console, sfruttando la stessa libreria dll.
		 * 5 Implementare, sia la versione in c#, che in c++(no dll, ne interfaccia grafica).
		 * 
		 * Extra: Pubblicare il link del nuovo repository prima possibile, rendendolo pubblico a partire dalla data di scadenza ed effettuare un commit/push per ogni funzionalità richiesta.
		*/

		//divisione in pagine?? (40 linee per pagina) ??

		public string path;
		public int totfield, max, fdi, sel; //totale campi (tranne logic remove) //max length // fdi fixed dim //sel selected line
		public short[] lines; //associa le linee del file a quelle visibili nella listview // l'indice è la linea nel file, mentre il valore è l'indice nella listview // indice 0 headers // se è -1 allora non è presente in listview. se è 0 allora non esiste.
		ColumnHeader[] ch;
		public Elaboratore_CSV()
		{
			InitializeComponent();
			path = Path.GetFullPath("..\\..\\..") + "\\files\\damora.csv";
			fdi = 0;

			//continua in shown
		}

		/* funzioni non utilizzate *
		private string[] FileReadAllLines(string path)
		{
			byte[] b = new byte[1024];
			UTF8Encoding temp = new UTF8Encoding(true);
			string line = "";
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
			for(int l; (l = fs.Read(b, 0, b.Length)) > 0;)
				line += temp.GetString(b, 0, l);
			fs.Close();

			if(line == "") return new string[0];
			string[] lines;
			if(line[line.Length - 1] == '\n')
				// .SubString() perché altrimenti per ultimo rimarrebbe una stringa vuota
				lines = line.Substring(0, line.Length - 1).Split('\n');
			else
				lines = line.Split('\n');
			for(int i = 0; i < lines.Length; i++)
				lines[i] = lines[i].TrimEnd('\r');
			return lines;
		}
		private void FileWriteAllText(string path, string allLines, FileMode mode)
		{
			Byte[] info = new UTF8Encoding(true).GetBytes(allLines);
			FileStream fs = new FileStream(path, mode, FileAccess.Write, FileShare.None);
			fs.Write(info, 0, info.Length);
			fs.Close();
		}
		private void FileWriteAllText(string path, string allLines)
		{
			FileWriteAllText(path, allLines, FileMode.Create);
		}
		private void FileWriteAllLines(string path, string[] lines)
		{
			FileWriteAllLines(path, lines, FileMode.Create);
		}
		private void FileWriteAllLines(string path, string[] lines, FileMode mode)
		{
			FileWriteAllText(path, string.Join("\n", lines), mode);
		} 
		private void ArrayResize(ref string[] array, int newSize)
		{
			if(array.Length != newSize)
			{
				string[] array2 = new string[newSize];
				if(array.Length < newSize) //se newsize è più grande, copia fino ad array.length e il resto rimane default
					for(int i = 0; i < array.Length; i++)
						array2[i] = array[i];
				else //se newsize è più piccolo copia fino a newsize
					for(int i = 0; i < newSize; i++)
						array2[i] = array[i];
				array = array2;
			}
		} */

		private void StampaCSV(ref ColumnHeader[] ch, int fdi, string path, bool headers = false)
		{
			Lista.Items.Clear();
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea tranne \r\n
				UTF8Encoding enc = new UTF8Encoding(true);
				fs.Read(b, 0, fdi); //legge una linea
				string[] split = enc.GetString(b).TrimEnd().Split(';');
				if(headers)
				{
					ch = CreaHeaders(split);
					Lista.Columns.AddRange(ch);
				}
				fs.Position+=2; // \r\n

				ListViewItem item;
				string line;
				for(short ind = 1; fs.Read(b, 0, fdi) > 0; fs.Position+=2) //ind line index
				{
					line = enc.GetString(b).TrimEnd(); //gestione fixed dim
					if(line.EndsWith("1")) continue; //skippa le linee cancellate.
					
					split = line.Split(';');
					item = new ListViewItem($"{ind++}");
					for(int j = 0; j < split.Length-1; j++) //salta logic
						item.SubItems.Add(split[j]); //sub item
					Lista.Items.Add(item);
				}
				for(int i = 0; i < split.Length; i++)
					ch[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}
		private ColumnHeader[] CreaHeaders(string[] split)
		{
			ColumnHeader[] ch = new ColumnHeader[split.Length];
			ch[0] = new ColumnHeader
			{
				Text = "line",
				Width = -2,
				TextAlign = HorizontalAlignment.Center
			};
			for(int i = 0; i < split.Length-1; i++) //salta logic
				ch[i+1] = new ColumnHeader
				{
					Text = split[i],
					Width = -2,
					TextAlign = HorizontalAlignment.Center
				};

			return ch;
		}

		private void Form_Shown(object sender, EventArgs e)
		{
			//SetVisible();
			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max+2, path);

			if(AddCampo(fdi, path))
			{
				max = GetMaxLength(path);
				fdi = FixedDim(fdi, max+2, path);
			}

			// Un campo è il logic remove.
			totfield = GetTotFields(fdi, path) - 1;

			TotFieldBox.Text = "Numero di campi: " + (totfield);
			MaxLengthBox.Text = "lunghezza massima dei record: " + max;

			lines = new short[GetLinesLength(TrovaTotLinee(fdi, path))];
			ResetLines(lines, fdi, path);
			StampaCSV(ref ch, fdi, path, true);
		}
		//private void Shortcut(object sender, KeyEventArgs e)

		private void FieldLengthButton_Click(object sender, EventArgs e)
		{
			FieldLengthBox.Text = GetMaxLength(SearchBox.Text, totfield, fdi, path).ToString();
		}
		private void AddButton_Click(object sender, EventArgs e)
		{
			//check lunghezza testo non fatta. //??
			if(AddLine(AddBox.Text, totfield, fdi, path, true)) return;
			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max+2, path);
			MaxLengthBox.Text = "lunghezza massima dei record: " + max;

			/*
			ListViewItem line;
			string[] splits = AddBox.Text.Split(';');
			line = new ListViewItem($"{Lista.Items.Count + 1}");//il main item è l'indice linea
			for(int j = 0; j < splits.Length; j++)
				line.SubItems.Add(splits[j]); //sub item
			Lista.Items.Add(line);

			for(int i = 0; i < ch.Length; i++)
				ch[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			*/
			BtnReload_Click(sender, e);
			CheckLines(ref lines, Lista.Items.Count);
		}
		private void BtnSearch_Click(object sender, EventArgs e)
		{
			if(txtSearch.Text != "")
			{
				Deselect();
				string tpath = Path.GetDirectoryName(path) + "\\temp.csv";
				if(ResearchLines(txtSearch.Text, fdi, path, lines))
					MessageBox.Show("Nessun risultato trovato.\n\ntip:\nIl punto e virgola (;) è il divisore che fa cercare due parole diverse nella stessa linea", "errore nella ricerca");
				else
					StampaCSV(ref ch, fdi, tpath);
				File.Delete(tpath);
			}
			else
				MessageBox.Show("Digita qualcosa nella barra di Research per cercare.\n\ntip:\nIl punto e virgola (;) è il divisore che fa cercare due parole diverse nella stessa linea", "errore nella ricerca");
		}
		private void BtnReload_Click(object sender, EventArgs e)
		{
			Deselect();
			ResetLines(lines, fdi, path);
			StampaCSV(ref ch, fdi, path);
		}
		private void BtnSelect_Click(object sender, EventArgs e)
		{
			if(txtSelect.Text != "")
			{
				if(txtSelect.Text == "0")
				{
					Deselect();
					return;
				}
				if(SelectLine(txtSelect.Text, Lista.Items.Count))
				{
					sel = int.Parse(txtSelect.Text);
					//l'indice è -1 rispetto alla linea
					NameList.Text = $"Stai modificando l'elemento {sel--}.";
					//ListViewItem item = Lista.Items[sel]; //txtEdit.Text = item.Text;
					txtEdit.Text = "";
					for(int i = 1; i < Lista.Items[sel].SubItems.Count; i++) txtEdit.Text += Lista.Items[sel].SubItems[i].Text + ";"; //salto line
					txtEdit.Text = txtEdit.Text.Remove(txtEdit.Text.Length - 1);
					labEdit.Visible = txtEdit.Visible = BtnEdit.Visible = BtnDelete.Visible = true;
				}
			}
			else
				MessageBox.Show("Digita qualcosa nella barra di Select per selezionare un elemento da modificare o eliminare.\n\ntip:\nDigita '0' per deselezionare.", "errore nella selezione");
		}
		private void BtnEdit_Click(object sender, EventArgs e)
		{
			if(txtEdit.Text.TrimEnd(' ', ';') != "")
			{
				if(txtEdit.Text.Length < fdi-2)
				{
					//foreach(ListViewItem.ListViewSubItem sub in Lista.Items[sel].SubItems) old += sub.Text + ";";
					//string old = "";
					//for(int i = 1; i < Lista.Items[sel].SubItems.Count; i++) old += Lista.Items[sel].SubItems[i].Text + ";";
					//old = old.Remove(old.Length - 1);
					if(EditLine(txtEdit.Text, totfield, fdi, TrovaSelezionato(lines, (short)(sel+1)), path)) return;
					max = GetMaxLength(path);
					fdi = FixedDim(fdi, max+2, path);
					MaxLengthBox.Text = "lunghezza massima dei record: " + max;

					ListViewItem line;
					string[] splits = txtEdit.Text.Split(';');
					line = new ListViewItem($"{sel+1}"); //il main item è l'indice linea
					for(int j = 0; j < splits.Length; j++)
						line.SubItems.Add(splits[j]); //sub item
					Lista.Items[sel] = line;

					for(int i = 0; i < ch.Length; i++)
						ch[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
				}
				else
					MessageBox.Show($"L'elemento modificato è troppo lungo. (max: {fdi-2}).\n\ntip:\nIl punto e virgola (;) è il divisore che divide i campi nello stesso elemento", "errore nella modifica");
			}
			else
				MessageBox.Show("Digita qualcosa nella barra di Edit per modificare l'elemento selezionato.\n\ntip:\nIl punto e virgola (;) è il divisore che divide i campi nello stesso elemento", "errore nella modifica");
		}
		private void BtnDelete_Click(object sender, EventArgs e)
		{
			DeleteLine(TrovaSelezionato(lines, (short)(sel+1)), fdi, path);
			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max+2, path);
			MaxLengthBox.Text = "lunghezza massima dei record: " + max;

			BtnReload_Click(sender, e);
			CheckLines(ref lines, Lista.Items.Count);
		}

		private void Deselect()
		{
			sel = 0;
			NameList.Text = "Non è stato selezionato nessun elemento.";
			labEdit.Visible = txtEdit.Visible = BtnEdit.Visible = BtnDelete.Visible = false;

		}

	}
}

using System;
/* using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Data;
using System.Linq; //ha .ToArray();
using System.Net.Http;
using System.Threading; */
//using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace Elaborazione_dati_CSV
{
	public partial class Elaboratore_CSV : Form
	{
		//x 1. Aggiungere in coda ad ogni record un campo chiamato miovalore contenente un numero casuale 10<=X<=20 + un campo per la cancellazione logica
		//x 2. contare il numero dei campi che compongono il record
		//x 3. calcolare la lunghezza massima dei record presenti indicando anche la lunghezza massima di ogni campo
		//x 4. inserire in ogni record un numero di spazi necessari a rendere fissa la dimensione di tutti i record, senza perdere informazioni
		//x 5. Aggiungere un record in coda
		//n 6. Visualizzare dei dati mostrando tre campi significativi a scelta
		//x 7. Ricercare un record per campo chiave a scelta (se esiste, utilizzare il campo che contiene dati univoci)
		//wip 8. Modificare un record
		//9. Cancellare logicamente un record
		//wip 10. Realizzare l'interfaccia grafica che consenta l'interazione fluida con le funzionalità descritte. Richiamare le funzioni di servizio dalle funzioni di gestione degli eventi

		//x accesso diretto
		//divisione in pagine (42 linee per pagina)

		public string path;
		public int totfield, max, fdi, sel; //totale campi (tranne logic remove) //max length // fdi fixed dim //sel selected line
		public short[] lines; //associa le linee del file a quelle visibili nella listview // l'indice è la linea nel file, mentre il valore è l'indice nella listview // indice 0 headers // se è -1 allora non è presente in listview.
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

		private int GetLinesLength(int tot)
		{
			if(tot < 100) return 100;
			return tot/100 * 100 + 100;
		}
		private void CheckLines(ref short[] lines, int sum)
		{
			int newSize = GetLinesLength(lines.Length + sum);
			if(newSize != lines.Length)
				LinesResize(ref lines, newSize);
		}
		private void LinesResize(ref short[] array, int newSize)
		{
			if(array.Length != newSize)
			{
				short[] array2 = new short[newSize];
				if(array.Length < newSize) //se newsize è più grande, copia fino ad array.length e il resto rimane default
					for(int i = 0; i < array.Length; i++)
						array2[i] = array[i];
				else //se newsize è più piccolo copia fino a newsize
					for(int i = 0; i < newSize; i++)
						array2[i] = array[i];
				array = array2;
			}
		}
		private void ResetLines(short[] lines)
		{
			for(short i = 0; i < lines.Length; i++) lines[i] = i;
		}
		private short TrovaSelezionato(short[] lines, short sel)
		{
			for(short i = 1; i < lines.Length; i++) if(lines[i] == sel) return i;
			return -1;
		}

		private void StampaCSV(ref ColumnHeader[] ch, int fdi, string path, bool headers = false)
		{
			Lista.Items.Clear();
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea
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
				for(int i = 1; fs.Read(b, 0, fdi) > 0; i++) //i line index
				{
					split = enc.GetString(b).TrimEnd().Split(';'); //gestione fixed dim
					item = new ListViewItem($"{i}");
					for(int j = 0; j < split.Length-1; j++)
						item.SubItems.Add(split[j]); //sub item
					Lista.Items.Add(item);
					fs.Position+=2;
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
			for(int i = 0; i < split.Length-1; i++)
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
			fdi = FixedDim(fdi, max, path);

			if(AddCampo(fdi, path))
			{
				max = GetMaxLength(path);
				fdi = FixedDim(fdi, max, path);
			}

			// Un campo è il logic remove.
			totfield = GetTotFields(fdi, path) - 1;

			TotFieldBox.Text = "Numero di campi: " + (totfield);
			MaxLengthBox.Text = "lunghezza massima dei record: " + max;
			StampaCSV(ref ch, fdi, path, true);

			lines = new short[GetLinesLength(Lista.Items.Count)];
			ResetLines(lines);
		}
		private void Shortcut(object sender, KeyEventArgs e)
		{

		}

		private void FieldLengthButton_Click(object sender, EventArgs e)
		{
			FieldLengthBox.Text = GetMaxLength(SearchBox.Text, totfield, fdi, path).ToString();
		}
		private void AddButton_Click(object sender, EventArgs e)
		{
			//check lunghezza testo non fatta. //??
			if(AddLine(AddBox.Text, totfield, fdi, path, true)) return;
			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max, path);
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
			CheckLines(ref lines, 1);
			BtnReload_Click(sender, e);
		}
		private void BtnSearch_Click(object sender, EventArgs e)
		{
			if(txtSearch.Text != "")
			{
				Deselect();
				if(ResearchLines(txtSearch.Text, fdi, path, lines, ch))
					MessageBox.Show("Nessun risultato trovato.\n\ntip:\nIl punto e virgola (;) è il divisore che fa cercare due parole diverse nella stessa linea", "errore nella ricerca");
			}
			else
				MessageBox.Show("Digita qualcosa nella barra di Research per cercare.\n\ntip:\nIl punto e virgola (;) è il divisore che fa cercare due parole diverse nella stessa linea", "errore nella ricerca");
		}
		private void BtnReload_Click(object sender, EventArgs e)
		{
			Deselect();
			ResetLines(lines);
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
					fdi = FixedDim(fdi, max, path);
					MaxLengthBox.Text = "lunghezza massima dei record: " + max;

					ListViewItem line;
					string[] splits = txtEdit.Text.Split(';');
					line = new ListViewItem($"{sel + 1}"); //il main item è l'indice linea
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


			CheckLines(ref lines, -1);
		}

		/**
		 * <summary>
		 * Trova la linea più lunga. (Rimuove il fixed dim se presente.)
		 * </summary>
		 * <returns>
		 * La lunghezza della linea.
		 * </returns>
		 */
		private int GetMaxLength(string path)
		{
			int max = 0;
			int len;
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				int b;
				string line = "";
				while((b = fs.ReadByte()) > 0)
					if((char)b == '\n')
					{
						len = line.TrimEnd(' ', '\r').Length;
						if(len > max) max = len;
						line = "";
					}
					else
						line += (char)b;
			}
			return max;
		}
		/**
		 * <summary>
		 * Ricalcola il fixed dim e lo applica se diverso.
		 * </summary>
		 * <param name="fdi">è l'attuale fixed dim ( compreso testo ).</param>
		 * <param name="max">è la lunghezza solo del testo.</param>
		 * <returns>
		 * La dimensione fissa di ogni linea.
		 * </returns>
		 */
		private int FixedDim(int fdi, int max, string path)
		{
			int nfdi; //nuovo fdi
			if(max < 100) nfdi = 200; else nfdi = max/100*100 + 200;
			//se il testo è meno di 100 char allora fixed dim = 200; altrimenti arrotonda max a multipli di 100 e ci aggiunge 200

			if(fdi != nfdi)
			{
				string tpath = Path.GetDirectoryName(path) + "\\temp.csv";
				File.Copy(path, tpath, true);
				using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.None))
				{
					using(FileStream temp = new FileStream(tpath, FileMode.Open, FileAccess.Read, FileShare.None))
					{
						UTF8Encoding enc = new UTF8Encoding(true);
						int b;
						string line = "";
						while((b = temp.ReadByte()) > 0) //legge un byte e lo scrive in b, read restituisce -1 se il flusso(file) finisce
							if((char)b == '\n') //se ha letto /n
							{
								//TrimEnd() //toglie il vecchio fixed dim se presente
								Byte[] info = enc.GetBytes(line.TrimEnd(' ', '\r').PadRight(nfdi) + "\r\n");
								fs.Write(info, 0, info.Length); //scrive e posiziona il cursore
								line = "";
							}
							else // se non va a capo compila la linea
								line += (char)b;

						//se la lunghezza del file è minore di prima, cancella il resto
						fs.SetLength(fs.Position);
					}
					File.Delete(tpath);
				}
			}
			return nfdi;
		}
		/**
		 * <summary>
		 * Controlla la presenza del campo "miovalore", se manca lo aggiunge.
		 * </summary>
		 * <returns>
		 * <strong>true</strong> se aggiunge il campo, else <strong>false</strong>.
		 * </returns>
		 */
		private bool AddCampo(int fdi, string path)
		{
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea escluso \r\n
				UTF8Encoding enc = new UTF8Encoding(true);
				fs.Read(b, 0, fdi); //legge una linea
				string line = enc.GetString(b).TrimEnd();

				string[] split = line.Split(';');
				for(int i = 0; i < split.Length; i++)
					if(split[i] == "miovalore") return false;

				fs.Position = enc.GetBytes(line).Length; //posiziona alla fine del testo senza fdi

				byte[] info = enc.GetBytes(";miovalore;logic");
				fs.Write(info, 0, info.Length); //con fixed dim scrive sugli spazi

				fs.Position = fdi+2; // \r\n
				for(int lin = 2, pos; fs.Read(b, 0, fdi) > 0; lin++) //lin line index
				{
					line = enc.GetString(b);
					pos = enc.GetBytes(line.TrimEnd()).Length;
					fs.Position = fs.Position - fdi + pos; //position si trova a fine riga escluso \r\n, ci tolgo il fixed dim e ci aggiungo la lunghezza del testo

					//Random rnd = new Random(lin * Environment.TickCount); //può essere che un millisecondo nel ciclo non abbia il tempo di trascorrere
					line = $";{new Random(lin * Environment.TickCount).Next(10, 20 + 1)};0";
					info = enc.GetBytes(line);
					fs.Write(info, 0, info.Length); //con fixed dim scrive sugli spazi

					fs.Position = (fdi+2) * lin; //si posiziona a inizio riga dopo
				}
			}
			return true;
		}
		/**
		 * <summary>
		 * Ottiene il numero di campi presenti nella prima linea del csv.
		 * </summary>
		 */
		private int GetTotFields(int fdi, string path)
		{
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea
				fs.Read(b, 0, fdi); //legge una linea
				return new UTF8Encoding(true).GetString(b).Split(';').Length;
			}
		}

		private bool CheckField(string field, int totField) //ritorna true se ci sono errori
		{
			if(!int.TryParse(field, out int fie) || fie < 1)
			{//bad input
				MessageBox.Show("numero intero positivo", "errore nella selezione");
				return true;
			}
			if(fie > totField)
			{//bad input
				MessageBox.Show("inserisci un campo esistente", "errore nella selezione");
				return true;
			}
			return false;
		}
		private bool CheckField(int count, int totField) //ritorna true se ci sono errori
		{
			if(count == 0)
			{
				MessageBox.Show("per inserire in campi diversi separa con ';'", "errore in input");
				return true;
			}
			if(count > totField-1) //campo line
			{
				MessageBox.Show($"hai inserito troppi campi (max:{totField})", "errore in input");
				return true;
			}
			return false;
		}
		private bool CheckSelect(string check, int count)
		{
			if(!int.TryParse(check, out int ind) || ind < 1) //ind = indice linea da selezionare
			{//bad input
				MessageBox.Show("inserisci un intero positivo", "errore nella selezione");
				return false;
			}
			if(ind > count)
			{//bad input
				MessageBox.Show($"inserisci un indice che appare in lista (max: {count})", "errore nella selezione");
				return false;
			}
			return true; //ret false = la stringa non è valida
		}

		private int GetMaxLength(string field, int totField, int fdi, string path)
		{
			if(CheckField(field, totField)) //bad input
				return -1;

			int max = 0;
			int len;
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea escluso \n
				UTF8Encoding enc = new UTF8Encoding(true);
				fs.Position+= fdi + 2; // skippa gli header e \r\n
				while(fs.Read(b, 0, fdi)>0) //legge una linea
				{
					len = enc.GetString(b).TrimEnd().Split(';')[int.Parse(field)-1].Length;
					if(len > max) max = len;
					fs.Position+=2; // \r\n
				}
			}
			return max;
		}
		/**
* <summary>
* (append false non gestito).
* </summary>
* <returns>
* True se c'è errore.
* </returns>
*/
		private bool AddLine(string add, int totField, int fdi, string path, bool append = false)
		{
			int c = 0;
			for(int i = 0; i < add.Length; i++) if(add[i] == ';') c++;
			if(CheckField(c, totField)) return true; //c'è errore

			for(; c < totField; c++) add += ";";

			add = (add+"0").PadRight(fdi); //gestione fixed dim
			Byte[] info = new UTF8Encoding(true).GetBytes(add + "\r\n");

			if(append)
				using(FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None))
				{
					//fs.Position = fs.Length; //length - 1 è \n
					fs.Write(info, 0, info.Length);
				}
			//else //append false;

			return false;
		}
		private bool ResearchLines(string search, int fdi, string path, short[] lines, ColumnHeader[] ch)
		{
			string tpath = Path.GetDirectoryName(path) + "\\temp.csv";
			bool empty = true;
			string[] searchSplit = search.TrimEnd().ToLower().Split(';');

			byte[] bLine = new byte[fdi+2]; //lunghezza una linea + \r\n
			UTF8Encoding enc = new UTF8Encoding(true);

			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				using(FileStream temp = new FileStream(tpath, FileMode.Create))
				{
					fs.Read(bLine, 0, fdi+2); //legge una linea
					temp.Write(bLine, 0, fdi+2); //scrive e posiziona il cursore

					string searching = "";
					short j = 1;
					for(short i = 1; fs.Read(bLine, 0, fdi+2) > 0; i++)
					{
						//rimuove anche il logic.
						searching = enc.GetString(bLine).TrimEnd().TrimEnd('0', '1').ToLower();
						/*
						///Counter. quando arriva a 0 = la linea ha tutte le parole cercate.
						int c = searchSplit.Length; for(; c != 0; c--) if(!searching.Contains(searchSplit[c-1])) break;
						if(c == 0) { temp.Write(bLine, 0, fdi+2); empty = false; }
						*/
						/// Controllo. Controlla se la linea ha tutte le parole cercate.
						bool c = true;
						foreach(string str in searchSplit)
							if(!searching.Contains(str))
							{
								c = false;
								lines[i] = -1;
								break; // Interrompe se una parola cercata non è presente
							}
						if(c)
						{
							lines[i] = j++;
							temp.Write(bLine, 0, fdi+2);
							empty = false;
						}
					}
				}
			}
			if(!empty) StampaCSV(ref ch, fdi, tpath);
			File.Delete(tpath);
			return empty;
		}
		private bool SelectLine(string ind, int count)
		{
			return CheckSelect(ind, count);
		}
		private void Deselect()
		{
			sel = 0;
			NameList.Text = "Non è stato selezionato nessun elemento.";
			labEdit.Visible = txtEdit.Visible = BtnEdit.Visible = BtnDelete.Visible = false;

		}
		private bool EditLine(string edit, int totField, int fdi, short select, string path)
		{
			int c = 0;
			for(int i = 0; i < edit.Length; i++) if(edit[i] == ';') c++;
			if(CheckField(c, totField)) return true; //c'è errore

			for(; c < totField; c++) edit += ";";

			edit = (edit+"0").PadRight(fdi); //gestione fixed dim
			Byte[] info = new UTF8Encoding(true).GetBytes(edit + "\r\n");

			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.None))
			{
				fs.Position = (fdi+2) * select; //fdi + \r\n //select è la vera linea del file
				fs.Write(info, 0, info.Length);
			}

			return false;
		}
	}
}

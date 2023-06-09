﻿using System;
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
using System.Diagnostics;
using System.Reflection;
using static System.Windows.Forms.LinkLabel;
using System.Drawing;
using System.Security.AccessControl;

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
		//7. Ricercare un record per campo chiave a scelta (se esiste, utilizzare il campo che contiene dati univoci)
		//8. Modificare un record
		//9. Cancellare logicamente un record
		//10. Realizzare l'interfaccia grafica che consenta l'interazione fluida con le funzionalità descritte. Richiamare le funzioni di servizio dalle funzioni di gestione degli eventi

		//accesso diretto
		//divisione in pagine (42 linee per pagina)


		public string path;
		public int totfield, max, fdi; //totale campi //max length // fdi fixed dim
		ColumnHeader[] ch;
		public Elaboratore_CSV()
		{
			InitializeComponent();
			path = Path.GetFullPath("..\\..\\..") + "\\files\\damora.csv";
			fdi = 0;

			//continua in shown
		}

		/* funzioni non utilizzate
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

		private void StampaCSV(ref ColumnHeader[] ch, int fdi, string path)
		{
			Lista.Clear();
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea
				UTF8Encoding enc = new UTF8Encoding(true);
				fs.Read(b, 0, fdi); //legge una linea
				string[] split = enc.GetString(b).TrimEnd().Split(';');

				ch = new ColumnHeader[split.Length];
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
				Lista.Columns.AddRange(ch);

				fs.Position+=2; // \r\n
				ListViewItem item;

				for(int i = 1; fs.Read(b, 0, fdi) > 0; i++) //i line index
				{
					split = enc.GetString(b).TrimEnd().Split(';'); //gestione fixed dim
					item = new ListViewItem(i.ToString());
					for(int j = 0; j < split.Length-1; j++)
						item.SubItems.Add(split[j]); //sub item
					Lista.Items.Add(item);
					fs.Position+=2;
				}
				for(int i = 0; i < split.Length; i++)
					ch[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		private void Form_Shown(object sender, EventArgs e)
		{
			//SetVisible();
			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max, path);

			AddCampo(fdi, path);

			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max, path);

			totfield = GetTotFields(fdi, path);
			TotFieldBox.Text = "Numero di campi: " + (totfield-1);
			MaxLengthBox.Text = "lunghezza massima dei record: " + max;
			StampaCSV(ref ch, fdi, path);
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
			if(AddLine(AddBox.Text, totfield, fdi, path, true)) return;
			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max, path);

			ListViewItem line;
			string[] splits = AddBox.Text.Split(';');
			line = new ListViewItem(Lista.Items.Count.ToString()); //main item è indice linea
			for(int j = 0; j < splits.Length; j++)
				line.SubItems.Add(splits[j]); //sub item
			Lista.Items.Add(line);

			for(int i = 0; i < ch.Length; i++)
				ch[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private int GetMaxLength(string path) //rimuove il fixed dim se presente
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
		private int FixedDim(int fdi, int max, string path) //ricalcola il fixed dim e lo applica se diverso
		{
			//max è la lunghezza solo del testo
			//fdi è il fixed dim ( compreso testo )

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
		private void AddCampo(int fdi, string path)
		{
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea escluso \n
				UTF8Encoding enc = new UTF8Encoding(true);
				fs.Read(b, 0, fdi); //legge una linea
				string line = enc.GetString(b).TrimEnd();

				string[] split = line.Split(';');
				for(int i = 0; i < split.Length; i++)
					if(split[i] == "miovalore") return;

				fs.Position = enc.GetBytes(line).Length; //posiziona alla fine del testo senza fdi

				byte[] info = enc.GetBytes(";miovalore;logic");
				fs.Write(info, 0, info.Length); //con fixed dim scrive sugli spazi

				fs.Position = fdi+2; // \r\n
				for(int lin = 2, pos; fs.Read(b, 0, fdi) > 0; lin++) //lin line index
				{
					line = enc.GetString(b);
					pos = enc.GetBytes(line.TrimEnd()).Length;
					fs.Position = fs.Position - fdi + pos; //positon si trova a fine riga escluso \r\n, ci tolgo il fixed dim e ci aggiungo la lunghezza del testo

					//Random rnd = new Random(lin * Environment.TickCount); //può essere che un millisecondo nel ciclo non abbia il tempo di trascorrere
					line = $";{new Random(lin * Environment.TickCount).Next(10, 20 + 1)};0";
					info = enc.GetBytes(line);
					fs.Write(info, 0, info.Length); //con fixed dim scrive sugli spazi

					fs.Position = (fdi+2) * lin; //si posiziona a inizio riga dopo //???!!! manca +1 ?
				}
			}
		}
		private int GetTotFields(int fdi, string path)
		{
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				long debug = fs.Position;
				fs.Seek(0, SeekOrigin.Begin);
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
			if(count > totField-1)
			{
				MessageBox.Show($"hai inserito troppi campi (max:{totField})", "errore in input");
				return true;
			}
			return false;
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
		private bool AddLine(string add, int totField, int fdi, string path, bool append)
		{
			int c = 0;
			for(int i = 0; i < add.Length; i++)
			{
				if(add[i] == ';') c++;
			}
			if(CheckField(c, totField-1)) return true; //c'è errore


			for(; c < totField-1; c++)
				add += ";";

			add = (add+"0").PadRight(fdi); //gestione fixed dim
			Byte[] info = new UTF8Encoding(true).GetBytes(add + "\r\n");

			if(append)
				using(FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None))
				{
					//fs.Position = fs.Length; //length - 1 è \n
					fs.Write(info, 0, info.Length);
				}
			//else;

			return false;
		}
	}
}

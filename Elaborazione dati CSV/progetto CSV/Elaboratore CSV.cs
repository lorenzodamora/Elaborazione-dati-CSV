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
using System.Diagnostics;
using System.Reflection;

namespace Elaborazione_dati_CSV
{
	/*public struct StructLine
	{
		public int ind;
		public string[] splits;
	}
	public struct StructFile
	{
		public StructLine[] csvLines;
		public int totline; //totline totale linee
	}*/
	public partial class Elaboratore_CSV : Form
	{
		//Aggiungere in coda ad ogni record un campo chiamato miovalore contenente un numero casuale 10<=X<=20 + un campo per la cancellazione logica
		//contare il numero dei campi che compongono il record
		//calcolare la lunghezza massima dei record presenti indicando anche la lunghezza massima di ogni campo
		//inserire in ogni record un numero di spazi necessari a rendere fissa la dimensione di tutti i record, senza perdere informazioni
		//Aggiungere un record in coda
		//Visualizzare dei dati mostrando tre campi significativi a scelta
		//Ricercare un record per campo chiave a scelta (se esiste, utilizzare il campo che contiene dati univoci
		//Modificare un record
		//Cancellare logicamente un record
		//Realizzare l'interfaccia grafica che consenta l'interazione fluida con le funzionalità descritte. Richiamare le funzioni di servizio dalle funzioni di gestione degli eventi

		//divisione in pagine (42 linee per pagina)

		public string path;
		public int totline; //tutte le linee tranne header
		public string[] csvLines;
		public Elaboratore_CSV()
		{
			InitializeComponent();
			path = Path.GetFullPath("..\\..\\..") + "\\files\\damora.csv";
			totline = GetLineCount(path) -1;
		}
		private int GetLineCount(string path)
		{
			int lineCount = 0;
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
			for(int ch = fs.ReadByte(); ch!=-1; ch = fs.ReadByte())
				if((char)ch == '\n') lineCount++;
			fs.Close();
			return lineCount;
		}

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
			if(line[line.Length-1] == '\n')
				// .SubString() perché altrimenti per ultimo rimarrebbe una stringa vuota
				lines = line.Substring(0, line.Length-1).Split('\n');
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
		/* funzioni non utilizzate
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
		}*/

		private void StampaCSV(string[] lines)
		{
			Lista.Clear();
			string[] splits;
			ListViewItem line;
			for(int i = 1; i < lines.Length; i++)
			{
				splits = lines[i].Split(';');
				line = new ListViewItem(i.ToString()); //main item
				for(int j = 0; j < splits.Length; j++)
					line.SubItems.Add(splits[j]); //sub item
				Lista.Items.Add(line);
			}
			splits = lines[0].Split(';');
			Lista.Columns.Add("line", -2, HorizontalAlignment.Center);
			for(int i = 0; i < splits.Length; i++)
				Lista.Columns.Add(splits[i], -2, HorizontalAlignment.Center);
			Lista.Columns.Add("", -2);
		}

		private void Elaboratore_CSV_Shown(object sender, EventArgs e)
		{
			//SetVisible();
			csvLines = FileReadAllLines(path);
			StampaCSV(csvLines);
		}
		private void Shortcut(object sender, KeyEventArgs e)
		{

		}
	}
}

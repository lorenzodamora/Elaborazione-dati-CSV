#region using
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text;
using System.Diagnostics;

namespace Elaborazione_dati_CSV
{
	public partial class Elaboratore_CSV : Form
	{
		#endregion
		public string path;
		public Elaboratore_CSV()
		{
			InitializeComponent();
			path = GetPath() + "\\damora-originale.csv";
			string path2 = GetPath() + "\\damora.csv";
			string[] lines1 = FileReadAllLines(path);

			string[] lines = new string[lines1.Length];
			for (int i = 1; i < lines1.Length; i++)
				lines[i] = lines1[i];
			Array.Sort(lines);
			lines[0]=lines1[0];
			FileWriteAllLines(path2, lines);
		}
		private string GetPath()
		{
			string path = Path.GetFullPath("..\\..\\..\\files");
			//Directory.CreateDirectory(path);
			return path;
		}
		private string[] FileReadAllLines(string path)
		{
			byte[] b = new byte[1024];
			UTF8Encoding temp = new UTF8Encoding(true);
			string line = "";
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
			while (fs.Read(b, 0, b.Length)>0)
				line += temp.GetString(b);
			fs.Close();

			if (line == "") return new string[0];

			line = line.TrimEnd('\0');
			// .SubString() perché altrimenti per ultimo rimarrebbe una stringa vuota
			string[] lines = line.Substring(0, line.Length-1).Split('\n');
			for (int i = 0; i < lines.Length; i++)
				lines[i] = lines[i].TrimEnd('\r');
			return lines;
		}
		private void FileWriteAllLines(string path, string[] lines)
		{
			FileWriteAllLines(path, lines, FileMode.Create);
		}
		private void FileWriteAllLines(string path, string[] lines, FileMode mode)
		{
			string allLines = string.Join("\n", lines);
			Byte[] info = new UTF8Encoding(true).GetBytes(allLines);
			FileStream fs = new FileStream(path, mode, FileAccess.Write, FileShare.None);
			fs.Write(info, 0, info.Length);
			fs.Close();
		}

	}
}

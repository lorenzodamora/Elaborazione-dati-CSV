using System;
using System.IO;
using System.Text;

namespace Elaboratore_CSV_Libreria
{
	public static class InternalFun
	{
		public static int GetMaxLength(string field, int totField, int fdi, string path)
		{
			if(Controlli.CheckField(field, totField)) //bad input
				return -1;

			int max = 0;
			int len;
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea escluso \n
				UTF8Encoding enc = new UTF8Encoding(true);
				string line = "";
				fs.Position = fdi + 2; // skippa gli header e \r\n
				while(fs.Read(b, 0, fdi)>0) //legge una linea
				{
					line = enc.GetString(b).TrimEnd();
					if(line.EndsWith("0")) //skippa le linee cancellate.
					{
						len = line.Split(';')[int.Parse(field)-1].Length;
						if(len > max) max = len;
					}
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
		public static bool AddLine(string add, int totField, int fdi, string path, bool append = false)
		{
			int c = 0;
			for(int i = 0; i < add.Length; i++) if(add[i] == ';') c++;
			if(Controlli.CheckField(c, totField)) return true; //c'è errore

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
		public static bool ResearchLines(string search, int fdi, string path, short[] lines)
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

					string searching, line;
					bool c;
					for(short i = 1, j = 1; fs.Read(bLine, 0, fdi+2) > 0; i++)
					{
						line = enc.GetString(bLine).TrimEnd(); //gestione fixed dim
						if(line.EndsWith("1"))
						{
							lines[i] = -1;
							continue; //skippa le linee cancellate.
						}
						searching = line.TrimEnd('0', '1').ToLower();
						/*
						///Counter. quando arriva a 0 = la linea ha tutte le parole cercate.
						int c = searchSplit.Length; for(; c != 0; c--) if(!searching.Contains(searchSplit[c-1])) break;
						if(c == 0) { temp.Write(bLine, 0, fdi+2); empty = false; }
						*/
						/// Controllo. Controlla se la linea ha tutte le parole cercate.
						c = true;
						foreach(string str in searchSplit) if(!searching.Contains(str))
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
			//if(!empty) StampaCSV(ref ch, fdi, tpath);
			//File.Delete(tpath);
			return empty;
		}
		public static bool SelectLine(string ind, int count)
		{
			return Controlli.CheckSelect(ind, count);
		}
		public static bool EditLine(string edit, int totField, int fdi, short select, string path)
		{
			int c = 0;
			for(int i = 0; i < edit.Length; i++) if(edit[i] == ';') c++;
			if(Controlli.CheckField(c, totField)) return true; //c'è errore

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
		public static void DeleteLine(short select, int fdi, string path)
		{
			/* wip. ah non serve
			fdi += 2;
			byte[] bLine = new byte[fdi];
			UTF8Encoding enc = new UTF8Encoding(true);
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
			{
				fs.Position = fdi * (select+1); //fdi + \r\n //select è la vera linea del file
				while(fs.Read(bLine, 0, fdi) > 0)
				{
					fs.Position -= 2 * fdi;
					fs.Write(bLine, 0, fdi);
				}
				fs.SetLength(fs.Length - fdi);
			}
			*/
			byte[] bLine = new byte[fdi];
			UTF8Encoding enc = new UTF8Encoding(true);
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
			{
				fs.Position = (fdi+2) * select; //fdi + \r\n //select è la vera linea del file
				fs.Read(bLine, 0, fdi);
				//bLine = new UTF8Encoding(true).GetBytes((enc.GetString(bLine).TrimEnd(' ', '0')+"1").PadRight(fdi-2));
				string line = enc.GetString(bLine).TrimEnd(' ', '0');
				line = (line+"1").PadRight(fdi); //gestione fixed dim
				bLine = new UTF8Encoding(true).GetBytes(line);
				fs.Position -= fdi;
				fs.Write(bLine, 0, fdi);
			}
		}
	}
}

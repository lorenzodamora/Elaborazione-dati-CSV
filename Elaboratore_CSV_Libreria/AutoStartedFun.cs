using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elaboratore_CSV_Libreria
{
	public static class AutoStartedFun
	{
		/**
 * <summary>
 * Trova la linea più lunga. (Rimuove il fixed dim se presente.)
 * </summary>
 * <returns>
 * La lunghezza della linea.
 * </returns>
 */
		public static int GetMaxLength(string path)
		{
			int max = 0;
			int len;
			int sum = 0;
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				int b;
				string line = "";
				bool first = true;
				bool logic = false;
				while((b = fs.ReadByte()) > 0)
					if((char)b == '\n')
					{
						if(first)
						{
							if(line.TrimEnd(' ', '\r').EndsWith(";logic"))
							{
								sum = -2; // ;0 // ;1
								logic = true;
							}
							first = false;
						}
						else
						{
							line = line.TrimEnd(' ', '\r');
							if(!(logic && line.EndsWith("1"))) //skippa le linee cancellate.
							{
								len = line.Length;
								if(len > max) max = len;
							}
						}
						line = "";
					}
					else
						line += (char)b;
			}
			return max + sum;
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
		public static int FixedDim(int fdi, int max, string path)
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
								Byte[] info = enc.GetBytes(line.TrimEnd(' ', '\r').PadRight(nfdi) + "\r\n"); //Enviroment.NewLine
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
		public static bool AddCampo(int fdi, string path)
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
		public static int GetTotFields(int fdi, string path)
		{
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea
				fs.Read(b, 0, fdi); //legge una linea
				return new UTF8Encoding(true).GetString(b).Split(';').Length;
			}
		}

	}
}

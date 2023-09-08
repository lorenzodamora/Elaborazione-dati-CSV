using System.IO;
using System.Text;

namespace Elaboratore_CSV_Libreria
{
	public static class Gestore_Lines
	{
		public static int GetLinesLength(int tot)
		{
			if(tot < 100) return 100;
			return tot/100 * 100 + 100;
		}
		public static void CheckLines(ref short[] lines, int tot)
		{
			int newSize = GetLinesLength(tot);
			if(newSize != lines.Length)
				LinesResize(ref lines, newSize);
		}
		public static void LinesResize(ref short[] array, int newSize)
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
		/// <summary>
		/// Quando ristampa tutto il file allora resetta anche le linee.
		/// </summary>
		/// <param name="lines"></param>
		/// <param name="fdi"></param>
		/// <param name="path"></param>
		public static void ResetLines(short[] lines, int fdi, string path)
		{
			//for(short i = 0; i < lines.Length; i++) lines[i] = i;
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				byte[] b = new byte[fdi]; //lunghezza una linea tranne \r\n
				UTF8Encoding enc = new UTF8Encoding(true);
				fs.Position = fdi + 2; // \r\n

				for(short ind = 1, i = 1; fs.Read(b, 0, fdi) > 0; i++, fs.Position+=2) // ind listview index // i short array index
					if(enc.GetString(b).TrimEnd().EndsWith("0")) //skippa le linee cancellate.
						lines[i] = ind++;
					else lines[i] = -1;
			}
		}
		public static short TrovaSelezionato(short[] lines, short sel)
		{
			for(short i = 1; i < lines.Length; i++) if(lines[i] == sel) return i;
			return -1;
		}
		public static short TrovaTotLinee(int fdi, string path)
		{
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				long fl = fs.Length;
				return (short)(fl / (fdi+2));
			}
		}

	}
}
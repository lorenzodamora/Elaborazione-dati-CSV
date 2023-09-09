using System;
using System.IO;
using System.Text;
//console
using System.Runtime.InteropServices;
//using Elaboratore_CSV_Libreria;
using static Elaboratore_CSV_Libreria.Gestore_Lines;
using static Elaboratore_CSV_Libreria.AutoStartedFun;
using static Elaboratore_CSV_Libreria.InternalFun;

namespace Console_Elaborazione_dati_CSV
{
	internal class Elaboratore_CSV
	{
		#region Dll Import
		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();
		#endregion

		static string path;
		static int totfield, max, fdi, sel; //totale campi (tranne logic remove) //max length // fdi fixed dim //sel selected line
		static short[] lines; //associa le linee del file a quelle visibili nella listview // l'indice è la linea nel file, mentre il valore è l'indice nella listview // indice 0 headers // se è -1 allora non è presente in listview. se è 0 allora non esiste.

		static void Main()
		{
			//4 Sviluppare sia la versione con form, che la versione console, sfruttando la stessa libreria dll.
			
			if(InitializeConsole())
			{
				Console.WriteLine("error in InitializeConsole() \n\n\n\n\n\n\n\n");
				return;
			}

			path = Path.GetFullPath("..\\..\\..") + "\\files\\damora.csv";
			fdi = 0;

			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max+2, path);

			if(AddCampo(fdi, path))
			{
				max = GetMaxLength(path);
				fdi = FixedDim(fdi, max+2, path);
			}

			// Un campo è il logic remove.
			totfield = GetTotFields(fdi, path) - 1;

			//TotFieldBox.Text = "Numero di campi: " + (totfield);
			//MaxLengthBox.Text = "lunghezza massima dei record: " + max;

			lines = new short[GetLinesLength(TrovaTotLinee(fdi, path))];
			ResetLines(lines, fdi, path);
			//StampaCSV(ref ch, fdi, path, true);
			
		}

		static bool InitializeConsole()
		{
			//int SW_SHOWMAXIMIZED = 3;
			IntPtr hWnd = GetConsoleWindow();
			if(hWnd != IntPtr.Zero) ShowWindow(hWnd, 3);
			else return true;


			return false;
		}
	}
}

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
//console
using System.Runtime.InteropServices;
using static System.Console;
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
				WriteLine("error in InitializeConsole() \n\n\n\n\n\n\n\n");
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

			//TotFieldBox.Text = "Numero di campi: " + totfield;
			//MaxLengthBox.Text = "lunghezza massima dei record: " + max;

			BackgroundColor = ConsoleColor.DarkGray;
			CursorTop = 4;
			Button(81, $" Numero di campi: {totfield} ");
			CursorTop = 6;
			Button(81, $" Lunghezza massima dei record: {max} ");

			lines = new short[GetLinesLength(TrovaTotLinee(fdi, path))];
			ResetLines(lines, fdi, path);
			//StampaCSV(ref ch, fdi, path, true);
			BackgroundColor = ConsoleColor.White;
			ForegroundColor = ConsoleColor.Black;
			CursorTop = 8;
			CursorLeft = 118;
			Write("è troppo complicato stampare 2000 righe probabilmente ognuna più lunga della console");

			CursorTop = 30;
			Button(35, new string(' ', 52));
			CursorTop--;
			CursorLeft = 37;

			bool rpr = true; //ripeti programma
			do
			{
				int fun;
				while(!int.TryParse(ReadKey().KeyChar.ToString(), out fun) || !(fun >= 0 && fun <= 4))
				{//bad input
					Task.Delay(300).Wait();
					SetCursorPosition(38, 32);
					Write("numero intero tra 0 e 4");
					SetCursorPosition(37, 31);
					Write(" ");
					CursorLeft--;
				}
				Task.Delay(500).Wait();


				switch(fun)
				{
					case 1:
						break;
				}

			} while(rpr);
			//end
			ReadKey(true);
		}

		/** <summary>
		 * Designer Console. Crea le grafiche iniziali.
		 * </summary>
		 * <returns>True se non ha trovato la finestra da mettere full screen</returns>
		 */
		static bool InitializeConsole()
		{
			///schermo intero
			//int SW_SHOWMAXIMIZED = 3;
			IntPtr hWnd = GetConsoleWindow();
			if(hWnd != IntPtr.Zero) ShowWindow(hWnd, 3);
			else return true;

			//Console.WriteLine(Console.WindowWidth + "  &  " + Console.WindowHeight); //237  &  56
			BackgroundColor = ConsoleColor.Gray;
			Clear();

			BackgroundColor = ConsoleColor.Black;
			Button(100, "     ELABORATORE DATI CSV     ");

			BackgroundColor = ConsoleColor.White;
			ForegroundColor = ConsoleColor.Black;
			CursorTop = 4;
			CursorLeft = 116;
			Write(new string(' ', WindowWidth-117));
			CursorLeft = 118;
			Write("Non è stato selezionato nessun elemento.");
			CursorTop++;
			while(CursorTop < WindowHeight-3)
			{
				CursorLeft = 116;
				CursorTop++;
				Write(new string(' ', WindowWidth-117));
			}

			BackgroundColor = ConsoleColor.Green;
			CursorTop = 6;
			Button(6, "   1. Get Field Length    ");
			CursorTop += 4;
			Button(6, "   2. Add    ");
			CursorTop += 4;
			Button(6, "   3. Search  ");
			BackgroundColor = ConsoleColor.White;
			ForegroundColor = ConsoleColor.DarkGray;
			CursorTop -= 2;
			Button(20, "  0. Refresh   ");
			BackgroundColor = ConsoleColor.Green;
			ForegroundColor = ConsoleColor.Black;
			CursorTop = 6;
			Button(50, "   4. Select    ");

			BackgroundColor = ConsoleColor.Black;
			ForegroundColor = ConsoleColor.White;
			CursorTop = 25;
			Button(35, "   Digita il numero della funzione che vuoi usare:  ");

			return false;
		}

		static void Button(int left, string name)
		{
			int space = name.Length;

			CursorLeft = left;
			Write(new string(' ', space));
			
			CursorLeft = left;
			CursorTop++;
			//Write(name + CursorTop +  "  &  " + (CursorLeft + space));
			Write(name);

			CursorLeft = left;
			CursorTop++;
			Write(new string(' ', space));
		}
	}
}

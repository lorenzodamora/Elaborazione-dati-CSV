using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
//console
using System.Runtime.InteropServices;
using static System.Console;
//message error
using System.Windows.Forms;
//using Elaboratore_CSV_Libreria;
using static Elaboratore_CSV_Libreria.Gestore_Lines;
using static Elaboratore_CSV_Libreria.AutoStartedFun;
using static Elaboratore_CSV_Libreria.InternalFun;

namespace Console_Elaborazione_dati_CSV
{
	//4x Sviluppare sia la versione con form, che la versione console, sfruttando la stessa libreria dll.

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
		static string[] listView; //array di stringhe visualizzate

		static string[] FileReadAllLines(string path)
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

			//check windows 10

			//Console.WriteLine(Console.WindowWidth + "  &  " + Console.WindowHeight); //237  &  56
			BackgroundColor = ConsoleColor.Gray;
			Clear();

			BackgroundColor = ConsoleColor.Black;
			ForegroundColor = ConsoleColor.White;
			Button(100, "     ELABORATORE DATI CSV     ");

			NameList();
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

			BlackText();

			return false;
		}
		static void ContinueConsole()
		{
			BackgroundColor = ConsoleColor.DarkGray;
			CursorTop = 4;
			Button(81, $" Numero di campi: {totfield} ");
			CursorTop = 6;
			Button(81, $" Lunghezza massima dei record: {max} ");

			BackgroundColor = ConsoleColor.White;
			ForegroundColor = ConsoleColor.Black;
			CursorTop = 20;
			CursorLeft = 118;
			Write("è troppo complicato stampare 2000 righe probabilmente ognuna più lunga della console");

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
		static void BlackText(string name = "   Digita il numero della funzione che vuoi usare:  ")
		{
			BackgroundColor = ConsoleColor.Black;
			ForegroundColor = ConsoleColor.White;
			CursorTop = 25;
			Button(35, name);
		}
		static void TextBox()
		{
			BackgroundColor = ConsoleColor.White;
			ForegroundColor = ConsoleColor.Black;
			CursorTop = 30;
			Button(35, new string(' ', 52));
			CursorTop--;
			CursorLeft = 37;
		}
		static void NameList(string name = "Non è stato selezionato nessun elemento.")
		{
			BackgroundColor = ConsoleColor.White;
			ForegroundColor = ConsoleColor.Black;
			CursorTop = 4;
			CursorLeft = 116;
			Write(new string(' ', WindowWidth-117));
			CursorLeft = 118;
			Write(name);
		}
		static void Deselect()
		{
			sel = -1;
			//NameList();
			InitializeConsole();
			ContinueConsole();
			//labEdit.Visible = txtEdit.Visible = BtnEdit.Visible = BtnDelete.Visible = false;
		}

		static void Main()
		{
			if(InitializeConsole())
			{
				WriteLine("error in InitializeConsole() \n\n\n\n\n\n\n\n");
				return;
			}

			path = Path.GetFullPath("..\\..\\..") + "\\files\\damora.csv";
			fdi = 0;
			sel = -1;
			listView = FileReadAllLines(path);

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

			ContinueConsole();

			lines = new short[GetLinesLength(TrovaTotLinee(fdi, path))];
			ResetLines(lines, fdi, path);

			//StampaCSV(ref ch, fdi, path, true);
			listView = FileReadAllLines(path);

			bool rpr = true; //ripeti programma
			do
			{
				int fun, maxfun;
				if(sel == -1) maxfun = 4;
				else maxfun = 6;
				TextBox();
				//!(fun >= 0 && fun <= 4)
				while(!int.TryParse($"{ReadKey().KeyChar}", out fun) || fun < 0 || fun > maxfun)
				{//bad input
					Task.Delay(300).Wait();
					SetCursorPosition(38, 32);
					Write($"numero intero tra 0 e {maxfun}");
					SetCursorPosition(37, 31);
					Write(" ");
					CursorLeft--;
				}
				Task.Delay(500).Wait();

				switch(fun)
				{
					case 1:
						FieldLengthButton_Click();
						break;
					case 2:
						AddButton_Click();
						break;
					case 3:
						BtnSearch_Click();
						break;
					case 0:
						BtnReload_Click();
						break;
					case 4:
						BtnSelect_Click();
						break;
					case 5:
						BtnEdit_Click();
						break;
					case 6:
						BtnDelete_Click();
						break;
				}

				//rpr = false;
			} while(rpr);
			//end
			ReadKey(true);
		}

		static void FieldLengthButton_Click()
		{
			BlackText("   Digita il numero del campo che vuoi misurare:    ");
			TextBox();
			string input = ReadLine();
			int fieldLength = GetMaxLength(input, totfield, fdi, path);

			if(fieldLength != -1)
			{
				BackgroundColor = ConsoleColor.DarkGray;
				CursorTop = 10;
				Button(81, $" Lunghezza max del campo {input}: {fieldLength} ");
			}
			BlackText();
		}
		static void AddButton_Click()
		{
			BlackText("    Scrivi il testo che vuoi aggiungere in coda:    ");
			TextBox();
			string input = ReadLine();
			//check lunghezza testo non fatta. //??
			if(AddLine(input, totfield, fdi, path, true)) goto Ret;
			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max+2, path);

			BackgroundColor = ConsoleColor.DarkGray;
			ForegroundColor = ConsoleColor.White;
			CursorTop = 6;
			Button(81, $" Lunghezza massima dei record: {max} ");

			BtnReload_Click();
			CheckLines(ref lines, listView.Length-1);

			Ret:
			BlackText();
		}
		static void BtnSearch_Click()
		{
			BlackText("    Scrivi il testo che vuoi ricercare dal file:    ");
			TextBox();
			string input = ReadLine();
			if(input != "")
			{
				Deselect();
				string tpath = Path.GetDirectoryName(path) + "\\temp.csv";
				if(ResearchLines(input, fdi, path, lines))
					MessageBox.Show("Nessun risultato trovato.\n\ntip:\nIl punto e virgola (;) è il divisore che fa cercare due parole diverse nella stessa linea", "errore nella ricerca");
				else
				{
				  //StampaCSV(ref ch, fdi, tpath);
					listView = FileReadAllLines(tpath);
					string stamp = "line";
					int i = 0;
					for(; i < listView.Length && i < 51; i++)
						stamp += i + ";" + listView[i] + "\n";
					if(i == 51) stamp += "sono stati visualizzati solo i primi 50 risultati.";
					MessageBox.Show(stamp, i == 51 ? "sono stati visualizzati solo i primi 50 risultati." : "risultati della ricerca");
				}

				File.Delete(tpath);
			}
			else
				MessageBox.Show("Digita qualcosa nella barra di Research per cercare.\n\ntip:\nIl punto e virgola (;) è il divisore che fa cercare due parole diverse nella stessa linea", "errore nella ricerca");

			BlackText();
		}
		static void BtnReload_Click()
		{
			Deselect();
			ResetLines(lines, fdi, path);

			//StampaCSV(ref ch, fdi, path);
			listView = FileReadAllLines(path);
		}
		static void BtnSelect_Click()
		{
			BlackText("  Digita la linea che vuoi selezionare dalla lista ");
			TextBox();
			string input = ReadLine();
			if(input != "")
			{
				if(input == "0")
				{
					Deselect();
					return;
				}
				if(SelectLine(input, listView.Length-1)) //tolti header
				{
					sel = int.Parse(input);
					NameList($"Stai modificando l'elemento {sel}.");
					TextBox();
					//for(int i = 1; i < Lista.Items[sel-1].SubItems.Count; i++) txtEdit.Text += Lista.Items[sel-1].SubItems[i].Text + ";"; //salto line
					//txtEdit.Text = txtEdit.Text.Remove(txtEdit.Text.Length - 1);

					
					BackgroundColor = ConsoleColor.White;
					ForegroundColor = ConsoleColor.Black;
					CursorTop = 6;
					CursorLeft = 118;
					WriteLine("Elemento selezionato:\n");
					CursorLeft = 118;
					Write(GetSelectedLine(TrovaSelezionato(lines, (short)sel), fdi, path));

					//labEdit.Visible = txtEdit.Visible = BtnEdit.Visible = BtnDelete.Visible = true;
					BackgroundColor = ConsoleColor.Green;
					ForegroundColor = ConsoleColor.Black;
					CursorTop = 12;
					Button(50, "   5. Edit    ");
					CursorTop += 4;
					Button(50, "   6. Delete    ");
				}
			}
			else
				MessageBox.Show("Digita qualcosa nella barra di Select per selezionare un elemento da modificare o eliminare.\n\ntip:\nDigita '0' per deselezionare.", "errore nella selezione");
			BlackText();
		}
		static void BtnEdit_Click()
		{
			BlackText("  Scrivi il testo che sostituirà il selezionato:   ");
			TextBox();
			string input = ReadLine();
			if(input.TrimEnd(' ', ';') != "")
			{
				if(input.Length < fdi-2)
				{
					if(EditLine(input, totfield, fdi, TrovaSelezionato(lines, (short)sel), path)) return;
					max = GetMaxLength(path);
					fdi = FixedDim(fdi, max+2, path);

					BtnReload_Click();
				}
				else
					MessageBox.Show($"L'elemento modificato è troppo lungo. (max: {fdi-2}).\n\ntip:\nIl punto e virgola (;) è il divisore che divide i campi nello stesso elemento", "errore nella modifica");
			}
			else
				MessageBox.Show("Digita qualcosa nella barra di Edit per modificare l'elemento selezionato.\n\ntip:\nIl punto e virgola (;) è il divisore che divide i campi nello stesso elemento", "errore nella modifica");
			BlackText();
		}
		static void BtnDelete_Click()
		{
			DeleteLine(TrovaSelezionato(lines, (short)sel), fdi, path);
			max = GetMaxLength(path);
			fdi = FixedDim(fdi, max+2, path);

			BtnReload_Click();
			//CheckLines(ref lines, Lista.Items.Count);
		}

		static string GetSelectedLine(short select, int fdi, string path)
		{
			byte[] bLine = new byte[fdi];
			UTF8Encoding enc = new UTF8Encoding(true);
			string line;
			using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				fs.Position = (fdi+2) * select; //fdi + \r\n //select è la vera linea del file
				fs.Read(bLine, 0, fdi);
				line = enc.GetString(bLine).TrimEnd(' ', '0');
			}
			return line.Remove(line.Length-1);
		}
	}
}

#include <iostream>
#include <Windows.h> // full screen //path //Sleep(500)
#include <locale> // Include l'header per la gestione delle localizzazioni
#include <fstream> // file esterni
#include <vector>
#include <random>
#include <ctime>
#include <conio.h> // Per _getch() su Windows
#include <climits> //int max
#include <string>

using namespace std;

#pragma region console template
void Button(short left, string name);
void BlackText(string name = "   Digita il numero della funzione che vuoi usare:  ");
void TextBox();
void NameList(string name = "Non è stato selezionato nessun elemento.");
#pragma endregion
#pragma region auto started function
int GetMaxLength(string path);
int FixedDim(int fdi, int maxl, string path);
bool AddCampo(int fdi, string path);
int GetTotFields(int fdi, string path);
#pragma endregion
#pragma region gestore lines
int GetLinesLength(int tot);
void ResetLines(short* lines, int fdi, string path);
short TrovaTotLinee(int fdi, string path);
#pragma endregion
#pragma region input checkers
char ReadKeyChar(bool intercept = false);
bool MyTryParseInt32(string s, int& result);
#pragma endregion


#pragma region service function
void Clear()
{
	system("CLS"); // pulisce la console
}

string GetPath() {
	char buffer[MAX_PATH];
	GetCurrentDirectoryA(MAX_PATH, buffer);

	return string(buffer);
}

vector<string> FileReadAllLines(const string& path) {
	vector<string> lines;
	char buffer[1024];

	ifstream file(path);

	if (!file) {
		cerr << "Errore: Impossibile aprire il file." << endl;
		return lines;
	}

	while (file.getline(buffer, sizeof(buffer))) {
		lines.push_back(buffer);
	}

	file.close();

	return lines;
}

bool MyCopyFile(string sourcePath, string destinationPath) {
	ifstream sourceFile(sourcePath, ios::binary);
	ofstream destFile(destinationPath, ios::binary);

	if (!sourceFile || !destFile) {
		cerr << "Errore: Impossibile aprire i file." << endl;
		return false;
	}

	char buffer[1024];
	while (!sourceFile.eof()) {
		sourceFile.read(buffer, sizeof(buffer));
		destFile.write(buffer, sourceFile.gcount()); //gcount() restituisce il numero di byte effettivamente letti durante l'ultima operazione di lettura.
	}

	sourceFile.close();
	destFile.close();

	return true;
}

string TrimEnd(string str) {
	// Rimuovi spazi extra dalla fine della linea
	size_t trim = str.find_last_not_of(' '); //trova l'ultimo carattere non vuoto, altrimenti trim conterrà string::npos
	if (trim != string::npos)
		str = str.substr(0, trim + 1);
	return str;
}
#pragma endregion

#pragma region public variable
HANDLE console;
CONSOLE_SCREEN_BUFFER_INFO cbi;

string path;
int totfield, maxl, fdi, sel; //totale campi (tranne logic remove) //max length // fdi fixed dim //sel selected line
short* lines; //associa le linee del file a quelle visibili nella listview // l'indice è la linea nel file, mentre il valore è l'indice nella listview // indice 0 headers // se è -1 allora non è presente in listview. se è 0 allora non esiste.
vector<string> listView; //array di stringhe visualizzate
#pragma endregion

#pragma region designer
/**
 * Designer Console. Crea le grafiche iniziali.
 * @return True se non ha trovato la finestra da mettere full screen.
 */
bool InitializeConsole()
{
	/** Full Screen */
	HWND hWnd = GetConsoleWindow();
	if (hWnd != NULL) {
		ShowWindow(hWnd, 3); // Usato invece di SW_SHOWMAXIMIZED
	}
	else {
		return true;
	}
	//check windows 10

	/*
	//Console.WriteLine(Console.WindowWidth + "  &  " + Console.WindowHeight); //237  &  56
	BackgroundColor = ConsoleColor.Gray; //gray white = 127
	Clear();

	BackgroundColor = ConsoleColor.Black;
	ForegroundColor = ConsoleColor.White; // black white = 15
	*/
	//HANDLE console; //#include <windows.h>
	console = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(console, 127); //bianco su grigio
	Clear();
	SetConsoleTextAttribute(console, 15); //bianco su nero
	Button((short)100, "     ELABORATORE DATI CSV     ");

	NameList();
	/*
	CursorTop++;
	while (CursorTop < WindowHeight - 3)
	{
		CursorLeft = 116;
		CursorTop++;
		Write(new string(' ', WindowWidth - 117));
	}
	*/
	//short cursorTop = cbi.dwCursorPosition.Y + 1;
	short cursorTop = 5;
	short windowHeight = cbi.dwMaximumWindowSize.Y - 3;
	size_t windowWidth = (size_t)cbi.dwSize.X - 117;
	SetConsoleCursorPosition(console, { 116, cursorTop });
	while (cursorTop < windowHeight)
	{
		SetConsoleCursorPosition(console, { 116, ++cursorTop });
		cout << string(windowWidth, ' ');
	}
	
	SetConsoleTextAttribute(console, 160); //nero su verde
	SetConsoleCursorPosition(console, { 6, 6 });
	Button((short)6, "   1. Get Field Length    ");
	SetConsoleCursorPosition(console, { 6, 12 });
	Button(6, "   2. Add    ");
	SetConsoleCursorPosition(console, { 6, 18 });
	Button(6, "   3. Search  ");
	SetConsoleTextAttribute(console, 248); //grigio scuro su bianco
	SetConsoleCursorPosition(console, { 6, 18 });
	Button(20, "  0. Refresh   ");
	SetConsoleTextAttribute(console, 160); //nero su verde
	SetConsoleCursorPosition(console, { 50, 6 });
	Button(50, "   4. Select    ");

	BlackText();

	return false;
}

void ContinueConsole()
{
	/*
	BackgroundColor = ConsoleColor.DarkGray; 143
	CursorTop = 4;
	Button(81, $" Numero di campi: {totfield} ");
	CursorTop = 6;
	Button(81, $" Lunghezza massima dei record: {max} ");

	BackgroundColor = ConsoleColor.White;
	ForegroundColor = ConsoleColor.Black;
	CursorTop = 20;
	CursorLeft = 118;
	Write("è troppo complicato stampare 2000 righe probabilmente ognuna più lunga della console");
	*/
	SetConsoleTextAttribute(console, 143); //bianco su dark grigio
	SetConsoleCursorPosition(console, { 81, 4 });
	Button(81, " Numero di campi: " + to_string(totfield) + " ");
	SetConsoleCursorPosition(console, { 81, 6 });
	Button(81, " Lunghezza massima dei record: " + to_string(maxl) + " ");

	SetConsoleTextAttribute(console, 240); //nero su bianco
	SetConsoleCursorPosition(console, { 118, 20 });
	cout << "è troppo complicato stampare 2000 righe probabilmente ognuna più lunga della console";
}
#pragma endregion

#pragma region console template
void Button(short left, string name)
{
	/*
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
	*/

	size_t space = name.length();

	//CONSOLE_SCREEN_BUFFER_INFO cbi;
	GetConsoleScreenBufferInfo(console, &cbi);
	short top = cbi.dwCursorPosition.Y;
	//else coo = { 0, 0 };

	SetConsoleCursorPosition(console, { left, top });
	cout << string(space, ' ');

	SetConsoleCursorPosition(console, { left, ++top });
	cout << name;

	SetConsoleCursorPosition(console, { left, ++top });
	cout << string(space, ' ');

}

void BlackText(string name)
{
	SetConsoleTextAttribute(console, 15); //bianco su nero
	SetConsoleCursorPosition(console, { 35, 25 });
	Button(35, name);
}

void TextBox()
{
	/*
	BackgroundColor = ConsoleColor.White;
	ForegroundColor = ConsoleColor.Black;
	CursorTop = 30;
	Button(35, new string(' ', 52));
	CursorTop--;
	CursorLeft = 37;
	*/
	SetConsoleTextAttribute(console, 240); //nero su bianco
	SetConsoleCursorPosition(console, { 35, 30 });
	Button(35, string((size_t)52, ' '));
	SetConsoleCursorPosition(console, { 37, 31 });
}

void NameList(string name)
{
	/*
	BackgroundColor = ConsoleColor.White;
	ForegroundColor = ConsoleColor.Black;
	CursorTop = 4;
	CursorLeft = 116;
	Write(new string(' ', WindowWidth - 117));
	CursorLeft = 118;
	Write(name);
	*/

	SetConsoleTextAttribute(console, 240); //nero su bianco
	SetConsoleCursorPosition(console, { 116, 4 });

	//CONSOLE_SCREEN_BUFFER_INFO csbi;
	//GetConsoleScreenBufferInfo(console, &csbi);
	//COORD coo = cbi.dwSize;
	GetConsoleScreenBufferInfo(console, &cbi);
	cout << string((size_t)cbi.dwSize.X - 117, ' ');
	SetConsoleCursorPosition(console, { 118, 4 });
	cout << name;
}
#pragma endregion


void main()
{
	locale::global(locale("")); //gestione caratteri speciali come "è"

	if (InitializeConsole()) {
		cout << "error in InitializeConsole() \n\n\n\n\n\n\n\n";
		return;
	}

	path = GetPath();
	path = path.erase(path.length() - 12) + "files\\damora.csv";
	fdi = 0;
	sel = -1;
	listView = FileReadAllLines(path);

	maxl = GetMaxLength(path);
	fdi = FixedDim(fdi, maxl + 2, path);

	if (AddCampo(fdi, path))
	{
		maxl = GetMaxLength(path);
		fdi = FixedDim(fdi, maxl + 2, path);
	}

	// Un campo è il logic remove.
	totfield = GetTotFields(fdi, path) - 1;

	ContinueConsole();

	lines = new short[GetLinesLength(TrovaTotLinee(fdi, path))];
	ResetLines(lines, fdi, path);

	//StampaCSV(ref ch, fdi, path, true);
	listView = FileReadAllLines(path); //placeholder storto

	bool rpr = true; //ripeti programma
	do
	{
		int fun, maxfun;
		if (sel == -1) maxfun = 4;
		else maxfun = 6;
		TextBox();
		while (!MyTryParseInt32(string(1, ReadKeyChar()), fun) || fun < 0 || fun > maxfun)
		{
			/*
			SetCursorPosition(38, 32);
			Write($"numero intero tra 0 e {maxfun}");
			SetCursorPosition(37, 31);
			Write(" ");
			CursorLeft--;
			*/
			Sleep(300);
			SetConsoleCursorPosition(console, { 38, 32 });
			cout << "numero intero tra 0 e " << maxfun;
			SetConsoleCursorPosition(console, { 37, 31 });
			cout << " ";
			SetConsoleCursorPosition(console, { 37, 31 });
		}
		Sleep(500);

		ReadKeyChar(true);

		switch (fun)
		{
		case 1:
			//FieldLengthButton_Click();
			break;
		case 2:
			//AddButton_Click();
			break;
		case 3:
			//BtnSearch_Click();
			break;
		case 0:
			//BtnReload_Click();
			break;
		case 4:
			//BtnSelect_Click();
			break;
		case 5:
			//BtnEdit_Click();
			break;
		case 6:
			//BtnDelete_Click();
			break;
		}

		//rpr = false;
	} while (rpr);
	//end
	//ReadKey(true);
}

#pragma region auto started function
/**
 * Trova la linea più lunga. (Rimuove il fixed dim se presente.)
 * @param path Il percorso del file da leggere.
 * @return La lunghezza della linea più lunga.
 */
int GetMaxLength(string path) {
	int max = 0;
	int len;
	int sum = 0;
	bool first = true;
	bool logic = false;
	ifstream file(path, ios::binary);

	if (!file) {
		cerr << "Errore: Impossibile aprire il file." << endl;
		return -1;
	}

	char b;
	string line;

	while (file.get(b)) {
		if (b == '\n') {
			if (first) {
				if (line.find(";logic", line.length() - 6) != string::npos) {
					sum = -2; // ;0 // ;1
					logic = true;
				}
				first = false;
			}
			else {
				line.erase(line.find_last_not_of(" \r") + 1);
				if (!(logic && line[line.length() - 1] == '1')) {
					len = line.length();
					if (len > max) max = len;
				}
			}
			line.clear();
		}
		else {
			line += b;
		}
	}

	file.close();
	return max + sum;
}

/**
 * Ricalcola il fixed dim e lo applica se diverso.
 * @param fdi L'attuale fixed dim (compreso testo).
 * @param maxl La lunghezza solo del testo.
 * @param path Il percorso del file da elaborare.
 * @return La dimensione fissa di ogni linea.
 */
int FixedDim(int fdi, int maxl, string path) {
	int nfdi; // Nuovo fdi

	if (maxl < 100) nfdi = 200;
	else nfdi = (maxl / 100) * 100 + 200;
	// Se il testo è meno di 100 caratteri, il fixed dim è 200; altrimenti, arrotonda maxl a multipli di 100 e aggiunge 200

	if (fdi != nfdi) // Se i valori sono diversi
	{
		string tpath = path + ".temp.csv"; // Crea il percorso del file temporaneo
		MyCopyFile(path, tpath); // Utilizza la tua funzione MyCopyFile per copiare il file

		ifstream infile(tpath, ios::binary); // Apri il file temporaneo in modalità binaria di input
		ofstream outfile(path, ios::binary | ios::trunc); // Apri il file originale in modalità binaria di output e sovrascrivi

		if (!infile || !outfile) {
			cerr << "Errore: Impossibile aprire i file." << endl;
			return fdi; // Restituisci il valore originale se si verificano errori di apertura dei file
		}

		char b;
		string line;

		while (infile.get(b)) {
			if (b == '\n') {
				// Se il carattere corrente è una newline
				line = line.erase(line.find_last_not_of(" \r") + 1);
				line = line.append(nfdi - line.length(), ' ') + "\r\n";
				//line.append(nfdi - line.length(), ' '); // Ridimensiona la linea al nuovo fixed dim
				//line += "\r\n"; // Aggiunge una newline
				outfile.write(line.c_str(), line.size()); // Scrive la linea nel file originale
				line.clear(); // Svuota la stringa
			}
			else {
				line += b; // Altrimenti, aggiungi il carattere alla linea
			}
		}

		infile.close(); // Chiudi il file temporaneo
		outfile.close(); // Chiudi il file originale

		// Rimuovi il file temporaneo
		remove(tpath.c_str());
	}

	return nfdi;
}

bool AddCampo(int fdi, string path) {
	fstream file(path, ios::in | ios::out | ios::binary);
	if (!file) {
		cerr << "Errore: Impossibile aprire il file." << endl;
		return -1;
	}

	char* buffer = new char[fdi]; //array di caratteri
	file.read(buffer, fdi); // legge 'fixed dim' byte dal file e li memorizza in buffer
	string line(buffer, fdi); //si crea una stringa che utilizza i dati nel buffer
	/* delete[] buffer; // libera la memoria utilizzata dall'array */

	line = TrimEnd(line);

	if (line.find(";miovalore;logic") != string::npos) return false;

	file.seekp(line.size(), ios::beg);
	file.write(";miovalore;logic", 16);

	file.seekp((size_t)fdi + 2, ios::beg);
	for (int lin = 2, pos; file.read(buffer, fdi); lin++) {
		string line(buffer, fdi);
		pos = TrimEnd(line).length();
		file.seekp((size_t)pos - fdi, ios::cur);

		srand(static_cast<unsigned>(time(nullptr)) * lin); // Inizializza il generatore di numeri casuali
		int num = rand() % 11 + 10; // Genera un numero casuale compreso tra 10 e 20
		line = ";" + to_string(num) + ";0";
		file.write(line.c_str(), line.length());
		file.seekp((size_t)(fdi + 2) * lin);
	}

	delete[] buffer;
	file.close();
	return true;
}

/**
 * Ottiene il numero di campi presenti nella prima linea del CSV.
 * @param fdi La lunghezza prevista di una linea nel file.
 * @param path Il percorso del file CSV.
 * @return Il numero di campi nella prima linea.
 */
int GetTotFields(int fdi, string path) {
	ifstream file(path, ios::binary);
	if (!file.is_open()) {
		cerr << "Errore nell'apertura del file" << endl;
		return -1; // Ritorno un valore negativo per segnalare un errore
	}

	char* buffer = new char[fdi]; // Lunghezza di una linea
	file.read(buffer, fdi); // Legge una linea
	string line(buffer, file.gcount()); // Costruisce una stringa dalla linea letta

	// Conta il numero di campi separati dal carattere ';'
	int numFields = 0;
	size_t pos = 0;
	while ((pos = line.find(';', pos)) != string::npos) {
		numFields++;
		pos++; // Avanza alla posizione successiva dopo il carattere ';'
	}

	delete[] buffer; // Libera la memoria allocata per il buffer
	return numFields;
}
#pragma endregion

#pragma region gestore lines
int GetLinesLength(int tot)
{
	if (tot < 100) return 100;
	return tot / 100 * 100 + 100;
}

void ResetLines(short* lines, int fdi, string path) {
	ifstream file(path, ios::binary);

	if (!file.is_open()) {
		cerr << "Errore nell'apertura del file." << endl;
		return; // Gestione dell'errore
	}

	file.seekg(fdi + 2); // \r\n

	short ind = 1;
	short i = 1;
	char* buffer = new char[fdi];
	while (file.read(buffer, fdi)) {
		string line(buffer, fdi);
		line = TrimEnd(line);

		if (line[line.length() - 1] == '0') // skippa le linee cancellate
			lines[i] = ind++;
		else 
			lines[i++] = -1;

		file.seekg(2, ios::cur); // Salta i due caratteri \r\n
	}

	delete[] buffer;
	file.close();
}

short TrovaTotLinee(int fdi, string path) {
	ifstream fs(path, ios::binary);

	if (!fs.is_open()) {
		cerr << "Errore nell'apertura del file." << endl;
		return -1; // Gestione dell'errore
	}

	fs.seekg(0, ios::end);
	long fl = fs.tellg(); // Lunghezza del file in byte
	fs.close();

	short result = (short)fl / (fdi + 2);
	return result;
}
#pragma endregion

#pragma region input checkers
char ReadKeyChar(bool intercept) {
	// Legge un carattere dalla console
	char keyPressed = _getch(); // Utilizziamo _getch() per leggere un singolo tasto
	// Stampa il carattere letto
	if (!intercept) cout << keyPressed;
	return keyPressed;
}

bool MyTryParseInt32(string s, int& result) {
	result = 0;
	bool isNegative = false;
	bool foundDigit = false;

	if (s.empty()) {
		return false;
	}

	short startIndex = 0;
	if (s[0] == '-') {
		isNegative = true;
		startIndex = 1;
	}

	for (int i = startIndex; i < s.length(); i++) {
		if (isdigit(s[i])) {
			int digitValue = s[i] - '0';

			// Check for potential overflow before updating result
			if (result > (INT_MAX - digitValue) / 10) {
				result = 0;
				return false;
			}

			result = result * 10 + digitValue;
			foundDigit = true;
		}
		else {
			result = 0;
			return false;
		}
	}

	if (isNegative) {
		result = -result;
	}

	return foundDigit;
}
#pragma endregion

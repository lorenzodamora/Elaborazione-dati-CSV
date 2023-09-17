#include <iostream>
#include <windows.h> // full screen //path //Sleep(500)
#include <locale> // Include l'header per la gestione delle localizzazioni
#include <fstream> // file esterni
#include <vector>
#include <random>
#include <ctime>
#include <conio.h> // Per _getch() su Windows
#include <climits> //int max
#include <string>
#include <sstream>

using namespace std;

#pragma region prototipi
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
void CheckLines(vector<short>& lines, int tot);
void LinesResize(vector<short>& array1, int newSize);
void ResetLines(vector<short>& lines, int fdi, string path);
short TrovaSelezionato(vector<short> lines, short sel);
int TrovaTotLinee(int fdi, string path);
#pragma endregion
#pragma region funzioni per input
char ReadKeyChar(bool intercept = false);
bool MyTryParseInt32(string s, int& result);
#pragma endregion

#pragma region button_click functions
void FieldLengthButton_Click();
void AddButton_Click();
void BtnSearch_Click();
void BtnReload_Click();
void BtnSelect_Click();
void BtnEdit_Click();
void BtnDelete_Click();
#pragma endregion
void Deselect();
#pragma region funzioni interne
int GetMaxLength(string field, int totField, int fdi, string path);
bool AddLine(string add, int totField, int fdi, string path, bool append = false);
bool ResearchLines(string search, int fdi, string path, vector<short>& lines);
bool SelectLine(string ind, int count);
bool EditLine(string edit, int totField, int fdi, short select, string path);
void DeleteLine(short select, int fdi, string path);
string GetSelectedLine(short select, int fdi, string path);
#pragma endregion
#pragma region controlli interni
bool CheckField(string field, int totField);
bool CheckField(int count, int totField);
bool CheckSelect(string check, int count);
#pragma endregion
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

//rimuove dalla fine space e anche \r\n\0\v\f\t
string TrimEnd(const string& str, const char* c = " \r\n\0\v\f\t") {
	string res;
	size_t trim = str.find_last_not_of(c); //trova l'ultimo carattere non vuoto, altrimenti trim conterrà string::npos
	if (trim != string::npos)
		res = str.substr(0, trim + 1);
	return res;
}

string ToLower(const string& input) {
	string result = input;
	for (char& c : result) {
		if (isupper(c)) {
			c = tolower(c);
		}
	}
	return result;
}

vector<string> stringSplit(const string& input, char delimiter) {
	vector<string> tokens;
	string token;
	istringstream tokenStream(input);

	while (getline(tokenStream, token, delimiter)) {
		tokens.push_back(token);
	}

	return tokens;
}

vector<string> FileReadAllLines(string path) {
	ifstream file(path, ios::binary);
	string line = "";
	char ch;
	while (file.get(ch)) line += ch;
	file.close();

	vector<string> lines;
	if (line == "") return lines;

	if (line[line.length() - 1] == '\n') line.pop_back();
	lines = stringSplit(line, '\n');
	for (int i = 0; i < lines.size(); i++)
		lines[i] = TrimEnd(lines[i], "\r");
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

string GetField(string line, int field, char sep)
{
	int c = 0;
	string result = "";
	while (c < line.length() && field != 0) if (line[c++] == sep) field--;
	while (line[c] != sep) result += line[c++];
	return result;
}

bool StrContains(const std::string& str, const std::string& subs) {
	if (str.length() < subs.length())
		return false;

	for (size_t i = 0, j; i <= str.length() - subs.length(); i++) {
		for (j = 0; j < subs.length(); j++)
			if (str[i + j] != subs[j])
				break;
		if (j == subs.length())
			return true;
	}
	return false;
}
#pragma endregion

#pragma region public variable
HANDLE console;
CONSOLE_SCREEN_BUFFER_INFO cbi;

string path;
int totfield, maxl, fdi, sel; //totale campi (tranne logic remove) //max length // fdi fixed dim //sel selected line
vector<short> lines; //associa le linee del file a quelle visibili nella listview // l'indice è la linea nel file, mentre il valore è l'indice nella listview // indice 0 headers // se è -1 allora non è presente in listview. se è 0 allora non esiste.
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
	Button((short)(100), "     ELABORATORE DATI CSV     ");

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
	Button(35, string(52LL, ' '));
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

#pragma warning(disable : 4326)
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

	lines.resize(GetLinesLength(TrovaTotLinee(fdi, path)));
	ResetLines(lines, fdi, path);

	//StampaCSV(ref ch, fdi, path, true);
	listView = FileReadAllLines(path); //placeholder storto//dovrei saltare i logic remove

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

		switch (fun)
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
	} while (rpr);
	//end
	//ReadKey(true);
}

#pragma region button_click functions
void FieldLengthButton_Click()
{
	BlackText("   Digita il numero del campo che vuoi misurare:    ");
	TextBox();
	string input;
	getline(cin, input);

	int fieldLength = GetMaxLength(input, totfield, fdi, path);

	if (fieldLength != -1) {
		SetConsoleTextAttribute(console, 143); //bianco su dark grigio
		SetConsoleCursorPosition(console, { 81, 10 });
		Button(81, " Lunghezza max del campo " + input + ": " + to_string(fieldLength) + " ");
	}
	BlackText();
}

void AddButton_Click()
{
	BlackText("    Scrivi il testo che vuoi aggiungere in coda:    ");
	TextBox();
	string input;
	getline(cin, input);
	//check lunghezza testo non fatta. //??
	if (AddLine(input, totfield, fdi, path, true)) goto Ret;
	maxl = GetMaxLength(path);
	fdi = FixedDim(fdi, maxl + 2, path);

	SetConsoleTextAttribute(console, 143); //bianco su dark grigio
	SetConsoleCursorPosition(console, { 81, 6 });
	Button(81, " Lunghezza massima dei record: " + to_string(maxl) + " ");

	BtnReload_Click();
	CheckLines(lines, listView.size() - 1);

Ret:
	BlackText();
}

void BtnSearch_Click()
{
	BlackText("    Scrivi il testo che vuoi ricercare dal file:    ");
	TextBox();
	string input;
	getline(cin, input);
	//input = TrimEnd(input, "\r\n");
	if(input != "")
	{
		Deselect();
		string tpath = path + ".temp.csv";
		if (ResearchLines(input, fdi, path, lines))
			MessageBoxA(NULL, "Nessun risultato trovato.\n\ntip:\nIl punto e virgola (;) è il divisore che fa cercare due parole diverse nella stessa linea", "errore nella ricerca", MB_OK);
		else
		{
			//StampaCSV(ref ch, fdi, tpath);
			listView = FileReadAllLines(tpath);
			string stamp = "line";
			int i = 0;
			for (; i < listView.size() && i < 51; i++)
				stamp += to_string(i) + ";" + TrimEnd(listView[i]) + "\n";
			if (i == 51) stamp += "sono stati visualizzati solo i primi 50 risultati.";
			MessageBoxA(NULL, stamp.c_str(), i == 51 ? "sono stati visualizzati solo i primi 50 risultati." : "risultati della ricerca", MB_OK);
		}

		remove(tpath.c_str());
	}
	else
		MessageBoxA(NULL, "Digita qualcosa nella barra di Research per cercare.\n\ntip:\nIl punto e virgola (;) è il divisore che fa cercare due parole diverse nella stessa linea", "errore nella ricerca", MB_OK);

	BlackText();
}

void BtnReload_Click()
{
	Deselect();
	
	ResetLines(lines, fdi, path);

	//StampaCSV(ref ch, fdi, path);
	listView = FileReadAllLines(path);
}

void BtnSelect_Click()
{
	BlackText("  Digita la linea che vuoi selezionare dalla lista ");
	TextBox();
	string input;
	getline(cin, input);
	if (input != "")
	{
		if (input == "0")
		{
			Deselect();
			return;
		}
		if (SelectLine(input, listView.size() - 1)) //tolti header
		{
			sel = stoi(input);
			NameList("Stai modificando l'elemento " + to_string(sel) + ".");
			TextBox();

			SetConsoleTextAttribute(console, 240); //nero su bianco
			SetConsoleCursorPosition(console, { 118, 6 });
			cout << "Elemento selezionato:";
			SetConsoleCursorPosition(console, { 118, 8 });
			cout << GetSelectedLine(TrovaSelezionato(lines, (short)sel), fdi, path);

			SetConsoleTextAttribute(console, 160); //nero su verde
			SetConsoleCursorPosition(console, { 50, 12 });
			Button(50, "   5. Edit    ");
			SetConsoleCursorPosition(console, { 50, 18 });
			Button(50, "   6. Delete    ");
		}
	}
	else
		MessageBoxA(NULL, "Digita qualcosa nella barra di Select per selezionare un elemento da modificare o eliminare.\n\ntip:\nDigita '0' per deselezionare.", "errore nella selezione", MB_OK);
	BlackText();
}

void BtnEdit_Click()
{
	BlackText("  Scrivi il testo che sostituirà il selezionato:   ");
	TextBox();
	string input;
	getline(cin, input);
	if (TrimEnd(input, " ;") != "")
	{
		if (input.length() < (size_t)fdi - 2)
		{
			if (EditLine(input, totfield, fdi, TrovaSelezionato(lines, (short)sel), path)) return;
			maxl = GetMaxLength(path);
			fdi = FixedDim(fdi, maxl + 2, path);

			BtnReload_Click();
		}
		else
			MessageBoxA(
				NULL,
				("L'elemento modificato è troppo lungo. (max: " + to_string(fdi - 2) + ").\n\ntip:\nIl punto e virgola(;) è il divisore che divide i campi nello stesso elemento").c_str(),
				"errore nella modifica",
				MB_OK
			);
	}
	else
		MessageBoxA(
			NULL,
			"Digita qualcosa nella barra di Edit per modificare l'elemento selezionato.\n\ntip:\nIl punto e virgola (;) è il divisore che divide i campi nello stesso elemento",
			"errore nella modifica",
			MB_OK
		);
	
	BlackText();
}

void BtnDelete_Click()
{
	DeleteLine(TrovaSelezionato(lines, (short)sel), fdi, path);
	maxl = GetMaxLength(path);
	fdi = FixedDim(fdi, maxl + 2, path);

	BtnReload_Click();
}
#pragma endregion

void Deselect()
{
	sel = -1;
	//NameList();
	InitializeConsole();
	ContinueConsole();
	//labEdit.Visible = txtEdit.Visible = BtnEdit.Visible = BtnDelete.Visible = false;
}

#pragma region funzioni interne
int GetMaxLength(string field, int totField, int fdi, string path)
{
	if (CheckField(field, totField)) //bad input
		return -1;

	int max = 0;
	int len;
	fstream file(path, ios::in | ios::binary);

	file.seekp((size_t)fdi + 2, ios::beg);
	char* buffer = new char[fdi];
	while (file.read(buffer, fdi))
	{
		string line(buffer, fdi);
		line = TrimEnd(line);
		if (line[line.length() - 1] == '0') // skippa le linee cancellate
		{
			len = GetField(line, stoi(field) - 1, ';').length();
			if (len > max) max = len;
		}
		file.seekp(2LL, ios::cur);
	}
	delete[] buffer;
	return max;
}

/**
 * append false non gestito.
 * @return True se c'è errore.
 */
bool AddLine(string add, int totField, int fdi, string path, bool append) {
	size_t lastValidChar = add.find_last_not_of("\r\n");
	if (lastValidChar != string::npos) add.erase(lastValidChar + 1);

	int c = 0;
	for (int i = 0; i < add.length(); i++) if (add[i] == ';') c++;
	if (CheckField(c, totField)) return true; //c'è errore

	for (; c < totField; c++) add += ";";

	add += "0";
	add = add.append(fdi - add.length(), ' ') + "\r\n";

	if (append)
	{
		ofstream file(path, ios::out | ios::app | ios::binary);
		file.write(add.c_str(), add.length());
		file.close();
	}
	//else //append false;

	return false;
}

bool ResearchLines(string search, int fdi, string path, vector<short>& lines)
{
	string tpath = path + ".temp.csv";
	bool empty = true;
	vector<string> searchSplit = stringSplit(ToLower(TrimEnd(search)), ';');

	fstream file(path, ios::in | ios::binary);
	fstream tfile(tpath, ios::out | ios::binary);

	size_t count = (size_t)fdi + 2;
	char* buffer = new char[count];
	file.read(buffer, count);
	tfile.write(buffer, count);
	string searching, line;
	bool c;
	for (short i = 1, j = 1; file.read(buffer, count); i++) {
		string line(buffer, count);
		line = TrimEnd(line);
		if (line[line.length() - 1] == '1')
		{
			lines[i] = -1;
			continue; //skippa le linee cancellate.
		}

		searching = ToLower(TrimEnd(line, "01"));
		/** Controllo.Controlla se la linea ha tutte le parole cercate. */
		c = true;
		for (string str : searchSplit)
			if (!StrContains(searching, str))
			{
				c = false;
				lines[i] = -1;
				break;
			}
		if (c)
		{
			lines[i] = j++;
			tfile.write(buffer, count);
			empty = false;
		}
	}

	file.close();
	tfile.close();
	delete[] buffer;
	return empty;
}

bool SelectLine(string ind, int count) {
	return CheckSelect(ind, count);
}

bool EditLine(string edit, int totField, int fdi, short select, string path) {
	int c = 0;
	for (int i = 0; i < edit.length(); i++) if (edit[i] == ';') c++;
	if (CheckField(c, totField)) return true; //c'è errore
	
	for (; c < totField; c++) edit += ";";
	edit += "0";
	edit = edit.append(fdi - edit.length(), ' ') + "\r\n";

	fstream file(path, ios::in | ios::out | ios::binary);
	file.seekp((size_t)((fdi + 2) * select), ios::beg);
	file.write(edit.c_str(), edit.length());
	file.close();

	return false;
}

void DeleteLine(short select, int fdi, string path) {
	fstream file(path, ios::in | ios::out | ios::binary);
	size_t count = (size_t)fdi + 2;
	file.seekp(count * select, ios::beg);

	char* buffer = new char[fdi]; //array di caratteri
	file.read(buffer, fdi); // legge 'fixed dim' byte dal file e li memorizza in buffer
	string line(buffer, fdi); //si crea una stringa che utilizza i dati nel buffer

	line = TrimEnd(line, " 0") + "1";
	line = line.append(fdi - line.length(), ' ');

	file.seekg((streampos)-fdi, ios::cur);
	file.write(line.c_str(), line.length());

	file.close();
	delete[] buffer;
}

string GetSelectedLine(short select, int fdi, string path) {
	char* buffer = new char[fdi];
	ifstream file(path, ios::binary);
	file.seekg((size_t)((fdi + 2) * select));
	file.read(buffer, fdi);
	file.close();
	string line(buffer, fdi);
	line = TrimEnd(line, " 0");
	line.pop_back();
	return line;
}
#pragma endregion
#pragma region controlli interni
bool CheckField(string field, int totField) //ritorna true se ci sono errori
{
	int fie;
	if (!MyTryParseInt32(field, fie) || fie < 1)
	{//bad input
		MessageBox(NULL, L"numero intero positivo\n\ntip:\nIl campo Lines ha indice 0; ma non si può calcolare la lunghezza del campo Lines", L"errore nella selezione", MB_OK);
		return true;
	}
	if (fie > totField)
	{//bad input
		MessageBox(NULL, L"inserisci un campo esistente", L"errore nella selezione", MB_OK);
		return true;
	}
	return false;
}
bool CheckField(int count, int totField) //ritorna true se ci sono errori
{
	if (count == 0)
	{
		MessageBox(NULL, L"per inserire in campi diversi separa con ';'", L"errore in input", MB_OK);
		return true;
	}
	if (count > totField - 1) //campo line
	{
		MessageBoxA(NULL, ("hai inserito troppi campi (max: " + to_string(totField) + ")").c_str(), "errore in input", MB_OK);
		return true;
	}
	return false;
}
bool CheckSelect(string check, int count)
{
	int ind;
	if (!MyTryParseInt32(check, ind) || ind < 1)
	{//bad input
		MessageBoxA(NULL, "inserisci un intero positivo", "errore nella selezione", MB_OK);
		return false;
	}
	if (ind > count)
	{//bad input
		MessageBoxA(NULL, ("inserisci un indice che appare in lista (max: " + to_string(count) + ")").c_str(), "errore nella selezione", MB_OK);
		return false;
	}
	return true; //ret false = la stringa non è valida
}
#pragma endregion

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
	char b;
	string line;

	ifstream file(path, ios::binary);

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

		ifstream tfile(tpath, ios::binary); // Apri il file temporaneo in modalità binaria di input
		ofstream file(path, ios::binary | ios::trunc); // Apri il file originale in modalità binaria di output e sovrascrivi

		char b;
		string line;

		while (tfile.get(b)) {
			if (b == '\n') {
				line = TrimEnd(line);
				line = line.append(nfdi - line.length(), ' ') + "\r\n";
				//line.append(nfdi - line.length(), ' '); // Ridimensiona la linea al nuovo fixed dim
				//line += "\r\n"; // Aggiunge una newline
				file.write(line.c_str(), line.size()); // Scrive la linea nel file originale
				line.clear(); // Svuota la stringa
			}
			else {
				line += b; // Altrimenti, aggiungi il carattere alla linea
			}
		}

		tfile.close(); // Chiudi il file temporaneo
		file.close(); // Chiudi il file originale

		// Rimuovi il file temporaneo
		remove(tpath.c_str());
	}

	return nfdi;
}

bool AddCampo(int fdi, string path) {
	fstream file(path, ios::in | ios::out | ios::binary);

	char* buffer = new char[fdi]; //array di caratteri
	file.read(buffer, fdi); // legge 'fixed dim' byte dal file e li memorizza in buffer
	string line(buffer, fdi); //si crea una stringa che utilizza i dati nel buffer
	/* delete[] buffer; // libera la memoria utilizzata dall'array */

	line = TrimEnd(line);

	if (line.find(";miovalore;logic") != string::npos) return false;

	file.seekp(line.size(), ios::beg);
	file.write(";miovalore;logic", 16);

	file.seekp((size_t)fdi + 2, ios::beg);
	short lin = 2;
	for (size_t pos; file.read(buffer, fdi); lin++) {
		string line(buffer, fdi);
		pos = TrimEnd(line).length();
		file.seekp(pos - fdi, ios::cur);

		srand(static_cast<unsigned>(time(nullptr)) * lin); // Inizializza il generatore di numeri casuali
		//int num = rand() % 11 + 10; // Genera un numero casuale compreso tra 10 e 20
		line = ";" + to_string(rand() % 11 + 10) + ";0";
		file.write(line.c_str(), line.length());
		file.seekp((size_t)((fdi + 2) * lin));
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

	char* buffer = new char[fdi]; // Lunghezza di una linea
	file.read(buffer, fdi); // Legge una linea
	file.close();
	string line(buffer, fdi); // Costruisce una stringa dalla linea letta

	// Conta il numero di caratteri ';'
	int numFields = 0;
	size_t pos = 0;
	while ((pos = line.find(';', pos)) != string::npos) {
		numFields++;
		pos++; // Avanza alla posizione successiva dopo il carattere ';'
	}

	delete[] buffer; // Libera la memoria allocata per il buffer
	return numFields + 1; //con x separatori abbiamo x+1 campi
}
#pragma endregion

#pragma region gestore lines
int GetLinesLength(int tot)
{
	if (tot < 100) return 100;
	return tot / 100 * 100 + 100;
}

void CheckLines(vector<short>& lines, int tot) {
	int newSize = GetLinesLength(tot);
	if (newSize != lines.size())
		LinesResize(lines, newSize);
}

void LinesResize(vector<short>& array1, int newSize)
{
	int len = array1.size();
	if (len != newSize)
	{
		vector<short> array2(newSize);
		if (len < newSize) //se newsize è più grande, copia fino ad array.length e il resto rimane default
			for (int i = 0; i < len; i++)
				array2[i] = array1[i];
		else //se newsize è più piccolo copia fino a newsize
			for (int i = 0; i < newSize; i++)
				array2[i] = array1[i];
		array1 = array2;
	}
}

/**
 * Quando ristampa tutto il file allora resetta anche le linee.
 */
void ResetLines(vector<short>& lines, int fdi, string path) {
	/*
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
	*/

	ifstream file(path, ios::binary);
	file.seekg((size_t)fdi + 2); // \r\n

	char* buffer = new char[fdi];
	for (short ind = 1, i = 1; file.read(buffer, fdi); i++) {
		string line(buffer, fdi);
		line = TrimEnd(line);

		if (line[line.length() - 1] == '0') // skippa le linee cancellate
			lines[i] = ind++;
		else lines[i] = -1;

		file.seekg(2LL, ios::cur); // Salta i due caratteri \r\n
	}

	delete[] buffer;
	file.close();
}

short TrovaSelezionato(vector<short> lines, short sel)
{
	for (short i = 1; i < lines.size(); i++)
		if (lines[i] == sel) return i;
	return -1;
}

int TrovaTotLinee(int fdi, string path) {
	ifstream file(path, ios::binary);

	file.seekg(0, ios::end);
	size_t fl = file.tellg(); // Lunghezza del file in byte
	file.close();

	int result = (int)fl / (fdi + 2);
	return result;
}
#pragma endregion

#pragma region funzioni per input
char ReadKeyChar(bool intercept) {
	char keyPressed = _getch();
	if (!intercept) cout << keyPressed;
	return keyPressed;
}

bool MyTryParseInt32(string s, int& result) {
	s = TrimEnd(s);
	if (s.empty()) {
		return false;
	}

	bool isNegative = false;
	short startIndex = 0;
	if (s[0] == '-') {
		isNegative = true;
		startIndex = 1;
	}

	bool foundDigit = false;
	int old = result;
	result = 0;
	for (int i = startIndex; i < s.length(); i++) {
		if (isdigit(s[i])) {
			int digitValue = s[i] - '0';

			// Check for potential overflow before updating result
			if (result > (INT_MAX - digitValue) / 10) {
				result = old;
				return false;
			}

			result = result * 10 + digitValue;
			foundDigit = true;
		}
		else {
			result = old;
			return false;
		}
	}

	if (isNegative) {
		result = -result;
	}

	if (foundDigit) return true;
	else {
		result = old;
		return false;
	}
}
#pragma endregion

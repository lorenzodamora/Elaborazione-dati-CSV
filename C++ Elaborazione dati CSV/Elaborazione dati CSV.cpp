#include <iostream>
#include <Windows.h> // full screen
#include <string>

using namespace std;

void Button(short left, string name, HANDLE console);
void NameList(string name, HANDLE console);


void Clear()
{
	system("CLS"); // pulisce la console
}

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
	HANDLE console; //#include <windows.h>
	console = GetStdHandle(STD_OUTPUT_HANDLE);
	SetConsoleTextAttribute(console, 127); //bianco su grigio
	Clear();
	SetConsoleTextAttribute(console, 15); //bianco su nero
	Button((short)100, "     ELABORATORE DATI CSV     ", console);

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



	return false;
}

void Button(short left, string name, HANDLE console)
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

	CONSOLE_SCREEN_BUFFER_INFO cbi;
	short top = 0;
	if (GetConsoleScreenBufferInfo(console, &cbi))
		top = cbi.dwCursorPosition.Y;
	//else coo = { 0, 0 };

	SetConsoleCursorPosition(console, { left, top });
	cout << string(space, ' ');

	SetConsoleCursorPosition(console, { left, ++top });
	cout << name;

	SetConsoleCursorPosition(console, { left, ++top });
	cout << string(space, ' ');

}
void NameList(string name = "Non è stato selezionato nessun elemento.", HANDLE console)
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

	CONSOLE_SCREEN_BUFFER_INFO csbi;
	GetConsoleScreenBufferInfo(console, &csbi);
	COORD coo = csbi.dwSize;
	cout << string((size_t)csbi.dwSize.X - 117, ' ');
	SetConsoleCursorPosition(console, { 118, 4 });
	cout << name;
}


void main()
{
	if (InitializeConsole()) {
		cout << "error in InitializeConsole() \n\n\n\n\n\n\n\n";
		return;
	}
}

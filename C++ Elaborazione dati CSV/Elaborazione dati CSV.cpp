#include <iostream>
#include <Windows.h> // full screen

using namespace std;

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

	return false;
}

void main()
{
	cout << "Hello World!\n";

	if (InitializeConsole()) {
		cout << "error in InitializeConsole() \n\n\n\n\n\n\n\n";
		return;
	}
}

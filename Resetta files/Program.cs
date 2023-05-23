using System;
using System.IO;

namespace resetta_files
{
	internal class Program
	{
		static void Main()
		{
            string path = Path.GetFullPath("..\\..\\..\\Elaborazione dati CSV\\files\\");
			File.Copy(path + "damora-originale.csv", path + "damora.csv", true);
		}
	}
}
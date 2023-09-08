using System.Windows.Forms;

namespace Elaboratore_CSV_Libreria
{
	public class Controlli
	{
		public static bool CheckField(string field, int totField) //ritorna true se ci sono errori
		{
			if(!int.TryParse(field, out int fie) || fie < 1)
			{//bad input
				MessageBox.Show("numero intero positivo\n\ntip:\nIl campo Lines ha indice 0; ma non si può calcolare la lunghezza del campo Lines", "errore nella selezione");
				return true;
			}
			if(fie > totField)
			{//bad input
				MessageBox.Show("inserisci un campo esistente", "errore nella selezione");
				return true;
			}
			return false;
		}
		public static bool CheckField(int count, int totField) //ritorna true se ci sono errori
		{
			if(count == 0)
			{
				MessageBox.Show("per inserire in campi diversi separa con ';'", "errore in input");
				return true;
			}
			if(count > totField-1) //campo line
			{
				MessageBox.Show($"hai inserito troppi campi (max:{totField})", "errore in input");
				return true;
			}
			return false;
		}
		public static bool CheckSelect(string check, int count)
		{
			if(!int.TryParse(check, out int ind) || ind < 1) //ind = indice linea da selezionare
			{//bad input
				MessageBox.Show("inserisci un intero positivo", "errore nella selezione");
				return false;
			}
			if(ind > count)
			{//bad input
				MessageBox.Show($"inserisci un indice che appare in lista (max: {count})", "errore nella selezione");
				return false;
			}
			return true; //ret false = la stringa non è valida
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elaborazione_dati_CSV_ripasso_pre_rientro
{
	internal static class Starter
	{
		/** <summary>
		 * Punto di ingresso principale dell'applicazione.
		 * </summary>
		 */
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Elaboratore_CSV());
		}
	}
}
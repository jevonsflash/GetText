using System;
using System.Windows.Forms;

namespace CSharp
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{


            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Application.Run((Form)new Form1());
		}
	}
}

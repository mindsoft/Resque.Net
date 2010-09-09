using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Resque
{
	public class UAPushJob :IResqueJob
	{
		public string PleaseNo { get; set; }
		#region IResqueJob Members

		public void Perform()
		{
			Console.WriteLine("This is my golden ticket to job processing.");
		}

		#endregion
	}
}

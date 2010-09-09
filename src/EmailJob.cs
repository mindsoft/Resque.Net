using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resque
{
	public class EmailJob : IResqueJob
	{


		public long PK { get; set; }




		public void Perform()
		{
			Console.WriteLine("Performing acts of {0} with PK {1}", this.GetType().Name, this.PK);
		}
	}
}

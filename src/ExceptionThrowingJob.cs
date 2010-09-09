using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resque
{
	/// <summary>
	/// This Job intentionally throws an exception so that we
	/// can test the work flow of exception handling. 
	/// </summary>
	public class ExceptionThrowingJob : IResqueJob
	{
		public string Desc { get; set; }

		public void Perform()
		{
			Console.WriteLine("About to go Kaboom!");
			throw new Exception("Kaboom.  How did the worker handles this.  It should go in the exception queue");
		}
	}
}

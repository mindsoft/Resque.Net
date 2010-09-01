using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resque
{
	public class APNJob : IResqueJob
	{

		public string DeviceId { get; set;}

		#region IResqueJob Members

		public void Perform()
		{
			Console.WriteLine("Performing acts of {0} with DeviceId {1}", this.GetType().Name, this.DeviceId);
		}

		#endregion
	}
}

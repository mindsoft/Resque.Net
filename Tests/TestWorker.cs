using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Resque;


namespace Tests
{
	public class TestWorker
	{

		[Test]
		public void TestWork()
		{
			string[] queues = new string[] { "queue:apn", "queue:email" };

			Worker bee = Worker.GetWorker(queues);

			bee.Work(30);

		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Resque;
using ServiceStack.Text;

namespace Tests
{
	[TestFixture]
	public class TestResqueObject
	{

		[Test]
		public void TestDeserialization()
		{
			ResqueObject org = new ResqueObject { Queue = "testQueue"};

			org.Job = new EmailJob { PK=10 };
			org.JobType = org.Job.GetType();
			var jsv = TypeSerializer.SerializeToString(org);

			Console.WriteLine(jsv);
			

			var dynamicType = TypeSerializer.DeserializeFromString<ResqueObject>(jsv);

			var job = (IResqueJob)dynamicType.GetJob();

			Assert.AreEqual(typeof(EmailJob), job.GetType());

			job.Perform();


		
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Resque;
using ServiceStack.ServiceClient;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ServiceStack.Text;



namespace Tests
{
	[TestFixture]
	public class TestResque
	{


		[Test]
		public void TestPushJob()
		{
			ResqueClient q = ResqueClient.GetResque();

			//Create an Email Job
			ResqueObject email = new ResqueObject { Queue = "queue:email" };
			email.Job = new EmailJob { PK = 10 };
			email.JobType = email.Job.GetType();
			q.Push(email);

			//Create an Apple Push Notification
			ResqueObject apn = new ResqueObject { Queue = "queue:apn" };
			apn.Job = new APNJob { DeviceId = "sdfjkajsldfkjkl90823" };
			apn.JobType = apn.Job.GetType();

			q.Push(apn);

			////Create an Apple Push Notification
			//ResqueObject uaJob = new ResqueObject { Queue = "queue:apn" };
			//uaJob.Job = new UAPushJob { PleaseNo = "Does this work now?" };
			//uaJob.JobType = uaJob.Job.GetType();

			//q.Push(uaJob);



		}

		[Test]
		public void TestExceptionJob()
		{
			ResqueClient q = ResqueClient.GetResque();

			//Create an Email Job
			ResqueObject exp = new ResqueObject { Queue = "queue:email" };
			exp.Job = new ExceptionThrowingJob { Desc = "Go boom boom" };
			exp.JobType = exp.Job.GetType();
			q.Push(exp);
		
		}

		[Test]
		public void TestDequeueJob()
		{
			const string queueName = "queue:email";

			ResqueClient q = ResqueClient.GetResque();
			
			ResqueObject resObj = q.Pop(queueName);
			while (resObj != null)
			{
				resObj.PerformJob();
				resObj = q.Pop(queueName);
			}
		}


		[Test]
		public void TestStronglyTypedResque()
		{
			ResqueClient q = ResqueClient.GetResque();
			using (RedisClient redisClient = q.Redis())
			{
				//Create a 'strongly-typed' API that makes all Redis Value operations to apply against Shippers
				IRedisTypedClient<ResqueObject> redis = redisClient.GetTypedClient<ResqueObject>();

				//Redis lists implement IList<T> while Redis sets implement ICollection<T>
				var emailQueue = redis.Lists["queue:email"];

				emailQueue.Add(
						new ResqueObject
						{
							Queue = "queue:email"
						});


			}


		}

	}
}

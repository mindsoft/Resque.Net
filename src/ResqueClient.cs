using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ServiceStack.Text;

namespace Resque
{
	public class ResqueClient
	{

		public static ResqueClient GetResque()
		{
			return new ResqueClient();
		}

		public RedisClient Redis(string server, int port)
		{
			return new RedisClient(server, port);
		}

		public RedisClient Redis()
		{
			return Redis("localhost", 6379);
		}


		#region Queue Manipulation


		/// <summary>
		/// This allow you to override the queue field in the Resque Object
		/// Useful when a job fails and we need to put it in the error queue
		/// </summary>
		/// <param name="queueName"></param>
		/// <param name="item"></param>
		public void Push(string queueName, ResqueObject item)
		{
			using (RedisClient redisClient = Redis())
			{
				//Create a 'strongly-typed' API that makes all Redis Value operations to apply against Shippers
				WatchQueue(queueName);
				var jsv = TypeSerializer.SerializeToString(item);
				redisClient.PushItemToList(queueName, jsv);

			}

		}

		/// <summary>
		/// Pushes a job onto a queue. Queue name should be a string and the
		/// item should be any JSON-able POCO.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="?"></param>
		public void Push(ResqueObject item)
		{
			Push(item.Queue, item);
		}

		public void PushToFailures(ResqueObject item)
		{
			Push("queue:failures", item);
		}

		public ResqueObject Pop(string queueName)
		{
			using (RedisClient redisClient = Redis())
			{
				string jsv = redisClient.PopItemFromList(queueName);
				ResqueObject dynamicType = null;
				if (!String.IsNullOrEmpty(jsv))
				{
					dynamicType = TypeSerializer.DeserializeFromString<ResqueObject>(jsv);
				}

				return dynamicType;

				////Create a 'strongly-typed' API that makes all Redis Value operations to apply against Shippers
				//IRedisTypedClient<ResqueObject> redis = redisClient.GetTypedClient<ResqueObject>();

				////Redis lists implement IList<T> while Redis sets implement ICollection<T>
				//var queue = redis.Lists[queueName];
				//return queue.Dequeue();
			}
		
		}



		#endregion


		/// <summary>
		/// 
		/// </summary>
		/// <param name="queue"></param>
		public void WatchQueue(string queue)
		{ 
			//TODO:MJ Implement queue watching.
		}

	}
}

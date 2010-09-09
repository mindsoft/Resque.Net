using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.Text;

namespace Resque
{
	/// <summary>
	/// Services a specified Resque
	/// </summary>
	public class Worker
	{

		private bool _shutDown = false;

		private string[] _queues;
		public string[] Queues { get { return _queues; } }


		private Worker(string[] queues)
		{
			_queues = queues;
		}

		/// <summary>
		/// Use this factory to create a new worker
		/// Pass in a string[] of queue names
		/// Order is important, the worker will service all items in a specific 
		/// queue until there are no 
		/// </summary>
		/// <param name="queues"></param>
		/// <returns></returns>
		public static Worker GetWorker(string[] queues)
		{
			Worker newWorker = new Worker(queues);
			return newWorker;
		}

		public void Work()
		{
			Work(5);
		}

		public void Work(int interval)
		{
			Util.Log.InfoFormat("Starting Worker with {0} interval", interval);
			do
			{

				ResqueObject item = Reserve();
				if (item != null)
				{
					//log got job dump item
					Util.Log.InfoFormat("Found job {0}", item.Dump());
					WorkingOn(item);

					try
					{
						item.PerformJob();
					}
					catch (Exception e)
					{
						ResqueClient q = ResqueClient.GetResque();
						item.exception = e;
						q.PushToFailures(item);
						q = null;
					}
					
					DoneWorking();
				}
				else 
				{
					if (interval > 0)
					{
						//Log sleeping for interval
						Thread.Sleep(new TimeSpan(0, 0, interval));
					}
				}
				
			} while (true);
		}



		/// <summary>
		/// Get's the next ResqueObject to process.
		/// Checks each queue in succession.
		/// </summary>
		/// <returns></returns>
		private ResqueObject Reserve()
		{
			foreach (string queue in Queues)
			{ 
				//Log Checking Queue
				ResqueClient q = ResqueClient.GetResque();
				ResqueObject item = q.Pop(queue);
				if (item != null)
				{
					//Log Found Item in Queue
					
					return item;
				}
			}

			return null;
		}


		private void WorkingOn(ResqueObject item)
		{ 
			//TODO: Add to redis, what this worker is working on. 
		}

		private void DoneWorking()
		{ 
			//TODO: Undo what we did in WorkingOn
		}

		public void ShutDown()
		{ 
			//Log exiting
			_shutDown = true;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text;
using ServiceStack.Text.Json;
using System.Reflection;

namespace Resque
{
	public class ResqueObject
	{
		public Exception exception { get; set; }

		public string Queue { get; set; }
		
		/// <summary>
		/// Set this to the Type you want the Job property to be
		/// deserialized as.
		/// </summary>
		public Type JobType {get; set;}

		/// <summary>
		/// This object should implement the ResqueJob interface
		/// </summary>
		public Object Job { get; set; }
		
		public Object GetJob()
		{
			//When deserialized this.Body is a string so use the serilaized
	        //this.Type to deserialize it back into the original type
			if (this.Job is string)
			{

				//Console.WriteLine(AppDomain.CurrentDomain);
				
				//Assembly assem = AppDomain.CurrentDomain.Load(this.JobType.Assembly.FullName);
				//Object o = assem.CreateInstance(this.JobType.FullName);
				
				//Console.WriteLine(o);

				Object temp = null ;
				if (TypeSerializer.CanCreateFromString(this.JobType))
				{
					Console.WriteLine("Should be able to do this");
					temp = TypeSerializer.DeserializeFromString((string)this.Job, this.JobType);
				}
				else
				{
					Console.WriteLine("Cannot Deserialize type {0}",this.JobType.Name);
				}
				

				return temp;
			}
			else
				return this.Job;
			
		}

		public void PerformJob()
		{
			var jobOject = this.GetJob();
			IResqueJob job = (IResqueJob)jobOject; 
			job.Perform();
		}

	}
}

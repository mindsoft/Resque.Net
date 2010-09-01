using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text;
using ServiceStack.Text.Json;

namespace Resque
{
	public class ResqueObject
	{
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
				return TypeSerializer.DeserializeFromString((string)this.Job, this.JobType);
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

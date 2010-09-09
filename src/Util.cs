using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;

namespace Resque
{
	public static class Util
	{
		private static ILog _log;
		public static ILog Log
		{
			get
			{
				if (_log == null)
				{
					LogManager.LogFactory = new ConsoleLogFactory();
					_log = LogManager.GetLogger(LogManager.LogFactory.GetType());
				}
				return _log;
			}
		}
	}
}

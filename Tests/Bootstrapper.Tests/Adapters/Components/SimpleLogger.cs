using System;

namespace Bootstrap.Tests.Adapters.Components
{
    public class SimpleLogger : ILogger
	{
		public void Log(string msg)
		{
			Console.WriteLine(msg);
		}
	}
}
using System;

namespace TheMovieDb
{
	public class Logger
	{
		public Logger ()
		{
		}
		
		public static void Log(string Message, params object[] args)
		{
			TriggerLogMessage(String.Format(Message, args));
		}		
		
		private static void TriggerLogMessage(string Message)
		{
			if(Message == null)
				return;
			string formatted = String.Format("{0} [{1}] - {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), System.Threading.Thread.CurrentThread.ManagedThreadId, Message);
			if(LogMessage != null)
				LogMessage(Message, formatted);
		}
		
		public delegate void LogMessage_EventHandler(string RawMessage, string FormattedMessage);		
		public static event LogMessage_EventHandler LogMessage;
	}
}


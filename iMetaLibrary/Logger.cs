using System;

namespace iMetaLibrary
{
	public class Logger
	{
		static Logger ()
		{
			TheMovieDb.Logger.LogMessage += HandleTheMovieDbLoggerLogMessage;
		}

		static void HandleTheMovieDbLoggerLogMessage (string RawMessage, string FormattedMessage)
		{
			TriggerLogMessage(RawMessage);
		}
		
		public static void Log(string Message, params object[] args)
		{
			TriggerLogMessage(Message.FormatStr(args));
		}		
		
		private static void TriggerLogMessage(string Message)
		{
			if(Message == null)
				return;
			string formatted = "{0} [{1}] - {2}".FormatStr(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), System.Threading.Thread.CurrentThread.ManagedThreadId, Message);
			if(LogMessage != null)
				LogMessage(Message, formatted);
		}
		
		public delegate void LogMessage_EventHandler(string RawMessage, string FormattedMessage);		
		public static event LogMessage_EventHandler LogMessage;
	}
}


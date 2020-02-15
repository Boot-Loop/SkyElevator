using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
	public class Logger
	{
		/* *********** USAGE ***************
		 * 
		 * // recomented to create logger statically
		 * var logger = Logger("my logger");
		 * logger.log("hello world!");
		 * 
		 * 
		 * *********************************
		 */

		public static Logger logger = new Logger();
		private static String time_format = "HH:mm:ss";

		public enum LogLevel
		{
			LEVEL_SUCCESS   = 0,
			LEVEL_INFO		= 1,
			LEVEL_WARNING	= 2,
			LEVEL_ERROR		= 3,
		}

		private string logger_name	= null;
		private string file_path	= null;
		private LogLevel level = LogLevel.LEVEL_SUCCESS;

		/// <summary>
		/// if the file name is null -> logs to stdout
		/// </summary>
		/// <param name="path"></param>
		public Logger(String file_name = null) {
			var date_time = DateTime.Now.ToString(time_format);
			if (file_name != null) {
				this.logger_name	= file_name;
				this.file_path		= Path.Combine(Paths.LOGS, logger_name + ".log");
				using (var writer = new StreamWriter(file_path)) {
					writer.WriteLine(String.Format("[{0}] Logger Initialized", date_time));
				}
			}
		}

		public string getLoggerName()	=> logger_name;
		public string getFilePath()		=> file_path;

		public LogLevel getLevel()		=> this.level;
		public void setLevel(LogLevel level) => this.level = level;

		public void log(object log_msg, LogLevel level = LogLevel.LEVEL_INFO) {
			if (this.level <= level) {
				if (file_path != null) {
					string log_str = String.Format("[{0}] {1, 7}: {2}", DateTime.Now.ToString(time_format), levelToString(level), log_msg.ToString());
					File.AppendAllText(file_path, log_str+"\n");
				}
				else {
					string log_str = String.Format("[{0}] {1}", DateTime.Now.ToString(time_format), log_msg.ToString());
					consoleColoredLog(log_str, level);	
				}
			}
		}
		public void logSuccess(object log_msg)	=> log(log_msg, LogLevel.LEVEL_SUCCESS);
		public void logInfo(object log_msg)		=> log(log_msg, LogLevel.LEVEL_INFO);
		public void logWarning(object log_msg)	=> log(log_msg, LogLevel.LEVEL_WARNING);
		public void logError(object log_msg)	=> log(log_msg, LogLevel.LEVEL_ERROR);

		/********** PRIVATE *********/
		private static string levelToString(LogLevel level) {
			switch (level) {
				case LogLevel.LEVEL_INFO:		return "Info";
				case LogLevel.LEVEL_SUCCESS:	return "Success";
				case LogLevel.LEVEL_WARNING:	return "Warning";
				case LogLevel.LEVEL_ERROR:		return "Error";
				default:						return "Info";
			}
		}
		private static void consoleColoredLog(String log_str, LogLevel level)
		{
			switch ( level )
			{
				case LogLevel.LEVEL_INFO:
					Console.WriteLine(log_str);
					break;
				case LogLevel.LEVEL_SUCCESS: {
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.WriteLine(log_str);
					Console.ResetColor();
					break;
				}
				case LogLevel.LEVEL_WARNING: {
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine(log_str);
					Console.ResetColor();
					break;
				}
				case LogLevel.LEVEL_ERROR: {
					Console.BackgroundColor = ConsoleColor.DarkRed;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine(log_str);
					Console.ResetColor();
					break;
				}
			}
		}
	}
}

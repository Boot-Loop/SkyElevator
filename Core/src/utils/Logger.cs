﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.src;

namespace Core.utils
{
	public class Logger
	{
		/* *********** USAGE ***************
		 * 
		 * // recomented to create logger statically
		 * var logger = Logger.makeLogger("ui_logging");
		 * logger.log("hello world!");
		 * 
		 * // and in another place use the logger as
		 * var logger = Logger.getLogger("ui_logger");
		 * logger.log("some ui error", Logger.LogLevel.LEVEL_ERROR);
		 * 
		 * *********************************
		 */

		private static Dictionary<String, Logger> loggers = new Dictionary<String, Logger>();
		private static String time_format = "HH:mm:ss";

		public static Logger makeLogger(String file_name = null) {
			Logger logger = new Logger(file_name);
			if (file_name != null) loggers.Add(file_name, logger);
			return logger;
		}
		public static Logger getLogger(String file_name) {
			if (loggers.ContainsKey(file_name)) {
				return loggers[file_name];
			}
			throw new Exception("the logger file : "+ file_name + " does not exists");
		}

		public enum LogLevel
		{
			LEVEL_SUCCESS   = 0,
			LEVEL_INFO		= 1,
			LEVEL_WARNING	= 2,
			LEVEL_ERROR		= 3,
		}

		private String file_path = null; // if the file path is null -> logs to stdout
		private LogLevel level = LogLevel.LEVEL_SUCCESS;

		/// <summary>
		/// if the file name is null -> logs to stdout
		/// </summary>
		/// <param name="path"></param>
		public Logger(String file_name = null) {
			var date_time = DateTime.Now.ToString(time_format);
			if (file_name != null) {
				file_path = Path.Combine(Ref.LOGS_PATH, file_name + ".log");
				using (var writer = new StreamWriter(file_path)) {
					writer.WriteLine(String.Format("[{0}] Logger Initialized", date_time));
				}
			} else {
				Console.WriteLine(String.Format("[{0}] Logger Initialized", date_time));
			}
		}

		public void setLevel(LogLevel level) => this.level = level;

		public void log(string log_msg, LogLevel level = LogLevel.LEVEL_INFO) {
			if (this.level <= level) {
				string log_str = String.Format("[{0}] {1}", DateTime.Now.ToString(time_format), log_msg);
				if (file_path != null)
					File.AppendAllText(file_path, log_str+"\n");
				else
					consoleColoredLog(log_str, level);
				
			}
		}

		/********** PRIVATE *********/
		private void consoleColoredLog(String log_str, LogLevel level)
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
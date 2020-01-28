using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace SebeClient
{
	public class Utils
	{
		public static string getFirstMatch(string text, string expr)
		{
			MatchCollection mc = Regex.Matches(text, expr);
			foreach (Match m in mc){
				return m.ToString();
			}
			return null;
		}
	}
}

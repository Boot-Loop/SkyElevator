using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

/* use this as an api for your own validator or 
 * any 3rd party validator library.
 */
namespace Core.Utils
{
	public class Validator
	{

		public static bool validateEmail(String email) {
            Regex regex = new Regex(@"^[a-zA-Z_\-0-9]+@[a-zA-Z_\-0-9]+\.[a-zA-Z]+$");
            Match match = regex.Match(email);
            return match.Success;
            // return Regex.Matches(email, "$[A-Za-z0-9]+@[A-Za-z0-9]+\.com^");

        }

        public static bool validateFilePath(string path, bool is_new = false) {
            if (is_new) {
                path = new DirectoryInfo(System.IO.Path.GetDirectoryName(path)).FullName;
                return Directory.Exists(path);
            } else return File.Exists(path);
        }
		//bool validateName();
		//bool validatePhoneNumber();
		//bool validateAddress();
	}


}

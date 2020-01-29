using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* use this as an api for your own validator or 
 * any 3rd party validator library.
 */
namespace Core.utils
{
	public interface IValidator
	{
		bool validateEmail(String email);
		//bool validateName();
		//bool validatePhoneNumber();
		//bool validateAddress();
	}

	public class ValidatorYourOwn : IValidator
	{
		public bool validateEmail(String email) {
			/* your own implimentation */
			// return Regex.Matches(email, "$[A-Za-z0-9]+@[A-Za-z0-9]+\.com^");
			throw new NotImplementedException();
		}
	}

	public class Validator3rdParty : IValidator
	{
		public bool validateEmail(String email) {
			/* 3rd party implimentation */
			// return someVender.Validator.validateEmail(email);
			throw new NotImplementedException();
		}
	}
}

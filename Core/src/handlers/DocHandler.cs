using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.handlers
{
    public sealed class DocHandler
    {
        private DocHandler() { }

        private static readonly string[] FILE_TYPES = { "Inquiry Sheet", "Quotation", "Sales Agreement", "Handover" };

        private static readonly DocHandler singelton = new DocHandler();
        public static DocHandler Instance
        {
            get
            {
                return singelton;
            }
        }

        public void manipulateDoc(string source_file, string destination_file, Dictionary<string, string> replacement_dictionary)
        {
            /* source_file:             Source of the Doc file to be manipulated.
             * destination_file:        Destination where manipulated file to be stored.
             * replacement_dictionary:  Placeholder text and the replacxement text dictionary.
             */

            // Implement this method to manipulate Doc files.
        }

        public Dictionary<string, string> readDoc(string source_file, string type_of_file)
        {
            /* source_file:   Source of the Doc file to be read.
             * type_of_file:  Type of file to be read,(Types are specified above and the file template is known).
             */

            // Implement this method to read Doc files and return a dictionary.

            return new Dictionary<string, string>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramCarInsurance.Domain.Static
{
    /// <summary>
    /// Static class with all error's types
    /// </summary>
    public static class StaticErrors
    {
        public static string DefaultError => "{0} oops, something went wrong";
        public static string UnsupportedTypeMessage => "{0} sorry, but I don't support {1} type of message";
        public static string NotExistingCommand => "{0} sorry, but I don't support {1} command";
        public static string UnsupportedTypeDocument => "{0} sorry, but I don't support the type of documents what you sent. What does it {1} mean?";
        public static string UnknownTypeDocument => "{0} sorry, but I don't understand which document you sent. Could you please send it again with the specified document type?";


    }
}

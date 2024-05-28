using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class UnsupportedTypeMessageException : Exception
    {
        private static readonly string ErrorMessage = StaticErrors.UnsupportedTypeMessage;

        public UnsupportedTypeMessageException(string userName, string document)
            : base(string.Format(ErrorMessage, userName, document))
        {
        }

        public UnsupportedTypeMessageException(string userName, string document, Exception innerException)
            : base(string.Format(ErrorMessage, userName, document), innerException)
        {
        }
    }
}

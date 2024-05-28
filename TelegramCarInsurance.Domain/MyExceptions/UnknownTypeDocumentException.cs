using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class UnknownTypeDocumentException : Exception
    {
        private static readonly string ErrorMessage = StaticErrors.UnknownTypeDocument;

        public UnknownTypeDocumentException(string userName)
            : base(string.Format(ErrorMessage, userName))
        {
        }

        public UnknownTypeDocumentException(string userName, Exception innerException)
            : base(string.Format(ErrorMessage, userName), innerException)
        {
        }
    }
}

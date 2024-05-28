using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class UnsupportedTypeDocumentException : Exception
    {
        private static readonly string ErrorMessage = StaticErrors.UnsupportedTypeDocument;

        public UnsupportedTypeDocumentException(string userName, string document)
            : base(string.Format(ErrorMessage, userName, document))
        {
        }

        public UnsupportedTypeDocumentException(string userName, string document, Exception innerException)
            : base(string.Format(ErrorMessage, userName, document), innerException)
        {
        }
    }
}

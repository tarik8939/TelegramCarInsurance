using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class NotUploadedDocumentException : Exception
    {
        private static readonly string ErrorMessage = StaticErrors.NotUploadedDocument;

        public NotUploadedDocumentException(string userName, string command)
            : base(string.Format(ErrorMessage, userName, command))
        {
        }

        public NotUploadedDocumentException(string userName, string command, Exception innerException)
            : base(string.Format(ErrorMessage, userName, command), innerException)
        {
        }
    }
}

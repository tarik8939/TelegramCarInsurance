using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class NotExistingCommandException : Exception
    {
        private static readonly string ErrorMessage = StaticErrors.NotExistingCommand;

        public NotExistingCommandException(string userName, string command)
            : base(string.Format(ErrorMessage, userName, command))
        {
        }

        public NotExistingCommandException(string userName, string command, Exception innerException)
            : base(string.Format(ErrorMessage, userName, command), innerException)
        {
        }
    }
}

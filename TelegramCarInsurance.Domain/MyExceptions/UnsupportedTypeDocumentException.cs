using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class UnsupportedTypeDocumentException : Exception
    {
        private static string ErrorMessage => StaticErrors.UnsupportedTypeDocument;

        /// <summary>
        /// Button menu for error message
        /// </summary>
        public ReplyKeyboardMarkup? KeyboardButtons { get; set; }

        public UnsupportedTypeDocumentException(string userName, string document, ReplyKeyboardMarkup? keyboard = null)
            : base(string.Format(ErrorMessage, userName, document))
        {
            KeyboardButtons = keyboard;
        }
    }
}

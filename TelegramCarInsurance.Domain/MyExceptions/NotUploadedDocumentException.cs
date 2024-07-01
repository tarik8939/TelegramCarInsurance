using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class NotUploadedDocumentException : Exception
    {
        private static string ErrorMessage => StaticErrors.NotUploadedDocument;

        /// <summary>
        /// Button menu for error message
        /// </summary>
        public ReplyKeyboardMarkup? KeyboardButtons { get; set; }

        public NotUploadedDocumentException(string userName, string command, ReplyKeyboardMarkup? keyboard = null)
            : base(string.Format(ErrorMessage, userName, command))
        {
            KeyboardButtons = keyboard;
        }
    }
}

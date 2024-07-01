using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class GenerateAnswerException : Exception
    {
        private static string ErrorMessage => StaticErrors.GenerateAnswerError;

        /// <summary>
        /// Button menu for error message
        /// </summary>
        public ReplyKeyboardMarkup? KeyboardButtons { get; set; }

        public GenerateAnswerException(string userName, ReplyKeyboardMarkup? keyboard = null)
            : base(string.Format(ErrorMessage, userName))
        {
            KeyboardButtons = keyboard;
        }
    }
}

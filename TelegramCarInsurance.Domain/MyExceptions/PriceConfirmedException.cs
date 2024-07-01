using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class PriceConfirmedException : Exception
    {
        private static string ErrorMessage => StaticErrors.NotConfirmedPrice;

        /// <summary>
        /// Button menu for error message
        /// </summary>
        public ReplyKeyboardMarkup? KeyboardButtons { get; set; }

        public PriceConfirmedException(string userName, ReplyKeyboardMarkup? keyboard = null)
            : base(String.Format(ErrorMessage, userName))
        {
            KeyboardButtons = keyboard;
        }
    }
}

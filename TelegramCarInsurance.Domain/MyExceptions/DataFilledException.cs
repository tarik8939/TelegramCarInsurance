using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.MyExceptions
{
    public class DataFilledException : Exception
    {
        /// <summary>
        /// Button menu for error message
        /// </summary>
        public ReplyKeyboardMarkup? KeyboardButtons { get; set; }

        public DataFilledException(string errorMesage, ReplyKeyboardMarkup? keyboard = null)
            : base(errorMesage)
        {
            KeyboardButtons = keyboard;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramCarInsurance.Domain.Static
{
    /// <summary>
    /// Static class with all keyboards
    /// </summary>
    public static class Keyboard
    {
        public static ReplyKeyboardMarkup BasicButtonMarkup = new ReplyKeyboardMarkup(new[]
        {
            new []
            {
                new KeyboardButton(text:CommandsName.WatchDataCommand),
                new KeyboardButton(text:CommandsName.ConfirmDataCommand)
            },
            new []
            {
                new KeyboardButton(text:CommandsName.QuestionCommand),
            },

        })
        { ResizeKeyboard = true };

        public static ReplyKeyboardMarkup ConfirmationMarkup = new ReplyKeyboardMarkup(new[]
        {
            new []
            {
                new KeyboardButton(text:CommandsName.GeneratePolicyCommand),
                new KeyboardButton(text:CommandsName.GeneratePriceQuotationCommand),
            },
            new []
            {
                new KeyboardButton(text:CommandsName.WatchDataCommand),
                new KeyboardButton(text:CommandsName.QuestionCommand),
            },

        })
        { ResizeKeyboard = true };

        public static ReplyKeyboardMarkup PriceConfirmationMarkup = new ReplyKeyboardMarkup(new[]
            {
                new []
                {
                    new KeyboardButton(text:CommandsName.PriceAgreeCommand),
                    new KeyboardButton(text:CommandsName.PriceDisagreeCommand)
                },
            }) 
        { ResizeKeyboard = true };
    }
}

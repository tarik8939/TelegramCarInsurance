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
        public static InlineKeyboardMarkup BasicButtonMarkup = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(CommandsName.WatchDataCommand),
                InlineKeyboardButton.WithCallbackData(CommandsName.ConfirmDataCommand)
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(CommandsName.QuestionCommand),
            },
        });

        public static InlineKeyboardMarkup ConfirmationMarkup = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(CommandsName.GeneratePolicyCommand),
                InlineKeyboardButton.WithCallbackData(CommandsName.GeneratePriceQuotationCommand),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(CommandsName.WatchDataCommand),
                InlineKeyboardButton.WithCallbackData(CommandsName.QuestionCommand),
            },

        });

        public static InlineKeyboardMarkup PriceConfirmationMarkup = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(CommandsName.PriceAgreeCommand),
                InlineKeyboardButton.WithCallbackData(CommandsName.PriceDisagreeCommand)
            },
        });
    }
}

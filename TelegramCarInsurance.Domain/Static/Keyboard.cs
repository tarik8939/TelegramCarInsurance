﻿using System;
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
        public static ReplyKeyboardMarkup ConfirmButtonMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton(text:CommandsName.WatchDataCommand),
            new KeyboardButton(text:CommandsName.GeneratePriceQuotationCommand)

        })
        { OneTimeKeyboard = true, ResizeKeyboard = true };

        public static ReplyKeyboardMarkup PriceConfirmationMarkup = new ReplyKeyboardMarkup(new[]
        {
            new []
            {
                new KeyboardButton(text:CommandsName.GeneratePolicyCommand),
                new KeyboardButton(text:CommandsName.PriceDisagreeCommand)
            },
            new []
            {
                new KeyboardButton(text:CommandsName.WatchDataCommand),
            },

        })
        { OneTimeKeyboard = true, ResizeKeyboard = true };

    }
}
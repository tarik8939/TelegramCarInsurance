using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramCarInsurance.Domain.Abstractions
{
    public interface IErrorCommand
    {
        public Task Execute(Message message, string errorMessage, ReplyKeyboardMarkup? buttonsMenu = null);
    }
}

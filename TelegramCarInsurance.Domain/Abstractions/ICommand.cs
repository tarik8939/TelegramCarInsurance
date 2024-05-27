using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramCarInsurance.Domain.Abstractions
{
    public interface ICommand
    {
        /// <summary>
        /// TelegramBot instance
        /// </summary>
        public TelegramBotClient BotClient { get; set; }
        public string Name { get; }
        /// <summary>
        /// Method thad execute command's logic
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public Task Execute(Update update);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramCarInsurance.Domain.Abstractions
{
    public interface ICommand
    {
        /// <summary>
        /// TelegramBot instance
        /// </summary>
        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// Command's name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Method thad execute command's logic
        /// </summary>
        /// <param name="message"></param>
        public Task Execute(Message message);
    }
}

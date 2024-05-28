using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramCarInsurance.Domain.Abstractions;
using Telegram.Bot.Types.Enums;
using Mindee;
using Microsoft.Extensions.Configuration;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.Commands
{
    public class StartCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }
        public string Name => CommandsName.StartCommand;

        /// <summary>
        /// Constructor to initialize the StartCommand with dependencies
        /// </summary>
        /// <param name="botClient">Instance of TelegramBotClient</param>
        public StartCommand(TelegramBotClient botClient)
        {
            BotClient = botClient;
        }

        /// <summary>
        /// Executes the command to start bot
        /// </summary>
        /// <param name="message">Telegram message containing user request</param>
        public async Task Execute(Message message)
        {
            long chatId = message.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId, $"Hello {message.Chat.Username}, I'm Jarvis, your car insurance assistant bot");

            await BotClient.SendTextMessageAsync(chatId,
                "To apply for insurance, you need to upload a document of your passport with `/passport` caption " +
                "and a vehicle identification document with `/vehicle` caption and after all confirm data in button menu",
                replyMarkup: Keyboard.BasicButtonMarkup);
        }
    }
}

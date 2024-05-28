using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCarInsurance.Domain.Abstractions;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.Commands
{
    public class ErrorCommand : ICommand, IErrorCommand
    {
        public TelegramBotClient BotClient { get; set; }
        public string Name => CommandsName.ErrorCommand;
        public ErrorCommand(TelegramBotClient botClient)
        {
            BotClient = botClient;
        }

        /// <summary>
        /// Default error
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        async Task ICommand.Execute(Message message)
        {
            long chatId = message!.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId,
                String.Format(StaticErrors.NotExistingCommand,
                    message.Chat.Username));
        }

        /// <summary>
        /// Custom error 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        async Task IErrorCommand.Execute(Message message, string text)
        {
            long chatId = message!.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId,
                text);
        }
    }
}

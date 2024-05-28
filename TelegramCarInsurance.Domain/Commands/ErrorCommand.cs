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

        /// <summary>
        /// Constructor to initialize the bot client
        /// </summary>
        /// <param name="botClient"></param>
        public ErrorCommand(TelegramBotClient botClient)
        {
            BotClient = botClient;
        }
        // TODO:check this
        /// <summary>
        /// Default error handler
        /// </summary>
        /// <param name="message">The message that triggered the error</param>
        async Task ICommand.Execute(Message message)
        {
            long chatId = message!.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId,
                String.Format(StaticErrors.DefaultError,
                    message.Chat.Username));
        }

        /// <summary>
        /// Custom error handler
        /// </summary>
        /// <param name="message">The message that triggered the error</param>
        /// <param name="text">The custom error message text</param>
        async Task IErrorCommand.Execute(Message message, string text)
        {
            long chatId = message!.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId,
                text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
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
        /// <param name="errorMessage">The custom error message text</param>
        /// <param name="buttonsMenu">Menu for commands</param>
        async Task IErrorCommand.Execute(Message message, string errorMessage, ReplyKeyboardMarkup? buttonsMenu)
        {
            long chatId = message!.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId,
                errorMessage,
                replyMarkup: buttonsMenu);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCarInsurance.Domain.Abstractions;
using TelegramCarInsurance.Domain.MyExceptions;
using TelegramCarInsurance.Domain.Static;
using TelegramCarInsurance.Domain.Storage;

namespace TelegramCarInsurance.Domain.Commands
{
    internal class PriceAgreeCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// UserDataStorage instance to manage user data
        /// </summary>
        private UserDataStorage Storage { get; set; }
        public string Name => CommandsName.PriceAgreeCommand;

        /// <summary>
        /// Constructor to initialize the PriceAgreeCommand with dependencies
        /// </summary>
        /// <param name="botClient">Instance of TelegramBotClient</param>
        /// <param name="storage">Instance of UserDataStorage</param>
        public PriceAgreeCommand(TelegramBotClient botClient, UserDataStorage storage)
        {
            BotClient = botClient;
            Storage = storage;
        }

        /// <summary>
        /// Executes the command to agree with price command
        /// </summary>
        /// <param name="message">Telegram message containing user request</param>
        public async Task Execute(Message message)
        {
            long chatId = message.Chat.Id;

            // Retrieve user data based on chat ID
            var userData = Storage.GetData(chatId);

            if (userData.IsDataFilled())
            {
                if (userData.IsPriceConfirmed)
                {
                    await BotClient.SendTextMessageAsync(chatId,
                        $"{message.Chat.Username}, you have already agreed with price",
                        replyMarkup: Keyboard.ConfirmationMarkup);
                }
                else
                {
                    userData.ConfirmPrice();

                    await BotClient.SendTextMessageAsync(chatId,
                        $"{message.Chat.Username} great, you successfully agreed with price, now you can generate policy",
                        replyMarkup: Keyboard.ConfirmationMarkup);
                }
            }
            else
            {
                throw new DataFilledException(
                    $"{(userData.LicensePlateDocument == null ?
                        String.Format(StaticErrors.DoNotHaveDocument, message.Chat.Username, "license plate") : null)}" +
                    $"{(userData.PassportDocument == null ?
                        String.Format(StaticErrors.DoNotHaveDocument, message.Chat.Username, "passport") : null)}",
                    Keyboard.BasicButtonMarkup);
            }
        }
    }
}

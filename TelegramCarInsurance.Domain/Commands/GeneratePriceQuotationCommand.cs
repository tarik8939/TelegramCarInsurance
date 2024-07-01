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
    public class GeneratePriceQuotationCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// UserDataStorage instance to manage user data
        /// </summary>
        private UserDataStorage Storage { get; set; }
        public string Name => CommandsName.GeneratePriceQuotationCommand;

        /// <summary>
        /// Constructor to initialize the GeneratePriceQuotationCommand with dependencies
        /// </summary>
        /// <param name="botClient">Instance of TelegramBotClient</param>
        /// <param name="storage">Instance of UserDataStorage</param>
        public GeneratePriceQuotationCommand(TelegramBotClient botClient, UserDataStorage storage)
        {
            Storage = storage;
            BotClient = botClient;
        }

        /// <summary>
        /// Executes the command to generate price quotation command
        /// </summary>
        /// <param name="message">Telegram message containing user request</param>
        public async Task Execute(Message message)
        {
            long chatId = message.Chat.Id;

            // Retrieve user data based on chat ID
            var userData = Storage.GetData(chatId);

            if (userData.IsPriceConfirmed)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    $"{message.Chat.Username}, you have already agreed with price",
                    replyMarkup: Keyboard.ConfirmationMarkup);
            }
            else if (userData.IsDataConfirmed)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    "Fixed price for all insurance is 100 USD. Do you agree with this price?",
                    replyMarkup: Keyboard.PriceConfirmationMarkup);
            }
            else throw new DataConfirmedException(message.Chat.Username, Keyboard.BasicButtonMarkup);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCarInsurance.Domain.Abstractions;
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

            try
            {
                // Retrieve user data based on chat ID
                var userData = Storage.GetData(chatId);

                if (userData.IsConfirmed)
                {
                    await BotClient.SendTextMessageAsync(chatId,
                        "Fixed price for all insurance is 100 USD. Do you agree with this price?",
                        replyMarkup: Keyboard.AgreePriceButtonMarkup);
                }
                else
                {
                    await BotClient.SendTextMessageAsync(chatId,
                        String.Format(StaticErrors.NotConfirmedData, message.Chat.Username),
                        replyMarkup: Keyboard.BasicButtonMarkup);
                }

            }
            catch (KeyNotFoundException e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    String.Format(e.Message, message.Chat.Username));
            }
            catch (Exception e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    String.Format(StaticErrors.DefaultError, message.Chat.Username));
            }
        }
    }
}

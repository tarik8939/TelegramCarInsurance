using Mindee.Product.Passport;
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
    public class WatchDataCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// UserDataStorage instance to manage user data
        /// </summary>
        private UserDataStorage Storage { get; set; }
        public string Name => CommandsName.WatchDataCommand;

        /// <summary>
        /// Constructor to initialize the WatchDataCommand with dependencies
        /// </summary>
        /// <param name="botClient">Instance of TelegramBotClient</param>
        /// <param name="storage">Instance of UserDataStorage</param>
        public WatchDataCommand(TelegramBotClient botClient, UserDataStorage storage)
        {
            BotClient = botClient;
            Storage = storage;
        }

        /// <summary>
        /// Executes the command to watch user's data
        /// </summary>
        /// <param name="message">Telegram message containing user request</param>
        public async Task Execute(Message message)
        {
            long chatId = message.Chat.Id;

            try
            {
                // Retrieve user data based on chat ID
                var userData = Storage.GetData(chatId);

                await BotClient.SendTextMessageAsync(chatId, 
                    $"{(userData.LicensePlateDocument == null ?
                        $"{message.Chat.Username} sorry, but i don't have data about your license plate, try upload it again\n" :
                        $"Car's license plate:\n{userData.LicensePlateDocument}")}" +
                    $"{(userData.PassportDocument == null ?
                        $"{message.Chat.Username} sorry, but i don't have data about your passport, try upload it again\n" :
                        $"Car's plate data:\n{userData.PassportDocument}")}");

                await BotClient.SendTextMessageAsync(chatId,
                    $"If data incorrect just send documents again, If correct - press Confirm button",
                    replyMarkup: Keyboard.BasicButtonMarkup);
            }
            catch (KeyNotFoundException e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    String.Format(e.Message, message.Chat.Username), 
                    replyMarkup: Keyboard.BasicButtonMarkup);
            }
            catch (Exception e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    String.Format(StaticErrors.DefaultError, message.Chat.Username), 
                    replyMarkup: Keyboard.BasicButtonMarkup);
            }

        }
    }
}

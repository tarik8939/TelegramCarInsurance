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
        /// UserDataStorage instance
        /// </summary>
        private UserDataStorage Storage { get; set; }
        public string Name => CommandsName.WatchDataCommand;
        public WatchDataCommand(TelegramBotClient botClient, UserDataStorage storage)
        {
            BotClient = botClient;
            Storage = storage;
        }
        
        public async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;

            try
            {
                var userData = Storage.GetData(chatId);
                await BotClient.SendTextMessageAsync(chatId,
                        $"Car's plate data:\n{userData.LicensePlateDocument.ToString()}" +
                        $"Your personal data:\n{userData.PassportDocument.ToString()}");

                await BotClient.SendTextMessageAsync(chatId,
                    $"If data incorrect just send document again, If correct - press Generate Price Quotation button",
                    replyMarkup: Keyboard.ConfirmButtonMarkup);
            }
            catch (Exception e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    e.Message);
            }

        }
    }
}

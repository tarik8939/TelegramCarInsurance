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
        /// UserDataStorage instance
        /// </summary>
        private UserDataStorage Storage { get; set; }
        public string Name => CommandsName.GeneratePriceQuotationCommand;

        public GeneratePriceQuotationCommand(TelegramBotClient botClient, UserDataStorage storage)
        {
            Storage = storage;
            BotClient = botClient;
        }

        public async Task Execute(Update update)
        {
            long chatId = update.Message!.Chat.Id;

            try
            {
                Storage.GetData(chatId);
                await BotClient.SendTextMessageAsync(chatId,
                    "Fixed price for all insurance is 100 USD. Do you agree with this price?", 
                    replyMarkup:Keyboard.PriceConfirmationMarkup);
            }
            catch (Exception e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    e.Message);
            }
        }
    }
}

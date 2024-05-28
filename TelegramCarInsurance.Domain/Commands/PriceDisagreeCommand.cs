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
    public class PriceDisagreeCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }
        public string Name => CommandsName.PriceDisagreeCommand;
        public PriceDisagreeCommand(TelegramBotClient botClient)
        {
            BotClient = botClient;
        }

        public async Task Execute(Message message)
        {
            long chatId = message.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId, $"{message.Chat.Username} sorry, but 100 USD is the only available price for a car insurance policy",
                replyMarkup:Keyboard.PriceConfirmationMarkup);

        }
    }
}

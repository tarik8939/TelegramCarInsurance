using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramCarInsurance.Domain.Abstractions;
using Telegram.Bot.Types.Enums;
using Mindee;
using Microsoft.Extensions.Configuration;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.Commands
{
    public class StartCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }
        public string Name => CommandsName.StartCommand;

        public StartCommand(TelegramBotClient botClient)
        {
            BotClient = botClient;
        }

        public async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId, "Hello, I'm Jarvis, your car insurance assistant bot");
            await BotClient.SendTextMessageAsync(chatId,
                "To apply for insurance, you need to upload a document of your passport with `/passport` caption " +
                "and a vehicle identification document with `/vehicle` caption and after all confirm data in button menu",
                parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboard.ConfirmButtonMarkup);
        }
    }
}

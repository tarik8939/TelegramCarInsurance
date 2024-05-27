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
    public class ErrorFileCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }
        public string Name => CommandsName.ErrorCommand;
        public ErrorFileCommand(TelegramBotClient botClient)
        {
            BotClient = botClient;
        }

        public async Task Execute(Update update)
        {
            long chatId = update.Message!.Chat.Id;

            await BotClient.SendTextMessageAsync(chatId,
            $"Oops, we don't support {update.Message.Type.ToString().ToLower()} as type of message", 
            replyMarkup:Keyboard.ConfirmButtonMarkup);
        }
    }
}

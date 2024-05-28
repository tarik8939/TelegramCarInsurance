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
    public class ConfirmDataCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }
        /// <summary>
        /// UserDataStorage instance
        /// </summary>
        private UserDataStorage Storage { get; set; }
        public string Name => CommandsName.ConfirmDataCommand;

        public ConfirmDataCommand(TelegramBotClient botClient, UserDataStorage storage)
        {
            BotClient = botClient;
            Storage = storage;
        }

        public async Task Execute(Message message)
        {
            long chatId = message.Chat.Id;

            try
            {
                var userData = Storage.GetData(chatId);
                if (userData.IsDataFilled())
                {
                    userData.ConfirmData();

                    await BotClient.SendTextMessageAsync(chatId,
                        $"${message.Chat.Username} great, your data successfully confirmed, now you can generate price quotation",
                        replyMarkup: Keyboard.PriceConfirmationMarkup);
                }
                else
                {
                    await BotClient.SendTextMessageAsync(chatId,
                        $"{(userData.LicensePlateDocument == null ? 
                            $"{message.Chat.Username} sorry, but i don't have data about your license plate, try upload it again" : null)}" +
                        $"{(userData.PassportDocument == null ? 
                            $"{message.Chat.Username} sorry, but i don't have data about your passport, try upload it again" : null)}", 
                        replyMarkup:Keyboard.ConfirmButtonMarkup);
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
                    e.Message);
            }




            //var asd = 5;
            //await BotClient.SendTextMessageAsync(update.MyChatMember.From.Id,
            //    $"Bye {update.MyChatMember.From.Username}, see you soon");
        }
    }
}

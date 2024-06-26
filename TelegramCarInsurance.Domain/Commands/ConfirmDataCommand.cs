﻿using System;
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
    public class ConfirmDataCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }
        /// <summary>
        /// UserDataStorage instance to manage user data
        /// </summary>
        private UserDataStorage Storage { get; set; }
        public string Name => CommandsName.ConfirmDataCommand;

        /// <summary>
        /// Constructor to initialize the ConfirmDataCommand with dependencies
        /// </summary>
        /// <param name="botClient">Instance of TelegramBotClient</param>
        /// <param name="storage">Instance of UserDataStorage</param>
        public ConfirmDataCommand(TelegramBotClient botClient, UserDataStorage storage)
        {
            BotClient = botClient;
            Storage = storage;
        }

        /// <summary>
        /// Executes the command to confirm with extracted data
        /// </summary>
        /// <param name="message">Telegram message containing user request</param>
        public async Task Execute(Message message)
        {
            long chatId = message.Chat.Id;

            // Retrieve user data based on chat ID
            var userData = Storage.GetData(chatId);

            if (userData.IsDataFilled())
            {
                if (userData.IsDataConfirmed)
                {
                    await BotClient.SendTextMessageAsync(chatId,
                        $"{message.Chat.Username}, you have already confirmed your data",
                        replyMarkup: Keyboard.ConfirmationMarkup);
                }
                else
                {
                    userData.ConfirmData();

                    await BotClient.SendTextMessageAsync(chatId,
                        $"{message.Chat.Username} great, your data successfully confirmed, now you can generate price quotation",
                        replyMarkup: Keyboard.ConfirmationMarkup);
                }
;
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

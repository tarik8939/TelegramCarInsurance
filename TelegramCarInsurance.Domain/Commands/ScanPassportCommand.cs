using Mindee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindee.Input;
using Mindee.Product.Passport;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using TelegramCarInsurance.Domain.Abstractions;
using static System.Net.WebRequestMethods;
using System.Net;
using TelegramCarInsurance.Domain.Storage;
using TelegramCarInsurance.Domain.Static;
using Microsoft.Extensions.Configuration;
using Mindee.Exceptions;

namespace TelegramCarInsurance.Domain.Commands
{
    public class ScanPassportCommand : ICommand
    {

        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// MindeeClient instance
        /// </summary>
        private MindeeClient MindeeClient { get; set; }
 
        /// <summary>
        /// UserDataStorage instance
        /// </summary>
        private UserDataStorage Storage { get; set; }   
        private IConfiguration Configuration { get; }    
        public string Name => CommandsName.ScanPassportCommand;

        public ScanPassportCommand(TelegramBotClient botClient, UserDataStorage storage, IConfiguration configuration)
        {
            BotClient = botClient;
            Storage = storage;
            Configuration = configuration;
            MindeeClient = new MindeeClient(Configuration["Mindee_API_Key"]);
        }

        public async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;

            var fileId = update.Message.Document?.FileId;
            var fileInfo = await BotClient.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;
            var uriToDownload = $"https://api.telegram.org/file/bot{Configuration["Telegram_token"]}/{filePath}";

            //Downloading document from telegram's server
            using (var httpClient = new HttpClient())
            {
                using (var httpResponse = await httpClient.GetAsync(uriToDownload))
                {
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        await BotClient.SendTextMessageAsync(chatId, 
                            "Unable to upload passport photo",
                            replyMarkup: Keyboard.ConfirmButtonMarkup);
                    }
                    
                    using (var contentStream = await httpResponse.Content.ReadAsStreamAsync())
                    {
                        try
                        {
                            string fileName = "myfile.jpg";
                            var inputSource = new LocalInputSource(contentStream, fileName);

                            var response = await MindeeClient
                                .ParseAsync<PassportV1>(inputSource);

                            var passportInfo = response.Document.Inference.Prediction;

                            Storage.AddData(chatId, passportInfo);

                            await BotClient.SendTextMessageAsync(chatId,
                                $"Extracted data from passport:\n{passportInfo}\nIf data incorrect just send document again",
                                replyMarkup: Keyboard.ConfirmButtonMarkup);
                        }
                        catch (MindeeException e)
                        {
                            await BotClient.SendTextMessageAsync(chatId,
                                $"{e.Message}",
                                replyMarkup: Keyboard.ConfirmButtonMarkup);
                        }
                        catch (Exception e)
                        {
                            await BotClient.SendTextMessageAsync(chatId,
                                $"{e.Message}",
                                replyMarkup: Keyboard.ConfirmButtonMarkup);
                        }
                    }
                }
            }
        }
    }
}

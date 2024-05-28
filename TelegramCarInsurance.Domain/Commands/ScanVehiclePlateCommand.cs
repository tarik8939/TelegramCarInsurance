﻿using Mindee.Input;
using Mindee;
using Mindee.Product.Passport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCarInsurance.Domain.Abstractions;
using Mindee.Product.Eu.LicensePlate;
using TelegramCarInsurance.Domain.Storage;
using TelegramCarInsurance.Domain.Static;
using Microsoft.Extensions.Configuration;
using Mindee.Exceptions;
using Telegram.Bot.Types.Enums;

namespace TelegramCarInsurance.Domain.Commands
{
    public class ScanVehiclePlateCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// MineeClient instance to extract data
        /// </summary>
        private MindeeClient MindeeClient { get; set; }

        /// <summary>
        /// UserDataStorage instance to manage user data
        /// </summary>
        private UserDataStorage Storage { get; set; }

        /// <summary>
        /// Property to hold the configuration settings
        /// </summary>
        private IConfiguration Configuration { get; }
        public string Name => CommandsName.ScanVehiclePlateCommand;

        /// <summary>
        /// Constructor to initialize the ScanVehiclePlateCommand with dependencies
        /// </summary>
        /// <param name="botClient">Instance of TelegramBotClient</param>
        /// <param name="storage">Instance of UserDataStorage</param>
        /// <param name="configuration">Configuration instance to retrieve OpenAI API key</param>
        public ScanVehiclePlateCommand(TelegramBotClient botClient, UserDataStorage storage, IConfiguration configuration)
        {
            BotClient = botClient;
            Storage = storage;
            Configuration = configuration;
            MindeeClient = new MindeeClient(Configuration["Mindee_API_Key"]);
        }

        /// <summary>
        /// Executes the command to scan license plate
        /// </summary>
        /// <param name="message">Telegram message containing user request</param>
        public async Task Execute(Message message)
        {

            long chatId = message.Chat.Id;
            string fileId;

            // Check the message type and get the file ID
            if (message.Type == MessageType.Photo)
            {
                fileId = message.Photo[2].FileId;
            }
            else
            {
                fileId = message.Document?.FileId;

            }

            // Get file information from Telegram
            var fileInfo = await BotClient.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;
            var uriToDownload = $"https://api.telegram.org/file/bot{Configuration["Telegram_token"]}/{filePath}";

            // Download the document from Telegram's server
            using (var httpClient = new HttpClient())
            {
                using (var httpResponse = await httpClient.GetAsync(uriToDownload))
                {
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        await BotClient.SendTextMessageAsync(chatId,
                            "Unable to upload license plate photo",
                            replyMarkup: Keyboard.BasicButtonMarkup);
                    }

                    // Read the content stream from the response
                    using (var contentStream = await httpResponse.Content.ReadAsStreamAsync())
                    {
                        try
                        {
                            string fileName = "myfile.jpg";
                            var inputSource = new LocalInputSource(contentStream, fileName);

                            // Parse the license plate data using MindeeClient
                            var response = await MindeeClient
                                .ParseAsync<LicensePlateV1>(inputSource);

                            // Extract vehicle information from the response
                            var vehicleInfo = response.Document.Inference.Prediction;

                            // Store the extracted data in user data storage
                            Storage.AddData(chatId, vehicleInfo);

                            await BotClient.SendTextMessageAsync(chatId,
                                $"Extracted data from vehicle plate:\n{vehicleInfo}\nIf data incorrect just send document again",
                                replyMarkup: Keyboard.BasicButtonMarkup);


                        }
                        catch (MindeeException e)
                        {
                            // Handle Mindee specific exceptions
                            await BotClient.SendTextMessageAsync(chatId,
                                $"{e.Message}",
                                replyMarkup: Keyboard.BasicButtonMarkup);
                        }
                        catch (Exception e)
                        {
                            await BotClient.SendTextMessageAsync(chatId,
                                $"{e.Message}",
                                replyMarkup: Keyboard.BasicButtonMarkup);
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramCarInsurance.Domain.Abstractions;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Completions;
using TelegramCarInsurance.Domain.Storage;
using TelegramCarInsurance.Domain.Static;
using Telegram.Bot.Types.Enums;

namespace TelegramCarInsurance.Domain.Commands
{
    public class GeneratePolicyCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// UserDataStorage instance
        /// </summary>
        private UserDataStorage Storage { get; set; }

        /// <summary>
        /// OpenAiAPI instance
        /// </summary>
        private OpenAIAPI OpenAiClient { get; set; }
        public string Name => CommandsName.GeneratePolicyCommand;

        public GeneratePolicyCommand(TelegramBotClient botClient, UserDataStorage storage, IConfiguration configuration)
        {
            BotClient = botClient;
            Storage = storage;
            OpenAiClient = new OpenAIAPI(configuration["OpenAi_API_Key"]);
        }

        public async Task Execute(Message message)
        {
            long chatId = message!.Chat.Id;
            
            await BotClient.SendChatActionAsync(chatId, ChatAction.UploadDocument);

            try
            {
                var userData = Storage.GetData(chatId);
                if (userData.IsConfirmed)
                {
                    // Generate document with OpenAI
                    string document = await GeneratePolicyDocumentAsync(userData);

                    // Create PDF file
                    var pdf = GeneratePDF(document);

                    await BotClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputFileStream(new MemoryStream(pdf), "Policy.pdf"),
                        caption: $"Here's your Insurance Policy Issuance {message.Chat.Username}.",
                        replyMarkup: Keyboard.ConfirmButtonMarkup
                    );
                }
                else
                {
                    await BotClient.SendTextMessageAsync(chatId,
                        $"${message.Chat.Username} sorry, but you didn't confirm your personal data, please press Confirm button",
                        replyMarkup: Keyboard.ConfirmButtonMarkup);
                }

            }
            catch (Exception e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    e.Message);
            }
        }

        /// <summary>
        /// Method for generate PDF file for policy
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        private byte[] GeneratePDF(string inputText)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter pdfWriter = new PdfWriter(ms);
                var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfWriter);
                var document = new iText.Layout.Document(pdfDocument);

                // Add text to the PDF
                document.Add(new Paragraph(inputText));

                document.Close();
                pdfDocument.Close();

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Method for generate policy document with OpenAI API
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<string> GeneratePolicyDocumentAsync(CarUserData userData)
        {
            var passport = userData.PassportDocument;
            var plate = userData.LicensePlateDocument;

            // Prepare a prompt for OpenAI to generate dummy data
            string prompt = $"Generate a dummy insurance policy document with the following values for the service customer: " +
                            $"ID Number = {passport.IdNumber.Value}, Given Name(s) = {passport.GivenNames}, Surname = {passport.Surname.Value}," +
                            $"Issue Date = {passport.IssuanceDate.Value}, Expiration Date = {passport.ExpiryDate.Value}," +
                            $"MRZ Line 1 = {passport.Mrz1.Value}, MRZ Line 2 = {passport.Mrz2.Value}, License plates = {plate.LicensePlates}," +
                            "price for insurance = $100.00, and make it looks like a real insurance policy with a fake company name, address, zip code, email, " +
                            "phone number, service name";

            // Specify the model and create a completion request
            var completionRequest = new CompletionRequest
            {
                Prompt = prompt,
                Model = "gpt-3.5-turbo-instruct",
                MaxTokens = 2000
            };

            // Request completion from OpenAI API
            try
            {
                var completionResult = await OpenAiClient.Completions.CreateCompletionAsync(completionRequest);
                var document = completionResult.Completions[0].Text;

                return document;
            }
            catch (Exception e)
            {
                throw new Exception("Oops, some trouble while generating insurance policy file");
            }

        }
    }
}

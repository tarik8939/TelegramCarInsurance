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
using TelegramCarInsurance.Domain.MyExceptions;

namespace TelegramCarInsurance.Domain.Commands
{
    public class GeneratePolicyCommand : ICommand
    {
        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// UserDataStorage instance to manage user data
        /// </summary>
        private UserDataStorage Storage { get; set; }

        /// <summary>
        /// Instance of OpenAiAPI
        /// </summary>
        private OpenAIAPI OpenAiClient { get; set; }
        public string Name => CommandsName.GeneratePolicyCommand;

        /// <summary>
        /// Constructor to initialize the GeneratePolicyCommand with dependencies
        /// </summary>
        /// <param name="botClient">Instance of TelegramBotClient</param>
        /// <param name="storage">Instance of UserDataStorage</param>
        /// <param name="openAiClient">Instance of OpenAiAPI</param>
        public GeneratePolicyCommand(TelegramBotClient botClient, UserDataStorage storage, OpenAIAPI openAiClient)
        {
            BotClient = botClient;
            Storage = storage;
            OpenAiClient = openAiClient;
        }

        /// <summary>
        /// Executes the command to generate and send an insurance policy document
        /// </summary>
        /// <param name="message">Telegram message containing user request</param>
        public async Task Execute(Message message)
        {
            long chatId = message.Chat.Id;

            // Send chat action to indicate document upload is in progress
            await BotClient.SendChatActionAsync(chatId, ChatAction.UploadDocument);

            try
            {
                // Retrieve user data based on chat ID
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
                        replyMarkup: Keyboard.BasicButtonMarkup
                    );
                }
                else
                {
                    await BotClient.SendTextMessageAsync(chatId,
                        String.Format(StaticErrors.NotConfirmedData, message.Chat.Username),
                        replyMarkup: Keyboard.PriceConfirmationMarkup);
                }

            }
            catch (KeyNotFoundException e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    String.Format(e.Message, message.Chat.Username), 
                    replyMarkup: Keyboard.PriceConfirmationMarkup);
            }
            catch (Exception e)
            {
                await BotClient.SendTextMessageAsync(chatId,
                    String.Format(StaticErrors.GeneratePolicyError, message.Chat.Username), 
                    replyMarkup: Keyboard.PriceConfirmationMarkup);
            }
        }

        /// <summary>
        /// Method for generating a PDF file for the policy
        /// </summary>
        /// <param name="inputText">The text content to be included in the PDF</param>
        /// <returns>Byte array representing the PDF file</returns>
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
        /// Method for generating a policy document using the OpenAI API
        /// </summary>
        /// <param name="userData">User data used to generate the document</param>
        /// <returns>A string representing the generated policy document</returns>
        /// <exception cref="Exception">Throws an exception if there is an issue generating the document</exception>
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
            var completionResult = await OpenAiClient.Completions.CreateCompletionAsync(completionRequest);

            return completionResult.Completions[0].Text;
        }
    }
}

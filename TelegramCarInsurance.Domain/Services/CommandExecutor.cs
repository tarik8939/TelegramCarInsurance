using Microsoft.Extensions.Configuration;
using Mindee;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramCarInsurance.Domain.Abstractions;
using TelegramCarInsurance.Domain.Commands;
using TelegramCarInsurance.Domain.MyExceptions;
using TelegramCarInsurance.Domain.Static;
using TelegramCarInsurance.Domain.Storage;

namespace TelegramCarInsurance.Domain.Services
{
    /// <summary>
    /// Class that is responsible for executing commands 
    /// </summary>
    public class CommandExecutor : ICommandExecutor
    {
        /// <summary>
        /// List with all commands
        /// </summary>
        private List<ICommand> Commands { get; }

        private RegexService RegexService { get; }

        /// <summary>
        /// MineeClient instance to extract data
        /// </summary>
        private MindeeClient MindeeClient { get; set; }

        /// <summary>
        /// Instance of OpenAiAPI
        /// </summary>
        private OpenAIAPI OpenAiClient { get; set; }

        public CommandExecutor(TelegramBot botClient, UserDataStorage userDataStorage, IConfiguration configuration, RegexService regexService)
        {
            RegexService = regexService;
            MindeeClient = new MindeeClient(configuration["Mindee_API_Key"]);
            OpenAiClient = new OpenAIAPI(configuration["OpenAi_API_Key"]);

            Commands = new List<ICommand>
            {
                new StartCommand(botClient.GetClient()),
                new PriceDisagreeCommand(botClient.GetClient()),
                new ErrorCommand(botClient.GetClient()),
                new QuestionCommand(botClient.GetClient(), OpenAiClient),
                new ConfirmDataCommand(botClient.GetClient(), userDataStorage),
                new PriceAgreeCommand(botClient.GetClient(), userDataStorage),
                new GeneratePriceQuotationCommand(botClient.GetClient(), userDataStorage),
                new WatchDataCommand(botClient.GetClient(), userDataStorage),
                new ScanPassportCommand(botClient.GetClient(), userDataStorage, configuration, MindeeClient),
                new ScanLicensePlateCommand(botClient.GetClient(), userDataStorage, configuration, MindeeClient),
                new GeneratePolicyCommand(botClient.GetClient(), userDataStorage, OpenAiClient)
            };
        }

        /// <summary>
        /// Method that calling command execution
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task GetUpdate(Update update)
        {
            // If statement for UpdateType
            if (update.Type == UpdateType.Message)
            {
                Message msg = update.Message;

                try
                {
                    // If statement for MessageType.Text messages
                    if (msg.Type == MessageType.Text)
                    {
                        // Checking for the existence of a command
                        try
                        {
                            // If statement for a question
                            if (RegexService.IsQuestion(msg.Text))
                            {
                                await Commands
                                    .First(x => x.Name == CommandsName.QuestionCommand)
                                    .Execute(msg);
                            }
                            else
                            {
                                var command = Commands
                                    .FirstOrDefault(x => RegexService.CompareCommand(x.Name, msg.Text));

                                if (command == null)
                                {
                                    throw new NotExistingCommandException(msg.Chat.Username, msg.Text);
                                }
                                else await command.Execute(msg);
                            }

                        }
                        catch (NotExistingCommandException e)
                        {
                            var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                            await command.Execute(msg, e.Message);
                        }
                        catch (NotUploadedDocumentException e)
                        {
                            var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                            await command.Execute(msg, e.Message);
                        }
                    }
                    // If statement for MessageType.Document messages
                    else if (msg.Type == MessageType.Document || msg.Type == MessageType.Photo)
                    {
                        try
                        {
                            // If statement for document's caption
                            if (msg.Caption != null)
                            {
                                try
                                {
                                    var command = Commands
                                        .FirstOrDefault(x => RegexService.CompareCommand(x.Name, msg.Caption));

                                    if (command == null)
                                    {
                                        throw new UnsupportedTypeDocumentException(msg.Chat.Username!,
                                            msg.Caption);
                                    }
                                    else await command.Execute(msg);
                                }
                                catch (UnsupportedTypeDocumentException e)
                                {
                                    var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                                    await command.Execute(msg, e.Message);
                                }
                            }
                            else
                            {
                                throw new UnknownTypeDocumentException(msg.Chat.Username!);
                            }
                        }
                        catch (UnknownTypeDocumentException e)
                        {
                            var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                            await command.Execute(msg, e.Message);
                        }
                    }
                    //If statement for unsupported type messages
                    else
                    {
                        throw new UnsupportedTypeMessageException(msg.Chat.Username!, msg.Type.ToString());
                    }
                }
                catch (UnsupportedTypeMessageException e)
                {
                    var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                    await command.Execute(msg!, e.Message);
                }
                catch (Exception)
                {
                    await Commands
                        .First(x => x.Name == CommandsName.ErrorCommand)
                        .Execute(msg!);
                }
                
            }
        }
    }
}

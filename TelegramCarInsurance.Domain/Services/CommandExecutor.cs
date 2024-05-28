using Microsoft.Extensions.Configuration;
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

        public CommandExecutor(TelegramBot botClient, UserDataStorage userDataStorage, IConfiguration configuration)
        {
            Commands = new List<ICommand>
            {
                new StartCommand(botClient.GetClient()),
                new PriceDisagreeCommand(botClient.GetClient()),
                new ErrorCommand(botClient.GetClient()),
                new ConfirmDataCommand(botClient.GetClient(), userDataStorage),
                new GeneratePriceQuotationCommand(botClient.GetClient(), userDataStorage),
                new WatchDataCommand(botClient.GetClient(), userDataStorage),
                new ScanPassportCommand(botClient.GetClient(), userDataStorage, configuration),
                new ScanVehiclePlateCommand(botClient.GetClient(), userDataStorage, configuration),
                new GeneratePolicyCommand(botClient.GetClient(), userDataStorage, configuration)
            };
        }

        /// <summary>
        /// Method that calling command execution
        /// </summary>
        /// <param name="update">gfhfgh</param>
        /// <returns></returns>
        public async Task GetUpdate(Update update)
        {
            //add exception for updateType
            if (update.Type == UpdateType.Message)
            {
                Message msg = update.Message;
                try
                {
                    // If statement for MessageType.Text messages
                    if (msg.Type == MessageType.Text)
                    {
                        //Checking for the existence of a command
                        try
                        {
                            var command = Commands
                                .FirstOrDefault(x => x.Name.ToLower() == msg.Text.ToLower());

                            if (command == null)
                            {
                                throw new NotExistingCommandException(msg.Chat.Username, msg.Text);
                            }
                            else await command.Execute(msg);
                        }
                        catch (NotExistingCommandException e)
                        {
                            var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                            await command.Execute(msg, e.Message);
                        }
                        catch (Exception e)
                        {
                            await Commands
                                .First(x => x.Name == CommandsName.ErrorCommand)
                                .Execute(msg);
                        }
                    }
                    // If statement for MessageType.Document messages
                    else if (msg.Type == MessageType.Document || msg.Type == MessageType.Photo)
                    {
                        try
                        {
                            if (msg.Caption != null)
                            {
                                try
                                {
                                    var command = Commands
                                        .FirstOrDefault(x => x.Name.ToLower() == msg.Caption.ToLower());

                                    if (command == null)
                                    {
                                        throw new UnsupportedTypeDocumentException(msg.Chat.Username,
                                            msg.Caption);
                                    }
                                    else await command.Execute(msg);
                                }
                                catch (UnsupportedTypeDocumentException e)
                                {
                                    var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                                    await command.Execute(msg, e.Message);
                                }
                                catch (Exception e)
                                {
                                    await Commands
                                        .First(x => x.Name == CommandsName.ErrorCommand)
                                        .Execute(msg);
                                }
                            }
                            else
                            {
                                throw new UnknownTypeDocumentException(msg.Chat.Username);
                            }
                        }
                        catch (UnknownTypeDocumentException e)
                        {
                            var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                            await command.Execute(msg, e.Message);
                        }
                        catch (Exception e)
                        {
                            await Commands
                                .First(x => x.Name == CommandsName.ErrorCommand)
                                .Execute(msg);
                        }

                    }
                    //If statement for unsupported type messages
                    else
                    {
                        throw new UnsupportedTypeMessageException(msg.Chat.Username, msg.Type.ToString());
                    }
                }
                catch (UnsupportedTypeMessageException e)
                {
                    var command = (IErrorCommand)Commands.First(x => x.Name == CommandsName.ErrorCommand);
                    await command.Execute(msg, e.Message);
                }
                catch (Exception e)
                {
                    await Commands
                        .First(x => x.Name == CommandsName.ErrorCommand)
                        .Execute(msg);
                }
                
            }

        }
    }
}

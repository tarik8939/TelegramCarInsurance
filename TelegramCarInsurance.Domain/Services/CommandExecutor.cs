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
        /// <summary>
        /// TelegramBot instance
        /// </summary>
        private TelegramBotClient TgBot { get; }
        /// <summary>
        /// UserDataStorage instance
        /// </summary>
        private UserDataStorage Storage { get; }
        private IConfiguration Configuration { get; }

        public CommandExecutor(TelegramBot botClient, UserDataStorage userDataStorage, IConfiguration configuration)
        {
            Configuration = configuration;
            Commands = new List<ICommand>
            {
                new StartCommand(botClient.GetClient()),
                new PriceDisagreeCommand(botClient.GetClient()),
                new ErrorFileCommand(botClient.GetClient()),
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
            Message msg = update.Message;

            // If statement for MessageType.Text messages
            if (msg.Type == MessageType.Text)
            {
                foreach (var command in Commands)
                {
                    if (command.Name == msg.Text)
                    {
                        await command.Execute(update);
                    }
                }
            }
            // If statement for MessageType.Document messages
            else if (msg.Type == MessageType.Document)
            {
                foreach (var command in Commands)
                {
                    if (command.Name == msg.Caption)
                    {
                        await command.Execute(update);
                    }
                }
            }
            // If statement for unsupported type messages
            else
            {
                await Commands
                    .FirstOrDefault(x => x.Name == CommandsName.ErrorCommand)!
                    .Execute(update);
            }

        }
    }
}

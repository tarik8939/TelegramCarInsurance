using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramCarInsurance.Domain.Services;

namespace TelegramCarInsurance.Controllers
{
    [ApiController]
    [Route("/")]
    public class BotController : ControllerBase
    {
        private  CommandExecutor CommandExecutor { get; }
     
        public BotController(CommandExecutor commandExecutor)
        {
            CommandExecutor = commandExecutor;
        }

        [HttpPost]
        public async void Update(Update update) 
        {
            await CommandExecutor.GetUpdate(update);
        }

    }
}


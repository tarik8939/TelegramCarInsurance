using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramCarInsurance.Domain;

/// <summary>
/// Class for TelegramBot
/// </summary>
public class TelegramBot
{
    private IConfiguration Configuration { get; set; }

    public TelegramBot(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Function that returns TelegramBot client
    /// </summary>
    /// <returns>TelegramBotClient</returns>
    public TelegramBotClient GetClient()
    {
        return new TelegramBotClient(Configuration["Telegram_token"]);
    }

}
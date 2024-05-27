using Microsoft.Extensions.Configuration;
using Telegram.Bot;

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
        var telegramBot = new TelegramBotClient(Configuration["Telegram_token"]);

        return telegramBot;
    }

}
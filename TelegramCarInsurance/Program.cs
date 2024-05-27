using Mindee.Extensions.DependencyInjection;
using TelegramCarInsurance.Domain;
using TelegramCarInsurance.Domain.Services;
using TelegramCarInsurance.Domain.Storage;

namespace TelegramCarInsurance;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddSingleton<TelegramBot>();
        builder.Services.AddSingleton<CommandExecutor>();
        builder.Services.AddSingleton<UserDataStorage>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
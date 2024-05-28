using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Completions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramCarInsurance.Domain.Abstractions;
using TelegramCarInsurance.Domain.Static;

namespace TelegramCarInsurance.Domain.Commands;

public class QuestionCommand : ICommand
{
    public TelegramBotClient BotClient { get; set; }
    /// <summary>
    /// OpenAiAPI instance
    /// </summary>
    private OpenAIAPI OpenAiClient { get; set; }
    public string Name => CommandsName.QuestionCommand;

    public QuestionCommand(TelegramBotClient botClient, IConfiguration configuration)
    {
        BotClient = botClient;
        OpenAiClient = new OpenAIAPI(configuration["OpenAi_API_Key"]);

    }

    public async Task Execute(Message message)
    {
        long chatId = message.Chat.Id;

        await BotClient.SendChatActionAsync(chatId, ChatAction.Typing);

        var answer = await GenerateAnswer(message.Text);

        await BotClient.SendTextMessageAsync(chatId,
                answer);

    }

    private async Task<string> GenerateAnswer(string question)
    {
        // Prepare a prompt for OpenAI to generate dummy data
        string prompt = $"{question}. Can you help me?.";

        // Specify the model and create a completion request
        var completionRequest = new CompletionRequest
        {
            Prompt = prompt,
            Model = "gpt-3.5-turbo-instruct",
            MaxTokens = 250
        };

        // Request completion from OpenAI API
        try
        {
            var completionResult = await OpenAiClient.Completions.CreateCompletionAsync(completionRequest);
            var answer = completionResult.Completions[0].Text;
            return answer;
        }
        catch (Exception e)
        {
            throw new Exception("Oops, some trouble while generating your answer");
        }
    }
}
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
    /// Instance of OpenAiAPI
    /// </summary>
    private OpenAIAPI OpenAiClient { get; set; }
    public string Name => CommandsName.QuestionCommand;

    /// <summary>
    /// Constructor to initialize the QuestionCommand with dependencies
    /// </summary>
    /// <param name="botClient">Instance of TelegramBotClient</param>
    /// <param name="configuration">Configuration instance to retrieve OpenAI API key</param>
    public QuestionCommand(TelegramBotClient botClient, IConfiguration configuration)
    {
        BotClient = botClient;
        OpenAiClient = new OpenAIAPI(configuration["OpenAi_API_Key"]);

    }

    /// <summary>
    /// Executes the command to ask ChatGPT a question
    /// </summary>
    /// <param name="message">Telegram message containing user request</param>
    public async Task Execute(Message message)
    {
        long chatId = message.Chat.Id;

        // Send typing action to indicate that the bot is responding
        await BotClient.SendChatActionAsync(chatId, ChatAction.Typing);

        // Generate an answer using the OpenAI API
        var answer = await GenerateAnswer(message.Text);

        // Send the generated answer back to the user
        await BotClient.SendTextMessageAsync(chatId,
                answer);

    }

    /// <summary>
    /// Sending answer to ChatGPT
    /// </summary>
    /// <param name="question"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task<string> GenerateAnswer(string question)
    {
        // Prepare a prompt for OpenAI to generate a response
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
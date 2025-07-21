using Azure;
using Azure.AI.OpenAI;
using DotNetEnv;
using ExpenseTracker.Application.Contracts.AI;
using ExpenseTracker.Application.Models.AI;
using OpenAI.Chat;

namespace ExpenseTracker.Infrastructure.AIService;

public class AiRequestor : IAiRequestor
{
    private readonly AzureOpenAIClient _azureClient;
    private readonly string _deploymentName;
    private readonly ChatCompletionOptions _requestOptions;

    public AiRequestor()
    {
        Env.TraversePath().Load();
        _deploymentName = "gpt-4.1-nano";

        var apiKey = Env.GetString("OPENAI_API_KEY");

        var endpoint1 = new Uri(Env.GetString("OPENAI_URL"));

        _azureClient = new AzureOpenAIClient(
            endpoint1,
            new AzureKeyCredential(apiKey));

        _requestOptions = new ChatCompletionOptions
        {
            MaxOutputTokenCount = 800,
            Temperature = 1.0f,
            TopP = 1.0f,
            FrequencyPenalty = 0.0f,
            PresencePenalty = 0.0f
        };
    }

    public async Task<string> GetSuggestionsForSavingsAsync(IReadOnlyList<UserTransaction> transactions,
        CancellationToken cancellation = default)
    {
        var chatClient = _azureClient.GetChatClient(_deploymentName);

        // to be refactored with a helper function
        var expenseSummary = """
                              Groceries: 320 AED  
                             Dining: 420 AED  
                             Entertainment: 280 AED  
                             Utilities: 540 AED  
                             Transport: 180 AED

                             Target: I am planning to save at least 500 AED in the next month.
                             """;
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                "You are a smart financial advisor. Review my expenses and suggest me possible ways to save money.."),
            new UserChatMessage(
                $"These are my expenses:\n{expenseSummary}\nSuggest 3 ways I can save money next month.")
        };

        var response = await chatClient.CompleteChatAsync(messages, _requestOptions, cancellation);
        return response.Value.Content[0].Text;
    }
}
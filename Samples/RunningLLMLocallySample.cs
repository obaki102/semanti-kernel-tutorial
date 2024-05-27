using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;

namespace Samples;

public class RunningLLMLocallySample
{
    public static async Task ShowAsync()
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("================================================");
        Console.WriteLine("       Sample:RunningLLMLocallySample");
        Console.WriteLine("================================================");

        // Just change the model that is available on your local machine.
        // You can use GetStreamingChatMessageContentsAsync!!!
        var modelId = "llama3";//"mistral";
 
        var endpoint = new Uri("http://localhost:11434");
#pragma warning disable SKEXP0010
        var kernelBuilder = Kernel.CreateBuilder();
        var kernel = kernelBuilder
            .AddOpenAIChatCompletion(
                modelId,
                endpoint,
                apiKey: null)
            .Build();

        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        var executionSettings = new OpenAIPromptExecutionSettings
        {
            MaxTokens = 2000,
            Temperature = 0.5,
        };

        var history = new ChatHistory();

        // Start the conversation
        Console.Write("User > ");
        string? userInput;
        while ((userInput = Console.ReadLine()) != null)
        {
            history.AddUserMessage(userInput);
            var result = chatService.GetStreamingChatMessageContentsAsync(
                                        history,
                                        executionSettings: executionSettings,
                                        kernel: kernel);


            // Stream the results
            string fullMessage = "";
            var first = true;
            await foreach (var content in result)
            {
                if (content.Role.HasValue && first)
                {
                    Console.Write("Assistant > ");
                    first = false;
                }
                Console.Write(content.Content);
                fullMessage += content.Content;
            }
            Console.WriteLine();

            // Add the message from the agent to the chat history
            history.AddAssistantMessage(fullMessage);

            // Get user input again
            Console.Write("User > ");
        }
    }


}

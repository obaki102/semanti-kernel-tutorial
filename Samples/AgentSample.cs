using Microsoft.SemanticKernel.Connectors.OpenAI;
using Samples.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;

namespace Samples;

public class AgentSample
{
    public static async Task ShowAsync()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("================================================");
        Console.WriteLine("       Sample:Agent");
        Console.WriteLine("================================================");

      
        var kernel = DefaultKernelBuilder.Build(builder =>
        {
            builder.Plugins.AddFromType<AuthorEmailPlanner>();
            builder.Plugins.AddFromType<EmailPlugin>();

        });
        // Retrieve the chat completion service from the kernel
        IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        // Create the chat history
        ChatHistory chatMessages = new ChatHistory("""
                You are a friendly assistant who likes to follow the rules. You will complete required steps
                and request approval before taking any consequential actions. If the user doesn't provide
                enough information for you to complete a task, you will keep asking questions until you have
                enough information to complete the task.
                """);



        while (true)
        {
            // Get user input
            System.Console.Write("User > ");
            chatMessages.AddUserMessage(Console.ReadLine()!);
            // Enable auto function calling
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            };

            // Get the response from the AI
            var result = chatCompletionService.GetStreamingChatMessageContentsAsync(
                chatMessages,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel);

            // Stream the results
            string fullMessage = "";
            await foreach (var content in result)
            {
                if (content.Role.HasValue)
                {
                    Console.Write("Assistant > ");

                }
                Console.Write(content.Content);
                fullMessage += content.Content;
            }
            Console.WriteLine();

            chatMessages.AddAssistantMessage(fullMessage);

        }

    }
}

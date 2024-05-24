using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Kernel = Microsoft.SemanticKernel.Kernel;
var configurationBuilder = new ConfigurationBuilder()
                                .AddUserSecrets("dcaf3079-8365-4fba-96ce-db6aaf6d7dbe");
IConfiguration configuration = configurationBuilder.Build();

var builder = Kernel.CreateBuilder();


var apiKey = configuration["ApiKey"];
var orgId = configuration["OrgId"];
string model = "gpt-3.5-turbo";

builder.AddOpenAIChatCompletion(model, apiKey, orgId);


var kernel = builder.Build();


const string skPrompt = @"
ChatBot can have a conversation with you about any topic.
It can give explicit instructions or say 'I don't know' if it does not have an answer.

{{$history}}
User: {{$userInput}}
ChatBot:";

var executionSettings = new OpenAIPromptExecutionSettings
{
    MaxTokens = 2000,
    Temperature = 0.7,
    TopP = 0.5
};

var chatFunction = kernel.CreateFunctionFromPrompt(skPrompt, executionSettings);

var history = "";
var arguments = new KernelArguments()
{
    ["history"] = history
};

var userInput = "Hi, I'm looking for book suggestions";
arguments["userInput"] = userInput;

var bot_answer = await chatFunction.InvokeAsync(kernel, arguments);

history += $"\nUser: {userInput}\nAI: {bot_answer}\n";
arguments["history"] = history;

Console.WriteLine(history);



Func<string, Task> Chat = async (string input) => {
    // Save new message in the arguments
    arguments["userInput"] = input;

    // Process the user message and get an answer
    var answer = await chatFunction.InvokeAsync(kernel, arguments);

    // Append the new interaction to the chat history
    var result = $"\nUser: {input}\nAI: {answer}\n";
    history += result;

    arguments["history"] = history;

    // Show the response
    Console.WriteLine(result);
};

await Chat("I would like a non-fiction book suggestion about Greece history. Please only list one book.");
Console.WriteLine(history);
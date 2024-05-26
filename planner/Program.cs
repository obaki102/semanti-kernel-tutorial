using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Planning.Handlebars;
using Kernel = Microsoft.SemanticKernel.Kernel;
var configurationBuilder = new ConfigurationBuilder()
                                .AddUserSecrets("dcaf3079-8365-4fba-96ce-db6aaf6d7dbe");
IConfiguration configuration = configurationBuilder.Build();

var builder = Kernel.CreateBuilder();


var apiKey = configuration["ApiKey"];
var orgId = configuration["OrgId"];
string model = "gpt-3.5-turbo";

builder.AddOpenAIChatCompletion(model, apiKey, orgId);

#pragma warning disable SKEXP0060
var kernel = builder.Build();
var planner = new HandlebarsPlanner();

string skPrompt = """
{{$input}}

Rewrite the above in the style of Shakespeare.
""";

var executionSettings = new OpenAIPromptExecutionSettings
{
    MaxTokens = 2000,
    Temperature = 0.7,
    TopP = 0.5
};

var shakespeareFunction = kernel.CreateFunctionFromPrompt(skPrompt, executionSettings, "Shakespeare");



var ask = @"Tomorrow is Valentine's day. I need to come up with a few date ideas.
She likes Shakespeare so write using his style. Write them in the form of a poem.";

var newPlan = await planner.CreatePlanAsync(kernel, ask);

Console.WriteLine("Updated plan:\n");
Console.WriteLine(newPlan);

#pragma warning disable SKEXP0060

var newPlanResult = await newPlan.InvokeAsync(kernel, new KernelArguments());

Console.WriteLine("New Plan results:\n");
Console.WriteLine(newPlanResult);
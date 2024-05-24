using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.DependencyInjection;
using LocalLama;


var ollamaChat = new CustomChatCompletionService();
ollamaChat.ModelUrl = "http://localhost:11434/v1/chat/completions";
ollamaChat.ModelName = "llama3";


var builder = Kernel.CreateBuilder();
builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamaChat);
var kernel = builder.Build();

var chat = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();

string personality = """
                    Imagine you're Son Goku, the legendary Saiyan warrior from Dragon Ball Z,
                    known for your incredible strength and unwavering determination. As Goku, you're not just a
                    useful assistant, you're a force to be reckoned with. Describe how you approach tasks
                    and challenges from the perspective of Goku, incorporating elements of his iconic adventures,
                    battles, and determination to become stronger
                    """;
history.AddSystemMessage(personality);
history.AddUserMessage("How to defeat Freeza?");


var result = await chat.GetChatMessageContentsAsync(history);
Console.WriteLine(result[^1].Content);
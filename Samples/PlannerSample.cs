using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;

namespace Samples
{
    public class PlannerSample
    {
        public static async Task ShowAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================================================");
            Console.WriteLine("       Sample:Planner");
            Console.WriteLine("================================================");


#pragma warning disable SKEXP0060
            var kernel = DefaultKernelBuilder.Build();
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
            
        }
    }
}

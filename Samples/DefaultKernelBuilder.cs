using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;
namespace Samples
{
    public class DefaultKernelBuilder
    {

        public static Kernel Build()
        {
            var builder = DefaultBuilder();
            return builder.Build();
        }

        public static Kernel Build(Action<IKernelBuilder> configureBuilder)
        {
            var builder = DefaultBuilder();
            configureBuilder(builder);
            return builder.Build();
        }

        private static IKernelBuilder DefaultBuilder()
        {
            var configurationBuilder = new ConfigurationBuilder().AddUserSecrets("dcaf3079-8365-4fba-96ce-db6aaf6d7dbe");
            IConfiguration configuration = configurationBuilder.Build();

            var apiKey = configuration["ApiKey"];
            var orgId = configuration["OrgId"];
            string model = "gpt-3.5-turbo";

            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(model, apiKey, orgId);
            return builder;
        }

    }
}

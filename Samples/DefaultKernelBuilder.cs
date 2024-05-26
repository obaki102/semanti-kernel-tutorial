using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;
namespace Samples
{
    public class DefaultKernelBuilder
    {

        public static Kernel Build()
        {

            var configurationBuilder = new ConfigurationBuilder().AddUserSecrets("dcaf3079-8365-4fba-96ce-db6aaf6d7dbe");
            IConfiguration configuration = configurationBuilder.Build();

            var builder = Kernel.CreateBuilder();
            var apiKey = configuration["ApiKey"];
            var orgId = configuration["OrgId"];
            string model = "gpt-3.5-turbo";
            builder.AddOpenAIChatCompletion(model, apiKey, orgId);


            return builder.Build();
        }

        public static Kernel Build(Action<IKernelBuilder> configureBuilder)
        {
            var configurationBuilder = new ConfigurationBuilder().AddUserSecrets("dcaf3079-8365-4fba-96ce-db6aaf6d7dbe");
            IConfiguration configuration = configurationBuilder.Build();

            string model = "gpt-3.5-turbo";
            var apiKey = configuration["ApiKey"];
            var orgId = configuration["OrgId"];

            var builder = Kernel.CreateBuilder();
            configureBuilder(builder);

            builder.AddOpenAIChatCompletion(model, apiKey, orgId);

            return builder.Build();
        }

    }
}

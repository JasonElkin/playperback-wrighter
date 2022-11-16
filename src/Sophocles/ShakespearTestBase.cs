using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Sophocles
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ShakespearTestBase : PageTest
    {
        protected IHost _host;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _host = CreateHostBuilder().Build();
            await _host.StartAsync();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _host.StopAsync();
        }

        public override BrowserNewContextOptions ContextOptions() =>
            new BrowserNewContextOptions()
            {
                RecordVideoDir = "../../../Videos"
            };

        private static IHostBuilder CreateHostBuilder()
        {
            return new HostBuilder().ConfigureHostConfiguration(config =>
            {
                // Make UseStaticWebAssets work
                var applicationPath = typeof(Shakespear.Program).Assembly.Location;
                var applicationDirectory = Path.GetDirectoryName(applicationPath);

                var name = Path.ChangeExtension(applicationPath, ".staticwebassets.runtime.json");

                var inMemoryConfiguration = new Dictionary<string, string>
                {
                    [WebHostDefaults.StaticWebAssetsKey] = name,
                };

            }).ConfigureUmbracoDefaults()
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseEnvironment("Development");
                    webHostBuilder.UseConfiguration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
                    webHostBuilder.UseSolutionRelativeContentRoot(typeof(Shakespear.Startup).Assembly.GetName().Name);
                    webHostBuilder.UseStaticWebAssets();
                    webHostBuilder.UseStartup<Shakespear.Startup>();
                });
        }


    }
}

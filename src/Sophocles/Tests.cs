using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright.NUnit;
using Shakespear;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Sophocles;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    private IHost _host;

    [SetUp]
    public async Task SetUp()
    {
        var builder = CreateHostBuilder();

        _host = builder.Build();

        await _host.StartAsync();
    }


    [Test]
    public async Task HomepageSnapshot()
    {
        await Page.GotoAsync("http://localhost:5000/");
    }

    [TearDown]
    public async Task TearDown()
    {
        await _host.StopAsync();
    }

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
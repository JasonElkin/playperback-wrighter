using Microsoft.Extensions.DependencyInjection;
using Shakespear.Dashboards;
using Umbraco.Cms.Core.Services;

namespace Sophocles;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class DashboardTests : ShakespearTestBase
{
    private ILocalizedTextService _localizedText => _host.Services.GetRequiredService<ILocalizedTextService>();

   [SetUp]
    public async Task LogInUmbraco()
    {
        await Page.GotoAsync("http://localhost:5000/umbraco#/login");
        var loginContainer = Page.Locator("form[name='vm.loginForm']");
        await loginContainer.GetByLabel("Email").FillAsync("admin@test.local");
        await loginContainer.GetByLabel("Password").FillAsync("FJg*5*F@XX");
        await loginContainer.GetByText("Login").ClickAsync();
    }

    [Test]
    public async Task Custom_Dashboard_Exists()
    {
        var dashboard = new JasonsPirateDashboard();

        foreach(var section in dashboard.Sections)
        {
            await Page.Locator("#applications").GetByText(section).ClickAsync();

            var dashboardName = _localizedText.Localize("dashboardTabs", dashboard.Alias);

            await Page.Locator(".umb-tabs-nav").GetByText(dashboardName).ClickAsync();

            await Expect(Page.Locator(".umb-dashboard__content h1")).ToHaveTextAsync("Ahoy!");
        }
    }
}

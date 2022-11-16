using Microsoft.Playwright;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace Shakespear
{
    internal class JustShootMe : INotificationAsyncHandler<ContentPublishedNotification>
    {
        private readonly IUmbracoContextFactory umbracoContextFactory;

        public JustShootMe(IUmbracoContextFactory umbracoContextFactory, IMediaService mediaService)
        {
            this.umbracoContextFactory = umbracoContextFactory;
        }

        public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken)
        {
            var home = notification.PublishedEntities.FirstOrDefault(x => x.ContentType.Alias == Home.ModelTypeAlias);

            if (home is null) return;

            using var umbracoContext = umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;

            var homeUrl = umbracoContext.Content?.GetById(home.Id)?.Url(mode: UrlMode.Absolute);

            if (homeUrl is null) return;

            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync();

            var page = await browser.NewPageAsync();

            await page.GotoAsync(homeUrl);

            var screenShot = await page.ScreenshotAsync( new() { FullPage = true, Path = $"./ScreenShots/Home-{DateTime.UtcNow:yyyy-MM-dd--HH-mm-ss}.png" });

            return;
        }
    }
}
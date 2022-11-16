using Umbraco.Cms.Core.Dashboards;

namespace Shakespear.Dashboards
{
    public class JasonsPirateDashboard : IDashboard
    {
        public string[] Sections => new[] { "content" };

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();

        public string? Alias => "jasonsPirateDashboard";

        public string View => "/pirate-dashboard/dashboard.html";
    }
}

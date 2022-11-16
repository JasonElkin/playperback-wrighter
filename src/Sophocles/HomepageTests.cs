using System.Text.RegularExpressions;

namespace Sophocles;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class HomepageTests : ShakespearTestBase
{

    [Test]
    public async Task Homepage_Has_Title()
    {
        await Page.GotoAsync("http://localhost:5000/");

        await Expect(Page).ToHaveTitleAsync(new Regex("Ahoy"));
    }
}
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace MyShop.PlaywrightTests;

[TestClass]
public class BooksPageTests : PageTest
{
    [TestMethod]
    public async Task Books_Page_Loads_And_Shows_Books()
    {
        // Arrange
        var baseUrl = "http://localhost:5186/";

        // Act
        await Page.GotoAsync(baseUrl);

        await Page.GetByRole(Microsoft.Playwright.AriaRole.Link, new() { Name = "Books", Exact = true })
            .ClickAsync();

        // Assert: URL contains /Books
        await Expect(Page).ToHaveURLAsync(new Regex(".*/Books.*", RegexOptions.IgnoreCase));

        // Assert: table has at least 1 row
        var rows = Page.Locator("table tbody tr");
        Assert.IsTrue(await rows.CountAsync() > 0);

        // Assert: at least one Details link exists (BookDetails?id=...)
        var detailsLinks = Page.Locator("a[href*='BookDetails?id=']");
        Assert.IsTrue(await detailsLinks.CountAsync() > 0);
    }
}

using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace MyShop.PlaywrightTests;

[TestClass]
public class BookDetailsTests : PageTest
{
    [TestMethod]
    public async Task Book_Details_Page_Loads_From_Books_Page()
    {
        // Arrange
        var baseUrl = "http://localhost:5186/";

        // Act
        await Page.GotoAsync(baseUrl);

        await Page.GetByRole(Microsoft.Playwright.AriaRole.Link, new() { Name = "Books", Exact = true })
            .ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex(".*/Books.*", RegexOptions.IgnoreCase));

        // Click the first Details link that navigates to /BookDetails?id=...
        var firstDetails = Page.Locator("a[href*='BookDetails?id=']").First;

        await firstDetails.ScrollIntoViewIfNeededAsync();
        await firstDetails.ClickAsync();

        // Assert: URL contains BookDetails (and usually id=)
        await Expect(Page).ToHaveURLAsync(new Regex(".*BookDetails.*", RegexOptions.IgnoreCase));

        // Assert: page rendered something meaningful (your details page uses <h2>)
        await Expect(Page.GetByRole(Microsoft.Playwright.AriaRole.Heading).First)
            .ToBeVisibleAsync();
    }
}

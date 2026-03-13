using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyShop.PlaywrightTests;

[TestClass]
public class HomePageTests : PageTest
{
    [TestMethod]
    public async Task HomePage_Loads_And_Shows_Book_Shop_Header()
    {
        // Arrange: setup data / URL
        var baseUrl = "http://localhost:5186/";

        // Act: do the user action
        await Page.GotoAsync(baseUrl);

        // Assert: visibly verify the behavior
        await Expect(Page).ToHaveTitleAsync("Martin's Book Shop");
        await Expect(Page.GetByText("Discount Books")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(Microsoft.Playwright.AriaRole.Link, new() { Name = "Books" }))
            .ToBeVisibleAsync();

        // DB-backed UI check: verify at least one book renders
        // AFTER the page loads, locate this element loading from the db
        var bookCards = Page.Locator(".card-body");
        Assert.IsTrue(await bookCards.CountAsync() > 0);
    }
}

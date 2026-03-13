using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace MyShop.PlaywrightTests;

[TestClass]
public class LoginPageTests : PageTest
{
    [TestMethod]
    public async Task Login_Page_Loads()
    {
        // Arrange
        var baseUrl = "http://localhost:5186/";

        // Act
        await Page.GotoAsync(baseUrl);

        // Click the actual Login link (Exact avoids strict-mode ambiguity)
        await Page.GetByRole(Microsoft.Playwright.AriaRole.Link, new() { Name = "Login", Exact = true })
            .ClickAsync();

        // Assert: URL includes /Auth/Login
        await Expect(Page).ToHaveURLAsync(new Regex(".*/Auth/Login.*", RegexOptions.IgnoreCase));

        // Assert: at least one input exists (username/email/password)
        var inputs = Page.Locator("input");
        Assert.IsTrue(await inputs.CountAsync() > 0);
    }
}

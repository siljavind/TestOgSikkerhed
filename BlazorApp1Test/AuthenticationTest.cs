using BlazorApp1.Components.Pages;
using BlazorApp1.Data;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BlazorApp1Test;

public class AuthenticationTest
{
    //private readonly UserManager<ApplicationUser> userManager
    private readonly AuthenticationStateProvider authStateProvider;
    public AuthenticationTest()
    {
        //userManager = GetUserManager();
        authStateProvider = new Mock<AuthenticationStateProvider>().Object;
    }

    [Fact]
    public void View_DisplaysNotLoggedInMessage_OnNotAuthorized()
    {
        //Arrange
        using var ctx = new TestContext();
        //ctx.Services.AddSingleton(userManager);
        ctx.AddTestAuthorization().SetNotAuthorized();

        //Act
        var cut = ctx.RenderComponent<Home>();
        var element = cut.Find("p");

        //Assert
        element.MarkupMatches("<p>You are not logged in.</p>"); // Same as Assert.Equal
        Assert.Equal("You are not logged in.", element.InnerHtml); // Same as MarkupMatches
    }

    [Fact]
    public void View_DisplaysLoggedInMessage_OnAuthorized()
    {
        //Arrange
        using var ctx = new TestContext();

        var username = "Test User";
        ctx.AddTestAuthorization().SetAuthorized(username);

        //Act
        var cut = ctx.RenderComponent<Home>();
        var element = cut.Find("p");

        //Assert
        element.MarkupMatches($"<p>Hello, {username}!</p>"); // Same as Assert.Equal
        Assert.Equal($"Hello, {username}!", element.InnerHtml); // Same as MarkupMatches
    }

    [Fact]
    public void View_DisplaysAdminMessage_OnAdmin()
    {
        //Arrange
        using var ctx = new TestContext();

        var username = "Admin User";
        ctx.AddTestAuthorization().SetAuthorized(username).SetRoles("Admin");

        //Act
        var cut = ctx.RenderComponent<Home>();
        var fragment = cut.FindAll("p");

        //Assert
        Assert.Equal(2, fragment.Count);
        Assert.Equal($"Hello, {username}!", fragment[0].InnerHtml);
        Assert.Equal("You are an admin.", fragment[1].InnerHtml);
    }

    [Fact]
    public void Code_AuthenticatedIsFalse_OnNotAuthorized()
    {
        //Arrange
        using var ctx = new TestContext();
        ctx.AddTestAuthorization().SetNotAuthorized();

        //Act
        var cut = ctx.RenderComponent<Home>();

        //Assert
        Assert.False(cut.Instance.isAuthenticated);
        Assert.False(cut.Instance.isAdmin);
    }

    [Fact]
    public void Code_AuthenticatedIsTrue_OnAuthorized()
    {
        //Arrange
        using var ctx = new TestContext();
        ctx.AddTestAuthorization().SetAuthorized("Test User");

        //Act
        var cut = ctx.RenderComponent<Home>();

        //Assert
        Assert.True(cut.Instance.isAuthenticated);
        Assert.False(cut.Instance.isAdmin);
    }

    [Fact]
    public void Code_AuthenticatedAndAdminIsTrue_OnAdmin()
    {
        //Arrange
        using var ctx = new TestContext();
        ctx.AddTestAuthorization().SetAuthorized("Admin User").SetRoles("Admin");

        //Act
        var cut = ctx.RenderComponent<Home>();

        //Assert
        Assert.True(cut.Instance.isAuthenticated);
        Assert.True(cut.Instance.isAdmin);
    }

    private UserManager<ApplicationUser> GetUserManager()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>().Object;
        return new UserManager<ApplicationUser>(
            userStore,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
    }
}
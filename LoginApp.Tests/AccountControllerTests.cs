using LoginApp.Controllers;
using LoginApp.Models;
using LoginApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace LoginApp.Tests;

public class AccountControllerTests
{
    [Fact]
    public async Task ForgotPassword_Post_RedirectsToConfirmation_WhenEmailNotFound()
    {
        var userManager = CreateUserManagerMock<ApplicationUser>();
        var signInManager = CreateSignInManagerMock<ApplicationUser>();
        var emailSender = new Mock<IEmailSender>();

        userManager.Setup(x => x.FindByEmailAsync("missing@example.com"))
            .ReturnsAsync((ApplicationUser?)null);

        var controller = new AccountController(userManager.Object, signInManager.Object, emailSender.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var model = new ForgotPasswordViewModel { Email = "missing@example.com" };

        var result = await controller.ForgotPassword(model);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("ForgotPasswordConfirmation", redirectResult.ActionName);

        emailSender.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    private static Mock<UserManager<TUser>> CreateUserManagerMock<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        return new Mock<UserManager<TUser>>(
            store.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<TUser>>().Object,
            Array.Empty<IUserValidator<TUser>>(),
            Array.Empty<IPasswordValidator<TUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<TUser>>>().Object);
    }

    private static Mock<SignInManager<TUser>> CreateSignInManagerMock<TUser>() where TUser : class
    {
        var userManager = CreateUserManagerMock<TUser>();
        var contextAccessor = new Mock<IHttpContextAccessor>();
        contextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        options.Setup(x => x.Value).Returns(new IdentityOptions());
        var logger = new Mock<ILogger<SignInManager<TUser>>>();
        var authSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
        var userConfirmation = new Mock<IUserConfirmation<TUser>>();

        return new Mock<SignInManager<TUser>>(
            userManager.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            options.Object,
            logger.Object,
            authSchemeProvider.Object,
            userConfirmation.Object);
    }
}

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
using System.Linq;
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

    [Fact]
    public async Task Register_Post_ReturnsView_WhenModelStateInvalid()
    {
        var userManager = CreateUserManagerMock<ApplicationUser>();
        var signInManager = CreateSignInManagerMock<ApplicationUser>();
        var emailSender = new Mock<IEmailSender>();

        var controller = new AccountController(userManager.Object, signInManager.Object, emailSender.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        controller.ModelState.AddModelError("Email", "Required");

        var model = new RegisterViewModel
        {
            FullName = "Test User",
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var result = await controller.Register(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
    }

    [Fact]
    public async Task Register_Post_RedirectsToDashboard_WhenCreateSucceeds()
    {
        var userManager = CreateUserManagerMock<ApplicationUser>();
        var signInManager = CreateSignInManagerMock<ApplicationUser>();
        var emailSender = new Mock<IEmailSender>();

        userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "Password123!"))
            .ReturnsAsync(IdentityResult.Success);

        signInManager.Setup(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null))
            .Returns(Task.CompletedTask);

        var controller = new AccountController(userManager.Object, signInManager.Object, emailSender.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var model = new RegisterViewModel
        {
            FullName = "Test User",
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var result = await controller.Register(model);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Dashboard", redirectResult.ControllerName);
        signInManager.Verify(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null), Times.Once);
    }

    [Fact]
    public async Task Login_Post_ReturnView_WhenUserNotFound()
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

        var model = new LoginViewModel
        {
            Email = "missing@example.com",
            Password = "Password123!",
            RememberMe = false
        };

        var result = await controller.Login(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public async Task Login_Post_RedirectsToDashboard_WhenPasswordSignInSucceeds()
    {
        var userManager = CreateUserManagerMock<ApplicationUser>();
        var signInManager = CreateSignInManagerMock<ApplicationUser>();
        var emailSender = new Mock<IEmailSender>();

        var user = new ApplicationUser { Email = "test@example.com", UserName = "test@example.com" };

        userManager.Setup(x => x.FindByEmailAsync("test@example.com"))
            .ReturnsAsync(user);

        signInManager.Setup(x => x.PasswordSignInAsync(user, "Password123!", false, true))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        var controller = new AccountController(userManager.Object, signInManager.Object, emailSender.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var model = new LoginViewModel
        {
            Email = "test@example.com",
            Password = "Password123!",
            RememberMe = false
        };

        var result = await controller.Login(model);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Dashboard", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Login_Post_ReturnsView_WhenModelStateInvalid()
    {
        var userManager = CreateUserManagerMock<ApplicationUser>();
        var signInManager = CreateSignInManagerMock<ApplicationUser>();
        var emailSender = new Mock<IEmailSender>();

        var controller = new AccountController(userManager.Object, signInManager.Object, emailSender.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        controller.ModelState.AddModelError("Email", "Required");

        var model = new LoginViewModel
        {
            Email = string.Empty,
            Password = "Password123!",
            RememberMe = false
        };

        var result = await controller.Login(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public async Task Register_Post_ReturnsView_WhenCreateFails()
    {
        var userManager = CreateUserManagerMock<ApplicationUser>();
        var signInManager = CreateSignInManagerMock<ApplicationUser>();
        var emailSender = new Mock<IEmailSender>();

        userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), "Password123!"))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid registration." }));

        var controller = new AccountController(userManager.Object, signInManager.Object, emailSender.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var model = new RegisterViewModel
        {
            FullName = "Test User",
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var result = await controller.Register(model);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
        Assert.False(controller.ModelState.IsValid);
        Assert.Contains(controller.ModelState.Values, v => v.Errors.Any(e => e.ErrorMessage == "Invalid registration."));
        signInManager.Verify(x => x.SignInAsync(It.IsAny<ApplicationUser>(), false, null), Times.Never);
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

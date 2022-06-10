namespace SimpleIdentity.Areas.Identity.Pages.Account;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

/// <summary>
/// View model for login page.
/// </summary>
[AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
public class LoginModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ILogger<LoginModel> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModel"/> class.
    /// </summary>
    /// <param name="signInManager">Sign-in manager for user login.</param>
    /// <param name="logger">Logger to use.</param>
    public LoginModel(
        SignInManager<ApplicationUser> signInManager,
        ILogger<LoginModel> logger)
    {
        this.signInManager = signInManager ?? throw new System.ArgumentNullException(nameof(signInManager));
        this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets view model for the input form on the login page.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = null!;

    /// <summary>
    /// Gets additional login methods available on this website.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; private set; } = null!;

    /// <summary>
    /// Gets or sets return url after login.
    /// </summary>
    public string ReturnUrl { get; set; } = null!;

    /// <summary>
    /// Gets or sets time in seconds to unlock user login.
    /// </summary>
    public int TimeToUnlock { get; set; }

    /// <summary>
    /// Gets a value indicating whether form should be disabled.
    /// </summary>
    public bool IsFormDisabled => this.TimeToUnlock > 0;

    /// <summary>
    /// Gets or sets error message associated with login process.
    /// </summary>
    [TempData]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Displays login page.
    /// </summary>
    /// <param name="returnUrl">URL to which return after success login process.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task OnGetAsync(string? returnUrl = null)
    {
        var loginId = this.Request.Cookies["simpleIdentityLoginId"];
        if (loginId != null)
        {
            var input = new InputModel();
            input.Login = loginId;
            this.Input = input;
        }

        if (!string.IsNullOrEmpty(this.ErrorMessage))
        {
            this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
        }

        returnUrl = returnUrl ?? this.Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        this.SetLastFailedAttemptTime();
        this.ReturnUrl = returnUrl;
    }

    /// <summary>
    /// Performs login process.
    /// </summary>
    /// <param name="returnUrl">URL to which return after success login process.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl = returnUrl ?? this.Url.Content("~/");

        if (!this.ModelState.IsValid)
        {
            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await this.signInManager.PasswordSignInAsync(this.Input.Login, this.Input.Password, this.Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            this.Response.Cookies.Append("simpleIdentityLoginId", this.Input.Login);
            this.logger.LogInformation("User logged in.");
            return this.LocalRedirect(returnUrl);
        }

        if (result.RequiresTwoFactor)
        {
            return this.RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = false });
        }

        if (result.IsLockedOut)
        {
            this.logger.LogWarning("User account locked out.");
            return this.RedirectToPage("./Lockout");
        }
        else
        {
            this.SetLastFailedAttemptTime();
            this.ModelState.AddModelError(string.Empty, "Invalid Login ID or Password.");
            this.ErrorMessage = "Invalid Login ID or Password.";
            return this.Page();
        }
    }

    private void SetLastFailedAttemptTime()
    {
        var lockoutDateString = this.HttpContext.Session.GetString("LastFailedAttempt");
        if (lockoutDateString != null)
        {
            var lockoutDate = DateTime.Parse(lockoutDateString);
            this.TimeToUnlock = 0;
            if (lockoutDate > DateTime.UtcNow)
            {
                var timeToUnlock = (lockoutDate - DateTime.UtcNow).TotalSeconds;
                this.TimeToUnlock = (int)timeToUnlock;
            }
        }
    }

    /// <summary>
    /// View model for input form.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Gets or sets login for the user.
        /// </summary>
        [Required]
        [Display(Name = "Login ID")]
        public string? Login { get; set; }

        /// <summary>
        /// Gets or sets password for the user.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user should be remembered.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}

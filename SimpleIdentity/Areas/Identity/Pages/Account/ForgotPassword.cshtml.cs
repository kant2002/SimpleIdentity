namespace SimpleIdentity.Areas.Identity.Pages.Account;

using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

/// <summary>
/// Page model for the forgot password page.
/// </summary>
[AllowAnonymous]
public class ForgotPasswordModel : PageModel
{
    private readonly UserManager userManager;
    private readonly EmailManager emailManager;
    private readonly ILogger<ForgotPasswordModel> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForgotPasswordModel"/> class.
    /// </summary>
    /// <param name="userManager">App user manager.</param>
    /// <param name="emailManager">Email manager.</param>
    /// <param name="logger">App logger.</param>
    public ForgotPasswordModel(UserManager userManager, EmailManager emailManager, ILogger<ForgotPasswordModel> logger)
    {
        this.userManager = userManager;
        this.emailManager = emailManager;
        this.logger = logger;
    }

    /// <summary>
    /// Gets or sets user login id.
    /// </summary>
    [Required]
    [BindProperty]
    public string? LoginId { get; set; }

    /// <summary>
    /// Submit form.
    /// </summary>
    /// <returns>Action result.</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        var user = await this.userManager.FindByNameAsync(this.LoginId!);
        if (user == null)
        {
            // Don't reveal that the user does not exist or is not confirmed
            this.logger.LogInformation("Reset password token generation was skiped for non existing user.");
            return this.RedirectToPage("./ForgotPasswordConfirmation");
        }

        var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = this.Url.Page(
            "/Account/ResetPassword",
            pageHandler: null,
            values: new { area = "Identity", code },
            protocol: this.Request.Scheme);

        var encoderUrl = HtmlEncoder.Default.Encode(callbackUrl);
        await this.emailManager.SendResetUserPasswordLinkAsync(user.Email, this.LoginId!, user.FirstName, user.LastName, encoderUrl);
        return this.RedirectToPage("./ForgotPasswordConfirmation");
    }
}
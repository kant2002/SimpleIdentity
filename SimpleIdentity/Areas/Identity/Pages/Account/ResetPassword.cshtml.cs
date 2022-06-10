namespace SimpleIdentity.Areas.Identity.Pages.Account;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

/// <summary>
/// Page model for reset password page.
/// </summary>
public class ResetPasswordModel : PageModel
{
    private readonly UserManager userManager;
    private readonly ILogger<ResetPasswordModel> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResetPasswordModel"/> class.
    /// </summary>
    /// <param name="userManager">App user manager.</param>
    /// <param name="logger">App logger.</param>
    public ResetPasswordModel(UserManager userManager, ILogger<ResetPasswordModel> logger)
    {
        this.userManager = userManager;
        this.logger = logger;
    }

    /// <summary>
    /// Gets or sets reset token.
    /// </summary>
    [Required]
    [BindProperty]
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets reset token.
    /// </summary>
    [BindProperty]
    [Required]
    public string? LoginId { get; set; }

    /// <summary>
    ///  Gets or sets password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [BindProperty]
    public string? Password { get; set; }

    /// <summary>
    ///  Gets or sets password confirmation.
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [BindProperty]
    [Required]
    public string? ConfirmPassword { get; set; }

    /// <summary>
    /// Get page model.
    /// </summary>
    /// <param name="code">Reset token.</param>
    /// <returns>Action result.</returns>
    public IActionResult OnGet(string? code = null)
    {
        if (code == null)
        {
            return this.BadRequest("A code must be supplied for password reset.");
        }

        this.Code = code;
        return this.Page();
    }

    /// <summary>
    /// Reset password.
    /// </summary>
    /// <returns>Action result.</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        if (this.Password != this.ConfirmPassword)
        {
            this.ModelState.AddModelError(nameof(this.ConfirmPassword), "The password and confirmation password do not match. ");
            return this.Page();
        }

        var user = await this.userManager.FindByNameAsync(this.LoginId!);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            this.logger.LogInformation("Password reset for non existing user is skipped.");
            return this.RedirectToPage("./ResetPasswordConfirmation");
        }

        var result = await this.userManager.ResetPasswordAsync(user, this.Code!, this.Password!);
        if (result.Succeeded)
        {
            return this.RedirectToPage("./ResetPasswordConfirmation");
        }

        foreach (var error in result.Errors)
        {
            this.ModelState.AddModelError(string.Empty, error.Description);
        }

        return this.Page();
    }
}
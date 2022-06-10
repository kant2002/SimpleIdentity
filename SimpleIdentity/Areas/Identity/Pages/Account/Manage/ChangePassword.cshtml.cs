namespace SimpleIdentity.Areas.Identity.Pages.Account.Manage;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

/// <summary>
/// Page for setting user profile.
/// </summary>
public class ChangePasswordModel : PageModel
{
    private readonly UserManager userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ILogger<ChangePasswordModel> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordModel"/> class.
    /// </summary>
    /// <param name="userManager">User manager for lookup user information.</param>
    /// <param name="signInManager">Sigin manager which manager sign-in operations.</param>
    /// <param name="logger">Logger for performing logging.</param>
    public ChangePasswordModel(
        UserManager userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<ChangePasswordModel> logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.logger = logger;
    }

    /// <summary>
    /// Gets or sets input model for data entry.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = null!;

    /// <summary>
    /// Gets or sets status message for the operation.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; } = null!;

    /// <summary>
    /// Gets or sets error message for the operation.
    /// </summary>
    public string ErrorMessage { get; set; } = null!;

    /// <summary>
    /// Displays page for changing password.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await this.userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
        }

        return this.Page();
    }

    /// <summary>
    /// Sets new password for the user.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        var user = await this.userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
        }

        var changePasswordResult = await this.userManager.ChangePasswordAsync(user, string.Empty, this.Input.NewPassword!);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
                this.ErrorMessage = error.Description;
            }

            return this.Page();
        }

        await this.signInManager.RefreshSignInAsync(user);
        this.logger.LogInformation("User changed their password successfully.");
        this.StatusMessage = "Your password has been changed.";

        return this.RedirectToPage();
    }

    /// <summary>
    /// View model for data entry.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Gets or sets value of the new password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string? NewPassword { get; set; }

        /// <summary>
        /// Gets or sets value which confirms new password.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}

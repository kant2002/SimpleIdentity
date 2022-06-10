namespace SimpleIdentity;

using SimpleIdentity.Models;

/// <summary>
/// Class for managing accounts context.
/// </summary>
public partial class AccountManager
{
    /// <summary>
    /// Retro Web database context.
    /// </summary>
    private readonly AccountsDbContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountManager"/> class.
    /// </summary>
    /// <param name="dbContext">Retro web database context.</param>
    public AccountManager(AccountsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Validates user password.
    /// </summary>
    /// <param name="login">Login which password should be validated.</param>
    /// <param name="password">Password for the login.</param>
    /// <param name="applicationName">Name of the application which perform user validation.</param>
    /// <returns>Information about user validation.</returns>
    [SqlMarshal("Admin_validate_user")]
    public partial UserValidationInformation ValidateUser(string login, string password);

    /// <summary>
    /// Gets user information by login.
    /// </summary>
    /// <param name="userId">Id of the user for which get user information.</param>
    /// <returns>Information about user validation.</returns>
    [SqlMarshal("Admin_sel_user")]
    public partial UserInformation SelectUserInformation(string userId);

    /// <summary>
    /// Gets user information by login.
    /// </summary>
    /// <param name="loginId">Id of the user for which get user information.</param>
    /// <returns>Information about user validation.</returns>
    [SqlMarshal("Admin_sel_user_by_login")]
    public partial UserInformation SelectUserInformationByName(string loginId);

    /// <summary>
    /// Validate user information.
    /// </summary>
    /// <param name="userId">Id of the user for which validate information.</param>
    /// <param name="passwordText">Password text.</param>
    /// <returns>Validation result.</returns>
    public ValidationResponse GetPasswordValidationResponse(
        int? userId,
        string? passwordText)
    {
        string? errorMessage;
        var errorCode = this.ValidatePassword(userId, passwordText, out errorMessage);
        return new ValidationResponse(errorCode, errorMessage);
    }

    /// <summary>
    /// Validate user information.
    /// </summary>
    /// <param name="userId">Id of the user for which validate information.</param>
    /// <param name="passwordText">Password text.</param>
    /// <param name="errorMessage">Error message text.</param>
    /// <returns>Error code.</returns>
    [SqlMarshal("Admin_validate_password")]
    public partial int ValidatePassword(
        int? userId,
        string? passwordText,
        out string? errorMessage);

    /// <summary>
    /// Validate user information.
    /// </summary>
    /// <param name="userId">Id of the user for which validate information.</param>
    /// <param name="passwordText">Password text.</param>
    [SqlMarshal("Admin_update_password")]
    public partial void UpdatePassword(
        int? userId,
        string? passwordText);

    /// <summary>
    /// Get user clients map.
    /// </summary>
    /// <param name="userId">User id.</param>
    /// <param name="token">Reset token.</param>
    [SqlMarshal("Admin_update_password_reset_token")]
    public partial void UpdatePasswordResetToken(int userId, string? token);

    /// <summary>
    /// Get user clients map.
    /// </summary>
    /// <param name="userId">User id.</param>
    /// <param name="token">Reset token.</param>
    /// <returns>Token.</returns>
    [SqlMarshal("Admin_valdate_password_reset_token")]
    public partial bool ValidatePasswordResetToken(int userId, string? token);
}

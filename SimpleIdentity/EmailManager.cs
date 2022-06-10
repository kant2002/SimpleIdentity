namespace SimpleIdentity;

using System.Threading.Tasks;

/// <summary>
/// Manages sending email.
/// </summary>
public class EmailManager
{
    /// <summary>
    /// Sends email about new user creation.
    /// </summary>
    /// <param name="userEmail">Email of the user for which user was created.</param>
    /// <param name="loginId">Login of the user.</param>
    /// <param name="firstName">First name of the user.</param>
    /// <param name="lastName">Last name of the user.</param>
    /// <param name="newPassword">New password for the user.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public Task SendCreateUserEmailAsync(
        string userEmail,
        string loginId,
        string? firstName,
        string? lastName,
        string newPassword)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sends email about new user creation.
    /// </summary>
    /// <param name="userEmail">Email of the user for which user was created.</param>
    /// <param name="loginId">Login of the user.</param>
    /// <param name="firstName">First name of the user.</param>
    /// <param name="lastName">Last name of the user.</param>
    /// <param name="newPassword">New password for the user.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public Task SendRegenerateUserPasswordAsync(
        string userEmail,
        string loginId,
        string? firstName,
        string? lastName,
        string newPassword)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sends email about new user creation.
    /// </summary>
    /// <param name="userEmail">Email of the user for which user was created.</param>
    /// <param name="loginId">Login of the user.</param>
    /// <param name="firstName">First name of the user.</param>
    /// <param name="lastName">Last name of the user.</param>
    /// <param name="callbackUrl">New password for the user.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public Task SendResetUserPasswordLinkAsync(
        string userEmail,
        string loginId,
        string? firstName,
        string? lastName,
        string callbackUrl)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sends email about new user creation.
    /// </summary>
    /// <param name="userEmail">Email of the user for which user was created.</param>
    /// <param name="loginId">Login of the user.</param>
    /// <param name="firstName">First name of the user.</param>
    /// <param name="lastName">Last name of the user.</param>
    /// <param name="newPassword">New password for the user.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public Task SendChangeUserLoginAsync(
        string userEmail,
        string loginId,
        string? firstName,
        string? lastName,
        string? newPassword)
    {
        return Task.CompletedTask;
    }
}

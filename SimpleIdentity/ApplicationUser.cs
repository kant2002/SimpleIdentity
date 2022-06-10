namespace SimpleIdentity;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Application user information.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Gets or sets type of the role.
    /// </summary>
    public string? RoleType { get; set; }

    /// <summary>
    /// Gets or sets user first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets user last name.
    /// </summary>
    public string? LastName { get; set; }
}

namespace SimpleIdentity.Models;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Information about user.
/// </summary>
public class UserInformation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserInformation"/> class.
    /// </summary>
    /// <param name="login">Login associated with the user.</param>
    public UserInformation(string login)
    {
        this.Login = login;
    }

    /// <summary>
    /// Gets or sets id of the user.
    /// </summary>
    [Column("user_id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets id of the login.
    /// </summary>
    [Column("login_id")]
    public string Login { get; set; }

    /// <summary>
    /// Gets or sets first name of the user.
    /// </summary>
    [Column("first_name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets last name of the user.
    /// </summary>
    [Column("last_name")]
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets email of the user.
    /// </summary>
    [Column("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets type of the role for the user.
    /// </summary>
    [Column("role_type")]
    public string? RoleType { get; set; }
}

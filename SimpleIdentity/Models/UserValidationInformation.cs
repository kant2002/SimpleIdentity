namespace SimpleIdentity.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Results of user validation.
/// </summary>
public class UserValidationInformation
{
    /// <summary>
    /// Gets or sets id of the user.
    /// </summary>
    [Column("user_id")]
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets role type returned from the server.
    /// </summary>
    [Column("role_type")]
    public string? RoleType { get; set; }
}

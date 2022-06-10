namespace SimpleIdentity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleIdentity.Models;

/// <summary>
/// Database context for identity access. Not used.
/// </summary>
public class IdentityContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityContext"/> class.
    /// </summary>
    /// <param name="options">Database context options.</param>
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }
}

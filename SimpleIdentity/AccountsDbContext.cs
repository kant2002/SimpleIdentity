namespace SimpleIdentity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleIdentity.Models;

/// <summary>
/// Database context for accessing accounts data.
/// </summary>
public partial class AccountsDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountsDbContext"/> class.
    /// </summary>
    /// <param name="options">Options for establishing database connection.</param>
    public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets db set for retreiving information about user validation.
    /// </summary>
    public DbSet<UserValidationInformation> UserValidationInformations { get; set; } = null!;

    /// <summary>
    /// Gets or sets db set for retreiving information about users.
    /// </summary>
    public DbSet<UserInformation> UserInformations { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInformation>(
            b =>
            {
                b.HasNoKey();
                b.Property(p => p.Login);
            });
        modelBuilder.Entity<UserValidationInformation>(
            b =>
            {
                b.HasNoKey();
            });
    }
}

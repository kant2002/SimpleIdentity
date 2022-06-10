using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace SimpleIdentity;

public class UserManager : UserManager<ApplicationUser>
{
    private readonly AccountManager dataAccess;
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserManager"/> class.
    /// </summary>
    /// <param name="dataAccess">Data access for pics data.</param>
    /// <param name="httpContextAccessor">HTTP context accessor.</param>
    /// <param name="store">The persistence store the manager will operate over.</param>
    /// <param name="optionsAccessor">The accessor used to access the Microsoft.AspNetCore.Identity.IdentityOptions.</param>
    /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
    /// <param name="userValidators">A collection of <see cref="IUserValidator{IdentityUser}"/> to validate users against.</param>
    /// <param name="passwordValidators">A collection of <see cref="IPasswordValidator{IdentityUser}"/> to validate passwords against.</param>
    /// <param name="keyNormalizer">The <see cref="ILookupNormalizer"/> to use when generating index keys for users.</param>
    /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
    /// <param name="services">The <see cref="IServiceProvider"/> used to resolve services.</param>
    /// <param name="logger">The logger used to log messages, warnings and errors.</param>
    public UserManager(
        AccountManager dataAccess,
        IHttpContextAccessor httpContextAccessor,
        IUserStore<ApplicationUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<ApplicationUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        this.dataAccess = dataAccess;
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc/>
    public override bool SupportsUserClaim => true;

    /// <inheritdoc/>
    public override bool SupportsUserSecurityStamp => false;

    /// <inheritdoc/>
    public override Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        var lastFailedAttemptCount = this.httpContextAccessor.HttpContext.Session.GetInt32("LastFailedAttemptCountr") ?? 0;
        var options = this.Options;
        if (lastFailedAttemptCount == options.Lockout.MaxFailedAccessAttempts)
        {
            var lockoutDateString = this.httpContextAccessor.HttpContext.Session.GetString("LastFailedAttempt");
            var lockoutDate = DateTime.Parse(lockoutDateString);
            if (lockoutDate > DateTime.UtcNow)
            {
                return Task.FromResult(false);
            }
        }

        var userValidation = this.dataAccess.ValidateUser(user.NormalizedUserName, password);
        var isPasswordValid = userValidation.UserId != 0;
        if (!isPasswordValid)
        {
            this.RegisterFailedAttempt(options.Lockout, lastFailedAttemptCount);
            return Task.FromResult(false);
        }

        user.Id = userValidation.UserId.ToString();
        this.ClearFailedAttempt();
        var userInformation = this.dataAccess.SelectUserInformation(user.Id);
        user.Email = userInformation.Email;
        user.NormalizedEmail = userInformation.Email;
        user.RoleType = userValidation.RoleType;
        user.FirstName = userInformation.FirstName;
        user.LastName = userInformation.LastName;
        user.UserName = userInformation.Login;
        return Task.FromResult(true);
    }

    /// <inheritdoc/>
    public override Task<ApplicationUser> FindByIdAsync(string userId)
    {
        this.ThrowIfDisposed();
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var userInformation = this.dataAccess.SelectUserInformation(userId);
        if (userInformation == null)
        {
            var lastFailedAttemptCount = this.httpContextAccessor.HttpContext.Session.GetInt32("LastFailedAttemptCountr") ?? 0;
            var options = this.Options;
            if (lastFailedAttemptCount == options.Lockout.MaxFailedAccessAttempts)
            {
                var lockoutDateString = this.httpContextAccessor.HttpContext.Session.GetString("LastFailedAttempt");
                var lockoutDate = DateTime.Parse(lockoutDateString);
                if (lockoutDate > DateTime.UtcNow)
                {
                    return Task.FromResult<ApplicationUser>(null!);
                }
            }

            this.RegisterFailedAttempt(options.Lockout, lastFailedAttemptCount);
            return Task.FromResult<ApplicationUser>(null!);
        }

        return Task.FromResult(new ApplicationUser()
        {
            Id = userInformation.Id.ToString(),
            Email = userInformation.Email,
            NormalizedEmail = userInformation.Email,
            UserName = userInformation.Login,
            NormalizedUserName = userInformation.Login,
            RoleType = userInformation.RoleType,
            EmailConfirmed = true,
            FirstName = userInformation.FirstName,
            LastName = userInformation.LastName,
        });
    }

    /// <inheritdoc/>
    public override Task<ApplicationUser> FindByNameAsync(string userName)
    {
        this.ThrowIfDisposed();
        if (userName == null)
        {
            throw new ArgumentNullException(nameof(userName));
        }

        userName = this.NormalizeName(userName);

        var userInformation = this.dataAccess.SelectUserInformationByName(userName);
        if (userInformation == null)
        {
            return Task.FromResult<ApplicationUser>(null!);
        }

        return Task.FromResult(new ApplicationUser()
        {
            Id = userInformation.Id.ToString(),
            Email = userInformation.Email,
            NormalizedEmail = userInformation.Email,
            UserName = userInformation.Login,
            NormalizedUserName = userInformation.Login,
            RoleType = userInformation.RoleType,
            EmailConfirmed = true,
            FirstName = userInformation.FirstName,
            LastName = userInformation.LastName,
        });
    }

    /// <summary>
    /// Resets the <paramref name="user"/>'s password to the specified <paramref name="newPassword"/> after
    /// validating the given password reset <paramref name="token"/>.
    /// </summary>
    /// <param name="user">The user whose password should be reset.</param>
    /// <param name="token">The password reset token to verify.</param>
    /// <param name="newPassword">The new password to set if reset token verification succeeds.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
    /// of the operation.
    /// </returns>
    public override async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
    {
        this.ThrowIfDisposed();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        // Make sure the token is valid and the stamp matches
        if (!await this.VerifyUserTokenAsync(user, this.Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, token))
        {
            return IdentityResult.Failed(this.ErrorDescriber.InvalidToken());
        }

        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (newPassword is null)
        {
            throw new ArgumentNullException(nameof(newPassword));
        }

        var userId = int.Parse(user.Id);
        var storedToken = this.dataAccess.ValidatePasswordResetToken(userId, token);
        if (!storedToken)
        {
            return IdentityResult.Failed(this.ErrorDescriber.InvalidToken());
        }

        var validation = this.dataAccess.GetPasswordValidationResponse(int.Parse(user.Id), newPassword);
        if (!validation.IsValid)
        {
            return IdentityResult.Failed(new IdentityError() { Code = "Failed", Description = validation.ErrorMessage });
        }

        try
        {
            this.dataAccess.UpdatePassword(int.Parse(user.Id), newPassword);
            this.dataAccess.UpdatePasswordResetToken(userId, null);
        }
        catch (Exception)
        {
            return IdentityResult.Failed(new IdentityError() { Code = "Failed", Description = "Change password failed" });
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// Generates a password reset token for the specified <paramref name="user"/>, using
    /// the configured password reset token provider.
    /// </summary>
    /// <param name="user">The user to generate a password reset token for.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation,
    /// containing a password reset token for the specified <paramref name="user"/>.</returns>
    public override async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
    {
        this.ThrowIfDisposed();
        var code = await this.GenerateUserTokenAsync(user, this.Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose);
        var userId = int.Parse(user.Id);
        this.dataAccess.UpdatePasswordResetToken(userId, code);
        return code;
    }

    /// <inheritdoc/>
    public override Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal)
    {
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var name = principal.FindFirstValue(ClaimTypes.Name);
        var email = principal.FindFirstValue(ClaimTypes.Email);
        var user = new ApplicationUser();
        user.Id = id;
        user.UserName = name;
        user.NormalizedUserName = name.ToUpperInvariant();
        user.Email = email;
        user.NormalizedEmail = email?.ToUpperInvariant();
        user.EmailConfirmed = true;
        return Task.FromResult(user);
    }

    /// <inheritdoc/>
    public override Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var claims = new List<Claim>();
        if (user.RoleType == "A" || user.RoleType == "a")
        {
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
        }

        claims.Add(new Claim(ClaimTypes.Sid, user.Id));
        return Task.FromResult<IList<Claim>>(claims);
    }

    /// <inheritdoc/>
    public override Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (currentPassword is null)
        {
            throw new ArgumentNullException(nameof(currentPassword));
        }

        if (newPassword is null)
        {
            throw new ArgumentNullException(nameof(newPassword));
        }

        var validation = this.dataAccess.GetPasswordValidationResponse(int.Parse(user.Id), newPassword);
        if (!validation.IsValid)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "Failed", Description = validation.ErrorMessage }));
        }

        try
        {
            this.dataAccess.UpdatePassword(int.Parse(user.Id), newPassword);
        }
        catch (Exception)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "Failed", Description = "Change password failed" }));
        }

        return Task.FromResult(IdentityResult.Success);
    }

    private void RegisterFailedAttempt(LockoutOptions options, int lastFailedAttemptCount)
    {
        this.httpContextAccessor.HttpContext.Session.SetInt32("LastFailedAttemptCountr", lastFailedAttemptCount + 1);
        var failedAttemptsThreshold = options.MaxFailedAccessAttempts;
        if (lastFailedAttemptCount >= failedAttemptsThreshold - 1)
        {
            var delay = options.DefaultLockoutTimeSpan;
            this.httpContextAccessor.HttpContext.Session.SetString("LastFailedAttempt", DateTime.UtcNow.Add(delay).ToString());
            this.httpContextAccessor.HttpContext.Session.SetInt32("LastFailedAttemptCountr", 0);
        }
    }

    private void ClearFailedAttempt()
    {
        this.httpContextAccessor.HttpContext.Session.SetInt32("LastFailedAttemptCountr", 0);
        this.httpContextAccessor.HttpContext.Session.Remove("LastFailedAttempt");
    }
}

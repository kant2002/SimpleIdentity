namespace SimpleIdentity.Models;

/// <summary>
/// Validation data.
/// </summary>
public class ValidationResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResponse"/> class.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    /// <param name="errorMessage">Error message.</param>
    public ValidationResponse(int errorCode, string? errorMessage)
    {
        this.ErrorMessage = errorMessage;
        this.ErrorCode = errorCode;
    }

    /// <summary>
    /// Gets or sets error code.
    /// </summary>
    public int ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets a value indicating whether model is valid.
    /// </summary>
    public bool IsValid => this.ErrorMessage == null ? true : false;
}

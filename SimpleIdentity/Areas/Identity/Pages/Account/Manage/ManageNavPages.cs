namespace SimpleIdentity.Areas.Identity.Pages.Account.Manage;

using System;
using Microsoft.AspNetCore.Mvc.Rendering;

/// <summary>
/// Helper classes for pages navigation.
/// </summary>
public static class ManageNavPages
{
    /// <summary>
    /// Gets name of the change password page.
    /// </summary>
    public static string ChangePassword => "ChangePassword";

    /// <summary>
    /// Gets navigation class for the page.
    /// </summary>
    /// <param name="viewContext">View context in which found active state for the page.</param>
    /// <returns>Navigation class for change password class.</returns>
    public static string? ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

    private static string? PageNavClass(ViewContext viewContext, string page)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string
            ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }
}

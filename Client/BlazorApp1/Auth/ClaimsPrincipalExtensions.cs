using System.Security.Claims;

namespace BlazorApp1.Auth;

public static class ClaimsPrincipalExtensions
{
    public const string UserIdClaimType = "Id";

    // Only call this for a logged-in user: anonymous users have no ID claim.
    public static int GetUserId(this ClaimsPrincipal user)
    {
        return int.Parse(user.FindFirst(UserIdClaimType)!.Value);
    }
}

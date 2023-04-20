using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            //to recognize the user we have to use ClaimTypes.NameIdentifier, not the JwtRegisteredClaimNames.NameId part even after registering with that
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            //to recognize the user we have to use ClaimTypes.NameIdentifier, not the JwtRegisteredClaimNames.NameId part even after registering with that
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}

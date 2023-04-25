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

        public static int GetUserId(this ClaimsPrincipal user)
        {
            //to recognize the user we have to use ClaimTypes.NameIdentifier, not the JwtRegisteredClaimNames.NameId part even after registering with that
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}

using System.Security.Claims;

namespace datingAPI.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal claimsPrincipal){
            var username = claimsPrincipal.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("Cannot get username from token");
            return username;
        }

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal){
            var username = int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get username from token"));
            return username;
        }
    }
}
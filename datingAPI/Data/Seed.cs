using System.Text.Json;
using datingAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace datingAPI.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager){
            if (await userManager.Users.AnyAsync()) return;

            var userdata = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userdata, options);

            if (users == null) return;

            var roles = new List<AppRole>{
                new() { Name = "Member"},
                new() { Name = "Admin"},
                new() { Name = "Moderator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName!.ToLower();
                await userManager.CreateAsync(user, "Password@123");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser{
                UserName = "admin",
                KnownAs = "Admin",
                Gender = "",
                City = "",
                Country = ""
            };

            await userManager.CreateAsync(admin, "Password@123");
            await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
        }
    }
}
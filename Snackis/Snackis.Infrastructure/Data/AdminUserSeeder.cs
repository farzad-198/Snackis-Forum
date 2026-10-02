using Microsoft.AspNetCore.Identity;
using Snackis.Core.Entities;

namespace Snackis.Infrastructure.Data
{
    public static class AdminUserSeeder
    {
        public static async Task SeedAsync(
            UserManager<AppUser> userManager,
            string email)
        {
            AppUser? user =
                await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "The configured admin account was not found.");
            }

            if (!user.EmailConfirmed)
            {
                throw new InvalidOperationException(
                    "Confirm the admin account email first.");
            }

            bool isAdmin =
                await userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                return;
            }

            IdentityResult result =
                await userManager.AddToRoleAsync(user, "Admin");

            if (!result.Succeeded)
            {
                string errors = string.Join(
                    ", ",
                    result.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Could not assign Admin role: {errors}");
            }
        }
    }
}
using Microsoft.AspNetCore.Identity;

namespace Snackis.Infrastructure.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole> roleManager)
        {
            bool roleExists =
                await roleManager.RoleExistsAsync("Admin");

            if (roleExists)
            {
                return;
            }

            IdentityRole adminRole =
                new IdentityRole("Admin");

            IdentityResult result =
                await roleManager.CreateAsync(adminRole);

            if (!result.Succeeded)
            {
                string errors = string.Join(
                    ", ",
                    result.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Could not create Admin role: {errors}");
            }
        }
    }
}
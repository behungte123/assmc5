using lab4.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Lab4.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager)
        {
            async Task EnsureUser(string email, string password, params string[] permissions)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(user, password);
                }

                var claims = await userManager.GetClaimsAsync(user);
                foreach (var p in permissions)
                {
                    if (!claims.Any(c => c.Type == "Permission" && c.Value == p))
                        await userManager.AddClaimAsync(user, new Claim("Permission", p));
                }
            }

            await EnsureUser(
                "admin@poly.edu.vn",
                "Admin@123",
                "Admin.Access",
                "Product.Create",
                "Inventory.Manage",
                "Order.Manage"
            );

            await EnsureUser(
                "sales@poly.edu.vn",
                "Sales@123",
                "Product.Create",
                "Order.Manage"
            );
        }
    }
}
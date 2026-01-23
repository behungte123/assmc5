using lab4.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Lab4.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager)
        {
            // 1. Tự động tạo và gán quyền cho Admin
            string adminEmail = "admin@poly.edu.vn";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(adminUser, "Admin@123"); // Tự động tạo user với mật khẩu mặc định
            }

            // Gán các Claim cho Admin theo yêu cầu [1], [3]
            var adminClaims = await userManager.GetClaimsAsync(adminUser);
            if (!adminClaims.Any(c => c.Type == "CreateProduct"))
                await userManager.AddClaimAsync(adminUser, new Claim("CreateProduct", "true")); 
            
            if (!adminClaims.Any(c => c.Type == "Admin"))
                await userManager.AddClaimAsync(adminUser, new Claim("Admin", "true")); 


            // 2. Tự động tạo và gán quyền cho Sales (Nhân viên kinh doanh)
            string salesEmail = "sales@poly.edu.vn";
            var salesUser = await userManager.FindByEmailAsync(salesEmail);

            if (salesUser == null)
            {
                salesUser = new ApplicationUser { UserName = salesEmail, Email = salesEmail, EmailConfirmed = true };
                await userManager.CreateAsync(salesUser, "Sales@123"); // Tự động tạo user
            }

            // Gán các Claim cho Sales theo yêu cầu [1], [3]
            var salesClaims = await userManager.GetClaimsAsync(salesUser);
            if (!salesClaims.Any(c => c.Type == "Sales"))
                await userManager.AddClaimAsync(salesUser, new Claim("Sales", "true")); 

            if (!salesClaims.Any(c => c.Type == "CreateProduct"))
                await userManager.AddClaimAsync(salesUser, new Claim("CreateProduct", "true")); 
        }
    }
}
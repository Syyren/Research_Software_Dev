using Microsoft.AspNetCore.Identity;
using Research_Software_Dev.Models.Researchers;

namespace Research_Software_Dev.Data
{
    public static class SeedRoles
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            // Get RoleManager and UserManager from the service provider
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Researcher>>();

            // Define roles
            var roles = new[] { "Study Admin", "Researcher", "Low-Auth", "Mid-Auth", "High-Auth" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed a Study Admin
            var adminEmail = "admin@example.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new Researcher
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    ResearcherFirstName = "Admin",
                    ResearcherLastName = "User",
                    ResearcherAddress = "123 Admin St.",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Study Admin");
            }

            // Seed a Researcher
            var researcherEmail = "researcher@example.com";
            var researcher = await userManager.FindByEmailAsync(researcherEmail);
            if (researcher == null)
            {
                researcher = new Researcher
                {
                    UserName = researcherEmail,
                    Email = researcherEmail,
                    ResearcherFirstName = "Researcher",
                    ResearcherLastName = "User",
                    ResearcherAddress = "456 Research Blvd.",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(researcher, "Researcher123!");
                await userManager.AddToRoleAsync(researcher, "Researcher");
            }
        }
    }
}

namespace Gradebook.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Gradebook.Common;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Models;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
            // await SeedRoleAsync(roleManager, GlobalConstants.PrincipalRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.TeacherRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.StudentRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.ParentRoleName);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

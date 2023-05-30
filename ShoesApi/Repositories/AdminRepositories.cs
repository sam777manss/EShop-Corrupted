using log4net;
using Microsoft.AspNetCore.Identity;
using ShoesApi.Interfaces;
using ShoesApi.Models;

namespace ShoesApi.Repositories
{
    public class AdminRepositories : IAdmin
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private static readonly ILog log = LogManager.GetLogger(typeof(UserRepositories));

        public AdminRepositories(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<List<AdminIndex>> AdminTables(string Uid)
        {
            try
            {
                AppUser current_user = await userManager.FindByIdAsync(Uid);
                if (current_user != null)
                {
                    var roles = await userManager.GetRolesAsync(current_user);
                    // 'roles' will contain the roles assigned to the user
                    if (roles.Contains("Admin"))
                    {
                        var users = userManager.Users;
                        List<AdminIndex> adminIndices = new List<AdminIndex>();
                        foreach (var user in users)
                        {
                            AdminIndex data = new AdminIndex()
                            {
                                Id = user.Id,
                                Name = user.UserName,
                                Email = user.Email,
                                Number = "",
                            };
                            adminIndices.Add(data);
                        }
                        return adminIndices;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return new List<AdminIndex>();
        }
    }
}

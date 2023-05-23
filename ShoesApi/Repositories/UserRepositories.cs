using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoesApi.Interfaces;
using ShoesApi.Models;

namespace ShoesApi.Repositories
{
    public class UserRepositories: IUser
    {
        private UserManager<AppUser> userManager;
        private static readonly ILog log = LogManager.GetLogger(typeof(UserRepositories));

        public async Task<bool> RegisterUser(Register register)
        {
            try
            {
                AppUser user = new AppUser()
                {
                    UserName = register.Name,
                    Email = register.Email,
                };
                IdentityResult Result = await userManager.CreateAsync(user, register.Password);
                if (Result.Succeeded)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return true;
        }
    }
}

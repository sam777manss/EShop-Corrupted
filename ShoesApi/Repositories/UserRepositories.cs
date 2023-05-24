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
        private SignInManager<AppUser> signInManager;
        private static readonly ILog log = LogManager.GetLogger(typeof(UserRepositories));
        // Need To be included in constructor all the interfaces and other dbcontext manager

        public UserRepositories(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

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
            return false;
        }

        public async Task<AdminIndex> LoginUser(Login login)
        {
            AdminIndex userData = null;
            try
            {
                AppUser user = await userManager.FindByEmailAsync(login.Email);               
                if (user != null)
                {
                    // sign out current user
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        userData = new AdminIndex() { Email = user.Email, Name = user.UserName, Id = user.Id };
                        return userData;
                    }
                }
                return userData;
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return userData;
        }
    }
}

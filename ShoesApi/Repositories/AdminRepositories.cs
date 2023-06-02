using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        private IUserValidator<AppUser> userValidator;
        // userValidator validate email and user name
        private IPasswordValidator<AppUser> passwordValidator;
        public AdminRepositories(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            RoleManager<IdentityRole> roleManager, IUserValidator<AppUser> userValidator, IPasswordValidator<AppUser> passwordValidator)
        {
            this.userValidator = userValidator; 
            this.passwordValidator = passwordValidator;

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

        public async Task<AppUser> Edit(string Id)
        {
            try
            {
                AppUser user = await userManager.FindByIdAsync(Id);

                if(user != null)
                {
                    return user;
                }
            }catch(Exception ex)
            {
                log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return new AppUser();
        }

        public async Task<bool> Delete(string Id)
        {
            try
            {
                AppUser user = await userManager.FindByIdAsync(Id);
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return false;
        }

        public async Task<IActionResult> SaveEdits(AppUser appUser)
        {
            try
            {
                if (appUser != null)
                {
                    AppUser user = await userManager.FindByIdAsync(appUser.Id);

                    if (!string.IsNullOrEmpty(appUser.Email))
                    {
                        IdentityResult validEmail = null;
                        validEmail = await userValidator.ValidateAsync(userManager, user);
                        if (validEmail.Succeeded)
                        {
                            user.State = appUser.State; user.Email = appUser.Email;
                            user.imageUrl = appUser.imageUrl; user.UserName = appUser.UserName;
                            user.PhoneNumber = appUser.PhoneNumber; user.UserSurname = appUser.UserSurname;
                            user.Country = appUser.Country; user.Zip_Code = appUser.Zip_Code;

                            IdentityResult result = await userManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                return new StatusCodeResult(200);
                            }
                        }
                        else
                        {
                            return new StatusCodeResult(401);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return new StatusCodeResult(500);
        }
    }
}

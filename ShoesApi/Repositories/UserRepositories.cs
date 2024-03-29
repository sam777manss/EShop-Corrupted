﻿using log4net;
using Microsoft.AspNetCore.Identity;
using ShoesApi.Interfaces;
using ShoesApi.Models;

namespace ShoesApi.Repositories
{
    public class UserRepositories : IUser
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private static readonly ILog log = LogManager.GetLogger(typeof(UserRepositories));
        // Need To be included in constructor all the interfaces and other dbcontext manager

        public UserRepositories(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        #region Add User
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
        #endregion

        #region Add AdminUser
        public async Task<Response> RegisterAdmin(Register registerAdmin)
        {
            try
            {
                AppUser user = new AppUser()
                {
                    UserName = registerAdmin.Name,
                    Email = registerAdmin.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                IdentityResult Result = await userManager.CreateAsync(user, registerAdmin.Password);
                if (!Result.Succeeded)
                {
                    // If user creation fails
                    return new Response
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again."
                    };
                }
                // Find Roles: Admin and User exists or not if not creates
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }
                // If Roles exists Admin and User assign those roles to user
                if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                if (await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                }
                return new Response { Status = "Success", Message = "User Created" };
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return new Response { Status = "Something Went Wrong", Message = "User Creation Failed" };
        }
        #endregion
        public async Task<CommonIndex> LoginUser(Login login)
        {
            CommonIndex commonIndex = new CommonIndex();
            try
            {
                AppUser user = await userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    // sign out current user
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, login.Password, true, false);
                    if (result.Succeeded)
                    {

                        var roles = await userManager.GetRolesAsync(user);
                        // roles variable contains the roles associated with the user
                        // You can iterate over the roles or perform any other necessary operations  
                        if (roles.Contains("Admin"))
                        {
                            AdminIndex adminIndex = new AdminIndex()
                            {
                                Email = user.Email,
                                Name = user.UserName,
                                Id = user.Id
                            };
                            commonIndex = new CommonIndex()
                            {
                                Roles = roles,
                                Admin = adminIndex,
                                User = new UserIndex()
                            };
                        }
                        else
                        {
                            UserIndex userIndex = new UserIndex()
                            {
                                Email = user.Email,
                                Name = user.UserName,
                                Id = user.Id
                            };
                            commonIndex = new CommonIndex()
                            {
                                Roles = roles,
                                Admin = new AdminIndex(),
                                User = userIndex
                            };
                        }
                        commonIndex.UserId = user.Id;
                        return commonIndex;
                    }
                    else
                    {
                        Response response = new Response() { Message = "Invalid Password", Status = "401" };
                        commonIndex = new CommonIndex()
                        {
                            response = response
                        };
                    }
                }
                else
                {
                    Response response = new Response() { Message = "Invalid Email", Status = "401" };
                    commonIndex = new CommonIndex()
                    {
                        response = response
                    };
                }
                return commonIndex;
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException != null ? string.Format("Inner Exception: {0} --- Exception: {1}", ex.InnerException.Message, ex.Message) : ex.Message, ex);
            }
            return commonIndex;
        }
        //public async Task<bool> LogOut()
        //{


        //    var Result = signInManager.SignOutAsync();
        //    if (Result != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}

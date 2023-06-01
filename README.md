# ShoesApi
Check my Roles Project
Identity Default Methods
1) find current logged in user
AppUser appUser = await userManager.GetUserAsync(HttpContext.User); // AppUser is a class created using inheriting IdentityUser
2) find user by email
AppUser appUser = await userManager.FindByEmailAsync(login.Email);
3) Create User
                AppUser user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identResult = await userManager.CreateAsync(user);
4) Delete User
            AppUser user = await userManager.FindByIdAsync(id);
            IdentityResult result = await userManager.DeleteAsync(user);
5) validate user
            AppUser user = await userManager.FindByIdAsync(id);
            validOrNot = await userValidator.ValidateAsync(userManager, user);
    validate password
                    private IPasswordValidator<AppUser> passwordValidator;
            validPass = await passwordValidator.ValidateAsync(userManager, user, password);
  

Cookie not working in web api so i created it in mvc controller and after creating cookies
values also not accessible in web api 
-------------------------------------------------------------------------------------------------------------
  This doesn't works in web api but works in mvc controller

            var user = HttpContext.User;

            // Access the desired claims by their claim type
            var passwordClaim = user.FindFirst(ClaimTypes.PrimarySid)?.Value;
  
-------------------------------------------------------------------------------------------------------------

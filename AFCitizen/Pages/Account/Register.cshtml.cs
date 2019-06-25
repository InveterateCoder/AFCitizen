using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public RoleManager<IdentityRole> roleManager;
        public UserManager<IdentityUser> userManager;
        public SignInManager<IdentityUser> signinManager;
        public RegisterModel(RoleManager<IdentityRole> roleMgr, UserManager<IdentityUser> userMgr, SignInManager<IdentityUser>signinMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;
            signinManager = signinMgr;
        }
        [Required, BindProperty]
        public string Name { get; set; }
        [Required, BindProperty]
        public string Email { get; set; }
        [Required, BindProperty]
        public string Password { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (!await roleManager.RoleExistsAsync("Пользователь"))
                    ModelState.AddModelError("", "Роль \"Пользователь\" не найдена, свяжитесь с администратором.");
                else
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = Name,
                        Email = Email
                    };
                    IdentityResult result = await userManager.CreateAsync(user, Password);
                    if (!result.Succeeded)
                        AddErrorsFromResult(result);
                    else
                    {
                        result = await userManager.AddToRoleAsync(user, "Пользователь");
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                            await userManager.DeleteAsync(user);
                        }
                        else
                        {
                            await signinManager.SignInAsync(user, false);
                            return Redirect("~/");
                        }
                    }
                }
            }
            return Page();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace AFCitizen.Pages.Account
{
    public class LoginModel : PageModel
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        public LoginModel(UserManager<IdentityUser> usrMgr, SignInManager<IdentityUser> signinMgr)
        {
            userManager = usrMgr;
            signInManager = signinMgr;
        }
        [BindProperty]
        public Models.Account.LoginMod Form { get; set; } = new Models.Account.LoginMod();
        public void OnGet(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByEmailAsync(Form.Email);
                if (user != null)
                {
                    if (await userManager.IsInRoleAsync(user, "Пользователь"))
                    {
                        await signInManager.SignOutAsync();
                        Microsoft.AspNetCore.Identity.SignInResult result =
                            await signInManager.PasswordSignInAsync(user, Form.Password, false, false);
                        if (result.Succeeded)
                            return Redirect(returnUrl ?? "/");
                        else
                            ModelState.AddModelError("", "Что то пошло не так, свяжитесь с администратором");
                            
                    }
                    else
                        ModelState.AddModelError("", "Данный пользователь не относится к роли \"Пользователь\"");
                }
                else
                    ModelState.AddModelError(nameof(Models.Account.LoginMod.Email), "Неправильны пользователь или пароль");
            }
            ViewData["returnUrl"] = returnUrl;
            return Page();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
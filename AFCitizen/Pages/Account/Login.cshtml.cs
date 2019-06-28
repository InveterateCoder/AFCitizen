using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using AFCitizen.Models;

namespace AFCitizen.Pages.Account
{
    public class LoginModel : PageModel
    {
        [Required, BindProperty]
        public string Email { get; set; }
        [Required, BindProperty]
        public string Password { get; set; }
        public bool isAuthority { get; set; }
        public void OnGet(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || returnUrl == "/Index")
                isAuthority = false;
            else
                isAuthority = true;
            ViewData["returnUrl"] = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl, [FromServices]UserManager<CitizenUser> userMgr, [FromServices]SignInManager<CitizenUser> signinMgr)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || returnUrl == "/Index")
                {
                    isAuthority = false;
                    CitizenUser user = await userMgr.FindByEmailAsync(Email);
                    if (user != null)
                    {
                        if (await userMgr.IsInRoleAsync(user, "Пользователь"))
                        {
                            await signinMgr.SignOutAsync();
                            Microsoft.AspNetCore.Identity.SignInResult result =
                                await signinMgr.PasswordSignInAsync(user, Password, false, false);
                            if (result.Succeeded)
                                return Redirect(returnUrl ?? "/");
                            else
                                ModelState.AddModelError("", "Что то пошло не так, свяжитесь с администратором");

                        }
                        else
                            ModelState.AddModelError("", "Данный пользователь не относится к роли \"Пользователь\"");
                    }
                    else
                        ModelState.AddModelError("", "Неправильны пользователь или пароль");
                }
                else
                {
                    isAuthority = true;
                    CitizenUser user = await userMgr.FindByEmailAsync(Email);
                    if (user != null)
                    {
                        if (await userMgr.IsInRoleAsync(user, "ПервУровень") || await userMgr.IsInRoleAsync(user, "СредУровень") || await userMgr.IsInRoleAsync(user, "ТопУровень"))
                        {
                            await signinMgr.SignOutAsync();
                            Microsoft.AspNetCore.Identity.SignInResult result =
                                await signinMgr.PasswordSignInAsync(user, Password, false, false);
                            if (result.Succeeded)
                                return Redirect(returnUrl ?? "/");
                            else
                                ModelState.AddModelError("", "Что то пошло не так, свяжитесь с администратором");

                            if (await userMgr.IsInRoleAsync(user, "Диспетчер"))
                                return RedirectToPage("/Dispatcher");
                            else
                                return RedirectToPage("/Agent");
                        }
                        else
                            ModelState.AddModelError("", "Уровень доступа не определён");
                    }
                    else
                        ModelState.AddModelError("", "Исполнитель не найден");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || returnUrl == "/Index")
                    isAuthority = false;
                else
                    isAuthority = true;
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
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
        public IActionResult OnGet(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || string.Equals(returnUrl, "/Index", System.StringComparison.OrdinalIgnoreCase)
                || string.Equals(returnUrl, "/Compose", System.StringComparison.OrdinalIgnoreCase))
                isAuthority = false;
            else if (string.Equals(returnUrl, "/Dispatcher", System.StringComparison.OrdinalIgnoreCase)
                || string.Equals(returnUrl, "/Agent", System.StringComparison.OrdinalIgnoreCase))
                isAuthority = true;
            else
                return RedirectToPage("AccessDenied");
            ViewData["returnUrl"] = returnUrl;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl, [FromServices]UserManager<CitizenUser> userMgr, [FromServices]SignInManager<CitizenUser> signinMgr)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || string.Equals(returnUrl, "/Index", System.StringComparison.OrdinalIgnoreCase)
                || string.Equals(returnUrl, "/Compose", System.StringComparison.OrdinalIgnoreCase))
                {
                    isAuthority = false;
                    CitizenUser user = await userMgr.FindByEmailAsync(Email);
                    if (user != null)
                    {
                        if (await userMgr.IsInRoleAsync(user, "������������"))
                        {
                            await signinMgr.SignOutAsync();
                            Microsoft.AspNetCore.Identity.SignInResult result =
                                await signinMgr.PasswordSignInAsync(user, Password, false, false);
                            if (result.Succeeded)
                                return Redirect(returnUrl ?? "/");
                            else
                                ModelState.AddModelError("", "��� �� ����� �� ���, ��������� � ���������������");

                        }
                        else
                            ModelState.AddModelError("", "������ ������������ �� ��������� � ���� \"������������\"");
                    }
                    else
                        ModelState.AddModelError("", "����������� ������������ ��� ������");
                }
                else
                {
                    isAuthority = true;
                    CitizenUser user = await userMgr.FindByEmailAsync(Email);
                    if (user != null)
                    {
                        if (await userMgr.IsInRoleAsync(user, "�����������") || await userMgr.IsInRoleAsync(user, "�����������") || await userMgr.IsInRoleAsync(user, "����������"))
                        {
                            await signinMgr.SignOutAsync();
                            Microsoft.AspNetCore.Identity.SignInResult result =
                                await signinMgr.PasswordSignInAsync(user, Password, false, false);
                            if (result.Succeeded)
                                return Redirect(returnUrl ?? "/");
                            else
                                ModelState.AddModelError("", "��� �� ����� �� ���, ��������� � ���������������");

                            if (await userMgr.IsInRoleAsync(user, "���������"))
                                return RedirectToPage("/Dispatcher");
                            else
                                return RedirectToPage("/Agent");
                        }
                        else
                            ModelState.AddModelError("", "������� ������� �� ��������");
                    }
                    else
                        ModelState.AddModelError("", "����������� �� ������");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl) || returnUrl == "/" || string.Equals(returnUrl, "/Index", System.StringComparison.OrdinalIgnoreCase)
                || string.Equals(returnUrl, "/Compose", System.StringComparison.OrdinalIgnoreCase))
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
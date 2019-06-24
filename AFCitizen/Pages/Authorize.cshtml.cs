using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages
{
    public class AuthorizeModel : PageModel
    {
        [Required, BindProperty]
        public string Email { get; set; }
        [Required, BindProperty]
        public string Password { get; set; }
        public async Task<IActionResult> OnPostAsync([FromServices]UserManager<IdentityUser> userMgr, [FromServices]SignInManager<IdentityUser> signinMgr)
        {
            await signinMgr.SignOutAsync();
            if (ModelState.IsValid)
            {
                IdentityUser user = await userMgr.FindByEmailAsync(Email);
                if (user != null)
                {
                    if(await userMgr.IsInRoleAsync(user, "�����"))
                    {
                        var result = await signinMgr.PasswordSignInAsync(user, Password, false, false);
                        if (result.Succeeded)
                        {
                            if (await userMgr.IsInRoleAsync(user, "���������"))
                            {
                                return RedirectToPage("/City/Index");
                            }
                            else
                            {
                                return RedirectToPage("/City/Agent");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "�� ���������� ��������������");
                        }
                    }
                    else
                    {
                        if (await userMgr.IsInRoleAsync(user, "�������"))
                        {
                            var result = await signinMgr.PasswordSignInAsync(user, Password, false, false);
                            if (result.Succeeded)
                            {
                                if (await userMgr.IsInRoleAsync(user, "���������"))
                                {
                                    return RedirectToPage("/Subject/Index");
                                }
                                else
                                {
                                    return RedirectToPage("/Subject/Agent");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "�� ���������� ��������������");
                            }
                        }
                        else
                        {
                            if (await userMgr.IsInRoleAsync(user, "���������"))
                            {
                                var result = await signinMgr.PasswordSignInAsync(user, Password, false, false);
                                if (result.Succeeded)
                                {
                                    if (await userMgr.IsInRoleAsync(user, "���������"))
                                    {
                                        return RedirectToPage("/Federation/Index");
                                    }
                                    else
                                    {
                                        return RedirectToPage("/Federation/Agent");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "�� ���������� ��������������");
                                }
                            }
                            else
                                ModelState.AddModelError("", "����������� �� ������");
                        }
                    }
                }
                else
                    ModelState.AddModelError("", "����������� �� ������");
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
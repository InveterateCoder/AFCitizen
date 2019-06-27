using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFCitizen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages
{
    public class IndexModel : PageModel
    {
        public async void OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Пользователь"))
                {
                    TempData["IsAuthenticated"] = true;
                    TempData["AuthenticatedName"] = User.Identity.Name;
                }
                else
                {
                    TempData["IsAuthenticated"] = false;
                    var signMgr = (SignInManager<CitizenUser>)HttpContext.RequestServices.GetService(typeof(SignInManager<CitizenUser>));
                    await signMgr.SignOutAsync();
                }
            }
            else
                TempData["IsAuthenticated"] = false;
        }
        public async void OnGetSignOutAsync([FromServices]SignInManager<CitizenUser> signinMgr)
        {
            await signinMgr.SignOutAsync();
            TempData["IsAuthenticated"] = false;
        }
    }
}
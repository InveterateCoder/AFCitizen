using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            TempData["IsAuthenticated"] = isAuthenticated;
            if (isAuthenticated)
                TempData["AuthenticatedName"] = User.Identity.Name;
        }
        public async void OnGetSignOutAsync([FromServices]SignInManager<IdentityUser> signinMgr)
        {
            await signinMgr.SignOutAsync();
            TempData["IsAuthenticated"] = false;
        }
    }
}
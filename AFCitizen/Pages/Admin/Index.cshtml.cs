using System.Collections.Generic;
using System.Threading.Tasks;
using AFCitizen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private UserManager<CitizenUser> userMagnager;
        public IndexModel(UserManager<CitizenUser> usrMgr) => userMagnager = usrMgr;
        public IEnumerable<CitizenUser> Users;
        public void OnGet()
        {
            Users = userMagnager.Users;
        }
        public async Task<IActionResult> OnPostDelete(string id)
        {
            CitizenUser user = await userMagnager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userMagnager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToPage("Index");
                else
                    AddErrorsFromResult(result);

            }
            else
                ModelState.AddModelError("", "User Not Found");
            Users = userMagnager.Users;
            return Page();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
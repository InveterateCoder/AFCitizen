using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Admin.Roles
{
    public class IndexModel : PageModel
    {
        private RoleManager<IdentityRole> roleManager;
        public IndexModel(RoleManager<IdentityRole> roleMgr) => roleManager = roleMgr;
        [BindProperty]
        public IEnumerable<IdentityRole> Roles { get; set; }
        public void OnGet() => Roles = roleManager.Roles;
        public async Task<IActionResult> OnPostAsync(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToPage("Index");
                else
                    AddErrorsFromResult(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            Roles = roleManager.Roles;
            return Page();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Admin.Roles
{
    public class CreateModel : PageModel
    {
        private RoleManager<IdentityRole> roleManager;
        public CreateModel(RoleManager<IdentityRole> roleMgr) => roleManager = roleMgr;
        [Required, BindProperty]
        public string Name { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(Name));
                if (result.Succeeded)
                    return RedirectToPage("Index");
                else AddErrorsFromResult(result);
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
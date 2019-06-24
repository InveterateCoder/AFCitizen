using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private UserManager<IdentityUser> userManager;
        public CreateModel(UserManager<IdentityUser> usrMgr) => userManager = usrMgr;
        [BindProperty]
        public Models.Admin.CreateMod Form { get; set; } = new Models.Admin.CreateMod();
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = Form.User,
                    Email = Form.Email
                };
                IdentityResult result = await userManager.CreateAsync(user, Form.Password);
                if (result.Succeeded)
                    return RedirectToPage("Index");
                else
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
            }
            return Page();
        }
    }
}
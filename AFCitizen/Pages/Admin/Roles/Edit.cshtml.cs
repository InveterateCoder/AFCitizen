using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Admin.Roles
{
    public class EditModel : PageModel
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<IdentityUser> userManager;
        public EditModel(RoleManager<IdentityRole> roleMgr, UserManager<IdentityUser> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;
        }
        public Models.Admin.RoleEditMod RoleEditModel { get; set; }
        public async Task OnGetAsync(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<IdentityUser> members = new List<IdentityUser>();
            List<IdentityUser> nonMembers = new List<IdentityUser>();
            foreach (IdentityUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            RoleEditModel = new Models.Admin.RoleEditMod
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
        }
        public async Task<IActionResult> OnPostAsync(Models.Admin.RoleModificationMod model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    IdentityUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            AddErrorsFromResult(result);
                    }
                }
            }
            foreach (string userId in model.IdsToDelete ?? new string[] { })
            {
                IdentityUser user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                        AddErrorsFromResult(result);
                }
            }
            if (ModelState.IsValid)
                return RedirectToPage("Index");
            else
                return Redirect(Url.Page("Edit", new { id = model.RoleId }));
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
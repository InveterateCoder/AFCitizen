using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Admin
{
    public class EditModel : PageModel
    {
        private UserManager<IdentityUser> userManager;
        private IUserValidator<IdentityUser> userValidator;
        private IPasswordValidator<IdentityUser> passwordValidator;
        private IPasswordHasher<IdentityUser> passwordHasher;
        public EditModel(UserManager<IdentityUser> usrMgr, IUserValidator<IdentityUser> userValid,
            IPasswordValidator<IdentityUser> passValid, IPasswordHasher<IdentityUser> passwordHash)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }
        [BindProperty]
        public Models.Admin.EditMod Form { get; set; } = new Models.Admin.EditMod();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                Form.Id = user.Id;
                Form.User = user.UserName;
                Form.Email = user.Email;
                return Page();
            }
            else
                return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostAsync(string password)
        {
            IdentityUser _user = await userManager.FindByIdAsync(Form.Id);
            if (_user != null)
            {
                if (!string.IsNullOrEmpty(Form.User))
                    _user.UserName = Form.User;
                IdentityResult validEmail = null;
                if (!string.IsNullOrEmpty(Form.Email))
                {
                    _user.Email = Form.Email;
                    validEmail = await userValidator.ValidateAsync(userManager, _user);
                    if (!validEmail.Succeeded)
                        AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, _user, password);
                    if (validPass.Succeeded)
                        _user.PasswordHash = passwordHasher.HashPassword(_user, password);
                    else
                        AddErrorsFromResult(validPass);
                }
                if (!((string.IsNullOrEmpty(Form.User) && (validPass == null && validEmail == null)) || (validEmail != null && !validEmail.Succeeded || validPass != null && !validPass.Succeeded)))
                {
                    IdentityResult result = await userManager.UpdateAsync(_user);
                    if (result.Succeeded)
                        return RedirectToPage("Index");
                    else
                        AddErrorsFromResult(result);
                }
            }
            else
                ModelState.AddModelError("", "Пользователь не найден");
            return Page();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
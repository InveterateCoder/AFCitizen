using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Pages
{
    [Authorize(Roles = "Пользователь")]
    public class ComposeModel : PageModel
    {
        [BindProperty]
        public Models.Document Doc { get; set; } = new Models.Document();
        [Required, BindProperty]
        public string City { get; set; }
        [Required, BindProperty]
        public string AuthorityType { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync([FromServices]Models.UserLevelDbContext userDbContext)
        {
            if (ModelState.IsValid)
            {
                Models.Authority authority = Models.Authority.Cities[City][AuthorityType][0];
                Models.ILevelDbContext levelDbContext = null;
                switch (authority.Level)
                {
                    case 1:
                        levelDbContext = HttpContext.RequestServices.GetService(typeof(Models.FirstLevelDbContext)) as Models.ILevelDbContext;
                        break;
                    case 2:
                        levelDbContext = HttpContext.RequestServices.GetService(typeof(Models.MidLevelDbContext)) as Models.ILevelDbContext;
                        break;
                    case 3:
                        levelDbContext = HttpContext.RequestServices.GetService(typeof(Models.TopLevelDbContext)) as Models.ILevelDbContext;
                        break;
                }
                if (levelDbContext == null)
                    ModelState.AddModelError("", "Не получилось установить связь с базой данных");
                else
                {
                    Models.Block block = new Models.Block();
                    block.From = User.Identity.Name;
                    block.AuthorityType = AuthorityType;
                    block.To = authority.Name;
                    block.Type = Models.BlockType.Open;
                    block.Document = Newtonsoft.Json.JsonConvert.SerializeObject(Doc);
                    block.PreviousHash = "origin";
                    block.DocId = Guid.NewGuid().ToString();
                    block.Lock();
                    await userDbContext.Blocks.AddAsync(block);
                    await userDbContext.SaveChangesAsync();
                    await levelDbContext.Blocks.AddAsync(block);
                    await ((DbContext)levelDbContext).SaveChangesAsync();
                    return RedirectToPage("Index");
                }
            }
            return Page();
        }
    }
}
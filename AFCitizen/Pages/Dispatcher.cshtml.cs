using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages
{
    [Authorize(Policy = "ƒиспетчер")]
    public class DispatcherModel : PageModel
    {
        public string Authority { get; set; }
        public IEnumerable<Models.Block> OpenBlocks;
        public async Task<IActionResult> OnGetAsync()
        {
            Authority = User.Identity.Name;
            Models.ILevelDbContext levelDbContext = null;
            if (User.IsInRole("ѕерв”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(Models.FirstLevelDbContext)) as Models.ILevelDbContext;
            else if (User.IsInRole("—ред”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(Models.MidLevelDbContext)) as Models.ILevelDbContext;
            else if (User.IsInRole("“оп”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(Models.TopLevelDbContext)) as Models.ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            OpenBlocks = await Task.Run(() => levelDbContext.Blocks.Where(block => block.To == Authority && block.Type == Models.BlockType.Open
                  && !levelDbContext.Blocks.Any(rep => (rep.DocId == block.DocId && block.Type != Models.BlockType.Open))).ToArray());
            return Page();
        }
    }
}
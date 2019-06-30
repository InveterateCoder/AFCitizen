using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFCitizen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AFCitizen.Pages
{
    [Authorize(Policy = "јгент")]
    public class AgentModel : PageModel
    {
        public string Header { get; set; }
        public IEnumerable<Block> OpenBlocks { get; set; }
        public IActionResult OnGet()
        {
            Header = User.Identity.Name;
            ILevelDbContext levelDbContext = null;
            if (User.IsInRole("ѕерв”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("—ред”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("“оп”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            OpenBlocks = levelDbContext.Blocks.Where(block => block.To == User.Identity.Name && block.Type == BlockType.Accept && !levelDbContext.Blocks.Any(ab => ab.DocId == block.DocId && ab.Type == BlockType.Close));
            return Page();
        }
        public async Task<IActionResult> OnPostAsync([FromServices] UserLevelDbContext userDbContext, [FromServices] UserManager<CitizenUser> userMgr, string blockId, string reply, string comment)
        {
            var user = userMgr.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            Block block = await userDbContext.Blocks.FindAsync(blockId);
            Reply _reply = new Reply
            {
                AgentFullName = User.Identity.Name,
                Body = reply,
                Comment = comment,
                From = user.UserName,
                Position = user.Position,
                To = block.From
            };
            Block newBlock = new Block();
            newBlock.AuthorityType = block.AuthorityType;
            newBlock.City = block.City;
            newBlock.DocId = block.DocId;
            newBlock.Document = block.Document;
            newBlock.From = User.Identity.Name;
            newBlock.PreviousHash = block.Hash;
            if (string.IsNullOrEmpty(block.Replies))
            {
                Reply[] replies = new Reply[] { _reply };
                newBlock.Replies = Newtonsoft.Json.JsonConvert.SerializeObject(replies);
            }
            else
            {
                List<Reply> list = new List<Reply>((Reply[])Newtonsoft.Json.JsonConvert.DeserializeObject(block.Replies, typeof(Reply[])));
                list.Add(_reply);
                newBlock.Replies = Newtonsoft.Json.JsonConvert.SerializeObject(list.ToArray());
            }
            newBlock.To = block.From;
            newBlock.Type = BlockType.Close;
            newBlock.Lock();
            ILevelDbContext levelDbContext = null;
            if (User.IsInRole("ѕерв”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("—ред”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("“оп”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            await levelDbContext.Blocks.AddAsync(newBlock);
            await ((DbContext)levelDbContext).SaveChangesAsync();
            await userDbContext.Blocks.AddAsync(newBlock);
            await userDbContext.SaveChangesAsync();
            return RedirectToPage("Agent");
        }
    }
}
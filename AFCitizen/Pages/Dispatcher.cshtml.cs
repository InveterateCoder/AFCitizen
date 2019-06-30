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
    [Authorize(Policy = "ƒиспетчер")]
    public class DispatcherModel : PageModel
    {
        public string Authority { get; set; }
        public IEnumerable<Block> OpenBlocks;
        public IEnumerable<CitizenUser> Employees;
        public async Task<IActionResult> OnGetAsync([FromServices] UserManager<CitizenUser> userMgr)
        {
            Authority = User.Identity.Name;
            ILevelDbContext levelDbContext = null;
            if (User.IsInRole("ѕерв”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("—ред”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("“оп”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            OpenBlocks = await Task.Run(() => levelDbContext.Blocks.Where(block => block.To == Authority && block.Type == BlockType.Open
                  && levelDbContext.Blocks.Where(bl => bl.DocId == block.DocId).Count() == 1).ToArray());
            Employees = await Task.Run(() => userMgr.Users.Where(user => user.Dispatcher == User.Identity.Name).ToArray());
            return Page();
        }
        public async Task<IActionResult> OnPostCloseAsync(string blockId, string reply, string comment)
        {
            ILevelDbContext levelDbContext = null;
            if (User.IsInRole("ѕерв”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("—ред”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("“оп”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            Block block = await levelDbContext.Blocks.FindAsync(blockId);
            Block newBlock = new Block();
            newBlock.From = User.Identity.Name;
            newBlock.To = block.From;
            newBlock.DocId = block.DocId;
            newBlock.Document = block.Document;
            newBlock.AuthorityType = block.AuthorityType;
            Document doc = (Document)Newtonsoft.Json.JsonConvert.DeserializeObject(block.Document, typeof(Document));
            Reply[] replies;
            if (string.IsNullOrEmpty(block.Replies))
            {
                replies = new Reply[] { new Reply { From = User.Identity.Name, Body = reply, Comment = comment, To = doc.From } };
            }
            else
            {
                replies = (Reply[])Newtonsoft.Json.JsonConvert.DeserializeObject(block.Replies, typeof(Reply[]));
                List<Reply> listReplies = new List<Reply>(replies);
                listReplies.Add(new Reply { From = User.Identity.Name, Body = reply, Comment = comment, To = doc.From });
                replies = listReplies.ToArray();
            }
            newBlock.Replies = Newtonsoft.Json.JsonConvert.SerializeObject(replies);
            newBlock.Type = BlockType.Close;
            newBlock.PreviousHash = block.Hash;
            newBlock.Lock();
            UserLevelDbContext userDbContext = HttpContext.RequestServices.GetService(typeof(UserLevelDbContext)) as UserLevelDbContext;
            levelDbContext.Blocks.Add(newBlock);
            await ((DbContext)levelDbContext).SaveChangesAsync();
            userDbContext.Blocks.Add(newBlock);
            await userDbContext.SaveChangesAsync();
            return RedirectToPage("Dispatcher");
        }

        public async Task<IActionResult> OnPostAssignAsync([FromServices]UserManager<CitizenUser> userMgr, string userName, string blockId)
        {
            ILevelDbContext levelDbContext = null;
            if (User.IsInRole("ѕерв”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("—ред”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("“оп”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            Block block = await levelDbContext.Blocks.FindAsync(blockId);
            Block newBlock = new Block();
            newBlock.From = block.From;
            newBlock.To = userName;
            newBlock.AuthorityType = block.AuthorityType;
            newBlock.DocId = block.DocId;
            newBlock.Document = block.Document;
            newBlock.Replies = block.Replies;
            newBlock.Type = BlockType.Accept;
            var accept = new Accept { AgentFullName = userName, Position = userMgr.Users.Where(u => u.UserName == userName).Select(n => n.Position).FirstOrDefault() };
            newBlock.TypeMessage = Newtonsoft.Json.JsonConvert.SerializeObject(accept);
            newBlock.PreviousHash = block.Hash;
            newBlock.Lock();
            levelDbContext.Blocks.Add(newBlock);
            await ((DbContext)levelDbContext).SaveChangesAsync();
            UserLevelDbContext userDbContext = HttpContext.RequestServices.GetService(typeof(UserLevelDbContext)) as UserLevelDbContext;
            userDbContext.Blocks.Add(newBlock);
            await userDbContext.SaveChangesAsync();
            return RedirectToPage("Dispatcher");
        }

        public async Task<IActionResult> OnPostRedirectAsync(string city, string authorityType, string blockId, string comment)
        {
            ILevelDbContext levelDbContext = null;
            if (User.IsInRole("ѕерв”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(FirstLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("—ред”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(MidLevelDbContext)) as ILevelDbContext;
            else if (User.IsInRole("“оп”ровень"))
                levelDbContext = HttpContext.RequestServices.GetService(typeof(TopLevelDbContext)) as ILevelDbContext;
            if (levelDbContext == null)
                return RedirectToPage("Error");
            Block block = await levelDbContext.Blocks.FindAsync(blockId);
            Authority authority = Models.Authority.Cities[city][authorityType][0];
            Block newBlock = new Block();
            newBlock.From = User.Identity.Name;
            newBlock.To = block.From;
            newBlock.DocId = block.DocId;
            newBlock.Document = block.Document;
            newBlock.AuthorityType = block.AuthorityType;
            newBlock.Type = BlockType.Redirect;
            newBlock.TypeMessage = Newtonsoft.Json.JsonConvert.SerializeObject(new Redirect { AuthorityLevel = authority.Level, AuthorityType = authorityType, To = authority.Name, Comment = comment });
            newBlock.PreviousHash = block.Hash;
            newBlock.Lock();
            levelDbContext.Blocks.Add(newBlock);
            await ((DbContext)levelDbContext).SaveChangesAsync();
            UserLevelDbContext userLevelDb = HttpContext.RequestServices.GetService(typeof(UserLevelDbContext)) as UserLevelDbContext;
            userLevelDb.Blocks.Add(newBlock);
            await userLevelDb.SaveChangesAsync();
            return RedirectToPage("Dispatcher");
        }
    }
}
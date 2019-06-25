using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.Subject
{
    [Authorize(Policy = "—Û·¿„")]
    public class AgentModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
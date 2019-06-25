using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AFCitizen.Pages.City
{
    [Authorize(Policy = "√Ó¿„")]
    public class AgentModel : PageModel
    {
        public string Header { get; set; }
        public void OnGet()
        {
            Header = User.Identity.Name;
        }
    }
}
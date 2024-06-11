using AutomobiliuNuoma.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace AutoNuomaFrontEnd.Pages
{
    public class IndexModel : PageModel
    {
       

        public IndexModel()
        {
            
        }

        public void OnGet()
        {
            Log.Information("Home page visited at {Time}", DateTime.UtcNow);
        }
    }
}

using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace AutoNuomaFrontEnd.Pages
{
    public class NuomaModel : PageModel
    {
        [BindProperty]
        public List<Nuoma> Nuomos { get; set; } = new List<Nuoma>();
        INuomaWebService _nuomaService;

        public NuomaModel(INuomaWebService nuomaService)
        {
            _nuomaService = nuomaService;
        }

        public void OnGet()
        {
            Nuomos = _nuomaService.GetNuoma();
            Log.Information("Nuoma page visited at {Time}", DateTime.UtcNow);
        }
    }
}

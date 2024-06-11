using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace AutoNuomaFrontEnd.Pages
{
    public class KlientaiModel : PageModel
    {
        [BindProperty]
        public List<Klientas> Klientai { get; set; } = new List<Klientas>();
        Klientas nKlientas = new Klientas();
        INuomaWebService _nuomaService;

        public KlientaiModel(INuomaWebService nuomaService)
        {
            _nuomaService = nuomaService;
        }

        public void OnGet()
        {
            Klientai = _nuomaService.GetKlientas();
            Log.Information("Klientai page visited at {Time}", DateTime.UtcNow);
        }


    }
}

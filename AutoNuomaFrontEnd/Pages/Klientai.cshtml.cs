using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoNuomaFrontEnd.Pages
{
    public class KlientaiModel : PageModel
    {
        [BindProperty]
        public List<Klientas> Klientai { get; set; } = new List<Klientas>();
        Klientas nKlientas = new Klientas();
        private readonly ILogger<IndexModel> _logger;
        INuomaWebService _nuomaService;

        public KlientaiModel(ILogger<IndexModel> logger, INuomaWebService nuomaService)
        {
            _nuomaService = nuomaService;
            _logger = logger;
        }

        public void OnGet()
        {
            Klientai = _nuomaService.GetKlientas();
        }


    }
}

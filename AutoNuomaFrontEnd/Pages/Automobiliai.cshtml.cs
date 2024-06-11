using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoNuomaFrontEnd.Pages
{
    public class AutomobiliaiModel : PageModel
    {
        [BindProperty]
        public List<Automobilis> Automobiliai { get; set; } = new List<Automobilis>();
        private readonly ILogger<IndexModel> _logger;
        INuomaWebService _nuomaService;

        public AutomobiliaiModel(ILogger<IndexModel> logger, INuomaWebService nuomaService)
        {
            _nuomaService = nuomaService;
            _logger = logger;
        }

        public void OnGet()
        {
            Automobiliai  = _nuomaService.GetVisiAuto();
        }
    }
}

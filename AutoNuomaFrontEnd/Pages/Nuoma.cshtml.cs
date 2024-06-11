using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoNuomaFrontEnd.Pages
{
    public class NuomaModel : PageModel
    {
        [BindProperty]
        public List<Nuoma> Nuomos { get; set; } = new List<Nuoma>();
        private readonly ILogger<NuomaModel> _logger;
        INuomaWebService _nuomaService;

        public NuomaModel(ILogger<NuomaModel> logger, INuomaWebService nuomaService)
        {
            _nuomaService = nuomaService;
            _logger = logger;
        }

        public void OnGet()
        {
            Nuomos = _nuomaService.GetNuoma();
        }
    }
}

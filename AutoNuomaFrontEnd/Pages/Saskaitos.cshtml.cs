using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoNuomaFrontEnd.Pages
{
    public class SaskaitosModel : PageModel
    {

        public List<Saskaita> Saskaitos { get; set; } = new List<Saskaita>();
        private readonly ILogger<SaskaitosModel> _logger;
        INuomaWebService _nuomaService;

        public SaskaitosModel(ILogger<SaskaitosModel> logger, INuomaWebService nuomaService)
        {
            _nuomaService = nuomaService;
            _logger = logger;
        }

        public void OnGet()
        {
            Saskaitos = _nuomaService.GetSaskaitos();
        }
    }
}

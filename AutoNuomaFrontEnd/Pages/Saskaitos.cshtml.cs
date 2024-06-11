using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace AutoNuomaFrontEnd.Pages
{
    public class SaskaitosModel : PageModel
    {

        public List<Saskaita> Saskaitos { get; set; } = new List<Saskaita>();
        INuomaWebService _nuomaService;

        public SaskaitosModel(INuomaWebService nuomaService)
        {
            _nuomaService = nuomaService;
        }

        public void OnGet()
        {
            Saskaitos = _nuomaService.GetSaskaitos();
            Log.Information("Saskaitos page visited at {Time}", DateTime.UtcNow);
        }
    }
}

using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;


namespace AutoNuomaFrontEnd.Pages
{
    public class DviraciaiModel : PageModel
    {
        [BindProperty]
        public List<Dviratis> Dviraciai { get; set; } = new List<Dviratis>();
        private readonly ILogger<DviraciaiModel> _logger;
        INuomaWebService _nuomaService;

        public DviraciaiModel(ILogger<DviraciaiModel> logger, INuomaWebService nuomaService)
        {
            _nuomaService = nuomaService;
            _logger = logger;
        }

        public void OnGet()
        {
            Dviraciai = _nuomaService.GetDviraciai();
            Log.Information("Dviraciai page visited at {Time}", DateTime.UtcNow);
        }
    }
}

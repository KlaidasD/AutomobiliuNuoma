using System;
using System.Collections.Generic;
using AutomobiliuNuoma.Models;
using AutoNuomaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace AutoNuomaFrontEnd.Pages
{
    public class DviraciuNuomaModel : PageModel
    {
        [BindProperty]
        public List<DviraciuNuoma> DviraciuList { get; set; } = new List<DviraciuNuoma>();

        private readonly ILogger<DviraciuNuomaModel> _logger;
        private readonly INuomaWebService _nuomaService;

        
        public DviraciuNuomaModel(ILogger<DviraciuNuomaModel> logger, INuomaWebService nuomaService)
        {
            _logger = logger;
            _nuomaService = nuomaService;
        }

        public void OnGet()
        {
            DviraciuList = _nuomaService.GetRentedDviraciai();
            Log.Information("DviraciuNuoma page visited at {Time}", DateTime.UtcNow);
        }
    }
}
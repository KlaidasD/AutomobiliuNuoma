using AutomobiliuNuoma.Models;

namespace AutoNuomaFrontEnd.Services
{
    public interface INuomaWebService
    {
        List<Automobilis> GetVisiAuto();
        List<Klientas> GetKlientas();
        List<Nuoma> GetNuoma();
        List<Saskaita> GetSaskaitos();

    }
}

using AutomobiliuNuoma.Contracts;
using AutomobiliuNuoma.Models;
using AutomobiliuNuoma.Repositories;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomobiliuNuoma.Services
{
    

    public class NuomaUI
    {
        private readonly INuomaService _nuomaService;
        public NuomaUI(INuomaService nuomaService, IDatabaseRepository databaseRepository)
        {
            _nuomaService = nuomaService;
        }

        public void Menu()
        {
            
            while(true)
            {
                Console.WriteLine("1. Registruoti nauja klienta");
                Console.WriteLine("2. Registruoti nauja automobili");
                Console.WriteLine("3. Perziureti automobilius pagal ju tipa");
                Console.WriteLine("4. Perziureti visus isnuomotus automobilius");
                Console.WriteLine("5. Perziureti visus klientus");
                Console.WriteLine("6. Atnaujinti automobilio informacija");
                Console.WriteLine("7. Istrinti automobili");
                Console.WriteLine("8. Istrinti klienta");
                Console.WriteLine("9. Isnuomuoti automobili");
                Console.WriteLine("10. Atnaujinti kliento duomenis");
                Console.WriteLine("11. Baigti darba");

                int ivestis = int.Parse(Console.ReadLine());

                switch (ivestis)
                {
                    case 1:
                        Klientas naujas = SukurtiKlienta();
                        _nuomaService.RegisterClient(naujas);
                        break;
                    case 2:
                        Automobilis automobilis = SukurtiAutomobili();
                        _nuomaService.RegisterAutomobilis(automobilis);
                        break;
                    case 3:
                        Console.WriteLine("Pasirinkite automobiliu tipa:*iveskite skaiciu*");
                        Console.WriteLine("1. Naftos/Kuro automobilis");
                        Console.WriteLine("2. Elektromobilis");

                        if (!int.TryParse(Console.ReadLine(), out int pasirinkimas))
                        {
                            Console.WriteLine("Neteisinga ivestis. Iveskite skaiciu 1 arba 2.");
                            continue;
                        }

                        string tipas = pasirinkimas ==  1 ? "NaftosKuroAutomobilis" : "Elektromobilis";
                        List<Automobilis> automobiliai = _nuomaService.GetAutomobiliai(tipas);

                        if (automobiliai.Count > 0)
                        {
                            Console.WriteLine($"Spausdinami visi {tipas} automobiliai:");
                            foreach (Automobilis auto in automobiliai)
                            {
                                Console.WriteLine(auto);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Nera {tipas} automobiliu.");
                        }
                        break;
                    case 4:
                     Console.WriteLine("Spausdinami visi isnuomoti automobiliai:");
                        List<Automobilis> isnuomoti = _nuomaService.GetRentedAutomobiliai();
                        if (isnuomoti.Count > 0)
                        {
                            foreach (Automobilis auto in isnuomoti)
                            {
                                Console.WriteLine(auto);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nera isnuomotu automobiliu.");
                        }
                        break;
                    case 5:
                        Console.WriteLine("Spausdinami visi klientai:");
                        List<Klientas> klientai = _nuomaService.GetKlientai();
                        if (klientai.Count > 0)
                        {
                            foreach (Klientas klientas in klientai)
                            {
                                Console.WriteLine(klientas);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nera klientu.");
                        }
                        break;
                    case 6:
                        Console.WriteLine("Iveskite automobilio id kuri norite atnaujinti: ");
                        int atnaujinamas;
                        if (!int.TryParse(Console.ReadLine(), out atnaujinamas))
                        {
                            Console.WriteLine("Neteisinga ivestis");
                            return;
                        }
                        _nuomaService.UpdateAutomobilis(atnaujinamas);
                        break;
                    case 7:
                        Console.WriteLine("Iveskite automobilio id kuri norite istrinti:");
                        int istrinamas;
                        if (!int.TryParse(Console.ReadLine(), out istrinamas))
                        {
                            Console.WriteLine("Neteisinga ivestis");
                            return;
                        }
                        _nuomaService.DeleteAutomobilis(istrinamas);
                        break;
                    case 8:
                        Console.WriteLine("Iveskite kliento id kuri norite istrinti:");
                        int kIstrinamas;
                        if (!int.TryParse(Console.ReadLine(), out kIstrinamas))
                        {
                            Console.WriteLine("Neteisinga ivestis");
                            return;
                        }
                        _nuomaService.DeleteClient(kIstrinamas);
                        break;
                    case 9:
                        Console.WriteLine("Kad isnuomoti automobili mums reikia siu duomenu.\nIveskite automobilio ID:");
                        int autoId;
                        if (!int.TryParse(Console.ReadLine(), out autoId))
                        {
                            Console.WriteLine("Neteisinga ivestis");
                            return;
                        }
                        Console.WriteLine("Iveskite savo kliento ID:");
                        int klientoId;
                        if (!int.TryParse(Console.ReadLine(), out klientoId))
                        {
                            Console.WriteLine("Neteisinga ivestis");
                            return;
                        }
                        Console.WriteLine("Iveskite nuomos pradzios data formatu yyyy-MM-dd:");
                        DateTime nuo = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("Iveskite nuomos pabaigos data formatu yyyy-MM-dd:");
                        DateTime iki = DateTime.Parse(Console.ReadLine());
                        _nuomaService.RentAutomobilis(autoId, klientoId, nuo, iki);
                        break;
                    case 10:
                        Console.WriteLine("Iveskite kliento ID kuri norite atnaujinti: ");
                        int kAtnaujinamas;
                        if (!int.TryParse(Console.ReadLine(), out kAtnaujinamas))
                        {
                            Console.WriteLine("Neteisinga ivestis");
                            return;
                        }
                        _nuomaService.UpdateClient(kAtnaujinamas);
                        break;
                    case 11:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Neteisinga ivestis");
                        break;
                }

            }
        }
        Klientas SukurtiKlienta()
        {
            Console.WriteLine("Iveskite kliento duomenis.\nVardas:");
            string vardas = Console.ReadLine();
            Console.WriteLine("Pavarde:");
            string pavarde = Console.ReadLine();
            Console.WriteLine("Email:");
            string email = Console.ReadLine();
            Klientas klientas = new Klientas
            {
                Vardas = vardas,
                Pavarde = pavarde,
                Email = email,
                RegData = DateTime.Now
            };
            return klientas;
        }
        Automobilis SukurtiAutomobili()
        {
            Console.WriteLine("Iveskite automobilio duomenis.\nMarke:");
            string marke = Console.ReadLine();
            Console.WriteLine("Modelis:");
            string modelis = Console.ReadLine();
            Console.WriteLine("Metai:");
            int metai = int.Parse(Console.ReadLine());
            Console.WriteLine("Registracijos numeris:");
            string regNr = Console.ReadLine();
            Console.WriteLine("Bako ar baterijos talpa:");
            float bakoTalpa = float.Parse(Console.ReadLine());
            Console.WriteLine("Automobilio tipas: *vesti skaiciu*(1.Naftos, 2.Elektromobilis)");
            int tipas;
            if(!int.TryParse(Console.ReadLine(), out tipas))
            {
                Console.WriteLine("Neteisinga ivestis");
            }

            if (tipas == 1)
            {
                Automobilis naujas = new NaftosKuroAutomobilis
                {
                    Marke = marke,
                    Modelis = modelis,
                    Metai = metai,
                    RegistracijosNumeris = regNr,
                    BakoTalpa = bakoTalpa
                };
                return naujas;
            }
            else
            {
                Automobilis naujas = new Elektromobilis
                {
                    Marke = marke,
                    Modelis = modelis,
                    Metai = metai,
                    RegistracijosNumeris = regNr,
                    BaterijosTalpa = bakoTalpa
                };
                return naujas;
            }
        }
    }
}

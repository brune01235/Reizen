using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reizen.Models;
using Reizen.Models.Repositories;
using System.Diagnostics;

namespace Reizen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReizenContext context;
        private readonly IWerelddeelRepository werelddeelRepository;
        private readonly ILandRepository landRepository;
        private readonly IBestemmingRepository bestemmingRepository;
        private readonly IReisRepository reisRepository;

        public HomeController(ReizenContext context, IWerelddeelRepository werelddeelRepository, ILandRepository landRepository
            ,IBestemmingRepository bestemmingRepository,IReisRepository reisRepository)
        {
            this.context = context;
            this.werelddeelRepository = werelddeelRepository;
            this.landRepository = landRepository;
            this.bestemmingRepository = bestemmingRepository;
            this.reisRepository = reisRepository;
            
        }

       

        public async Task<IActionResult> Index()
        {
            
            return View(await werelddeelRepository.GetWerelddelen());
        }

        public async Task<IActionResult> Land(int id)
        {
            var gekozenWereldDeel = await werelddeelRepository.GetWerelddeel(id);
            var landen = await context.Landen.Where(land => land.Werelddeelid == id).OrderBy(land => land.Naam).ToListAsync();
            if (gekozenWereldDeel == null) 
            {
                return NotFound();
            }
            
            ViewBag.WereldDeel = gekozenWereldDeel.Naam;

            return View(landen);
        }

        public async Task<IActionResult> Bestemmingen(int id)
        {
            var bestemmingen = await context.Bestemmingen.Where(bestemming => bestemming.Landid == id).OrderBy(bestemming => bestemming.Plaats).ToListAsync();
            var gekozenLand = await landRepository.GetLand(id);
            if (gekozenLand == null) 
            { 
                return NotFound(); 
            }

            ViewBag.Land = gekozenLand.Naam;
            return View(bestemmingen);
        }

        public async Task<IActionResult> Reizen(string code)
        {
            var reizen = await context.Reizen.Where(reis => reis.Bestemmingscode == code).OrderBy(reis => reis.Vertrek).ToListAsync();
            var gekozenBestemming = await bestemmingRepository.GetBestemming(code);
            if (gekozenBestemming == null) 
            { 
                return NotFound(); 
            }

            ViewBag.Bestemming = gekozenBestemming.Plaats;

            return View(reizen);
        }

        public async Task<IActionResult> ZoekForm(int id, string code) 
        {
            var bestemming = await bestemmingRepository.GetBestemming(code);
            var gekozenReis = await reisRepository.GetReis(id);
            if (gekozenReis == null) 
            { 
                return NotFound(); 
            }

            ;

            ViewBag.Bestemming = bestemming.Plaats;
            ViewBag.Vertrek = gekozenReis.Vertrek;
            ViewBag.Dagen = gekozenReis.AantalDagen;
            ViewBag.Prijs = gekozenReis.PrijsPerPersoon;

            return View(new ZoekKlantViewModel()); 
        }
        
        public async Task<IActionResult> ZoekKlant(ZoekKlantViewModel form)
        {
            form.Klanten = await context.Klanten.Include(klant => klant.Woonplaats).
                OrderBy(klant =>  klant.Familienaam).ThenBy(klant => klant.Voornaam).
                Where(klant => klant.Familienaam.StartsWith(form.BeginFamilienaam)).
                ToListAsync();
            if (form.Klanten == null)
                ViewBag.ErrorMessage = $"zoekveld mag niet leeg zijn";
            return View("ZoekForm", form);
        }

        public async Task<IActionResult> Boeking() 
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

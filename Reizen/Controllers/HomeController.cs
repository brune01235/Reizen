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
        private readonly IKlantRepository klantRepository;

        public HomeController(ReizenContext context, IWerelddeelRepository werelddeelRepository, ILandRepository landRepository
            ,IBestemmingRepository bestemmingRepository,IReisRepository reisRepository, IKlantRepository klantRepository)
        {
            this.context = context;
            this.werelddeelRepository = werelddeelRepository;
            this.landRepository = landRepository;
            this.bestemmingRepository = bestemmingRepository;
            this.reisRepository = reisRepository;
            this.klantRepository = klantRepository;
            
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
            var gekozenBestemming = await bestemmingRepository.GetBestemming(code);
            var gekozenReis = await reisRepository.GetReis(id);
            if (gekozenReis == null) 
            { 
                return NotFound(); 
            }

            ;

            HttpContext.Session.SetString("Bestemming", gekozenBestemming.Plaats);
            HttpContext.Session.SetString("Vertrek", gekozenReis.Vertrek.ToString("dd-MM-yyyy"));
            HttpContext.Session.SetInt32("AantalDagen", gekozenReis.AantalDagen);
            HttpContext.Session.SetString("PrijsPerPersoon", gekozenReis.PrijsPerPersoon.ToString("F2"));
            HttpContext.Session.SetInt32("ReisId", gekozenReis.Id);

            ViewBag.Bestemming = HttpContext.Session.GetString("Bestemming");
            ViewBag.Vertrek = HttpContext.Session.GetString("Vertrek");
            ViewBag.Dagen = HttpContext.Session.GetInt32("AantalDagen");
            ViewBag.Prijs = HttpContext.Session.GetString("PrijsPerPersoon");

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

            ViewBag.Bestemming = HttpContext.Session.GetString("Bestemming");
            ViewBag.Vertrek = HttpContext.Session.GetString("Vertrek");
            ViewBag.Dagen = HttpContext.Session.GetInt32("AantalDagen");
            ViewBag.Prijs = HttpContext.Session.GetString("PrijsPerPersoon");

            return View("ZoekForm", form);
        }

        public async Task<IActionResult> BoekingForm(int id)
        {
            ViewBag.Bestemming = HttpContext.Session.GetString("Bestemming");
            ViewBag.Vertrek = HttpContext.Session.GetString("Vertrek");
            ViewBag.Dagen = HttpContext.Session.GetInt32("AantalDagen");
            ViewBag.Prijs = HttpContext.Session.GetString("PrijsPerPersoon");
            ViewBag.ReisId = HttpContext.Session.GetInt32("ReisId");
            var gekozenKlant = await klantRepository.GetKlant(id);
            if (gekozenKlant == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetInt32("KlantId", gekozenKlant.Id);
            HttpContext.Session.SetString("KlantFamilie", gekozenKlant.Familienaam);
            HttpContext.Session.SetString("KlantVoornaam", gekozenKlant.Voornaam);
            HttpContext.Session.SetString("KlantAdres", gekozenKlant.Adres);
            HttpContext.Session.SetString("KlantWoonplaats", gekozenKlant.Woonplaats.Naam);
            HttpContext.Session.SetInt32("KlantPostcode", gekozenKlant.Woonplaats.Postcode);

            ViewBag.KlantNaam = HttpContext.Session.GetString("KlantVoornaam") + "" + HttpContext.Session.GetString("KlantFamilie");
            ViewBag.Adres = HttpContext.Session.GetString("KlantAdres"); 
            ViewBag.Woonplaats = HttpContext.Session.GetString("KlantWoonplaats") + "" + HttpContext.Session.GetInt32("KlantPostcode");


            return View(new BoekingViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Boeking(BoekingViewModel form) 
        {
            if (form.AantalVolwassenen < 1)
            {
                ModelState.AddModelError("", "Er moet minstens één volwassene zijn.");
                return View("BoekingForm", form);
            }

            var boeking = new Boeking
            {
                Klantid = (int)HttpContext.Session.GetInt32("KlantId"),
                Reisid = (int)HttpContext.Session.GetInt32("ReisId"),
                GeboektOp = DateOnly.FromDateTime(DateTime.Now),
                AantalVolwassenen = form.AantalVolwassenen,
                AantalKinderen = form.AantalKinderen,
                AnnulatieVerzekering = form.Annulatieverzekering
            };

            context.Boekingen.Add(boeking);
            await context.SaveChangesAsync();

            int boekingsNummer = boeking.Id;

            return RedirectToAction("Bevestiging", new { boekingId = boeking.Id });
        }
        public async Task<IActionResult> Bevestiging(int boekingId)
        {
            var boeking = await context.Boekingen.Include(b => b.Reis).Include(b => b.Klant).FirstOrDefaultAsync(b => b.Id == boekingId);
            if (boeking == null)
            {
                return NotFound();
            }

            ViewBag.Boodschap = $"Uw reis is geboekt met het nummer {boeking.Id}";

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

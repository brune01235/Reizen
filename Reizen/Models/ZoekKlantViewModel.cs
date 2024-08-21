using System.ComponentModel.DataAnnotations;
namespace Reizen.Models
{
    public class ZoekKlantViewModel
    {
        [Display(Name = "Familienaam bevat:")]
        public string? BeginFamilienaam { get; set; }
        public List<Klant> Klanten { get; set; } = new List<Klant>();
    }
}

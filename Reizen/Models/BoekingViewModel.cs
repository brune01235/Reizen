using System.ComponentModel.DataAnnotations;

namespace Reizen.Models
{
    public class BoekingViewModel
    {
        [Required(ErrorMessage = "Aantal volwassenen is verplicht.")]
        [Range(1, int.MaxValue, ErrorMessage = "Er moet minstens één volwassene zijn.")]
        public int AantalVolwassenen { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Aantal kinderen moet 0 of meer zijn.")]
        public int AantalKinderen { get; set; }

        public bool Annulatieverzekering { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
namespace SaloonReservation.Models
{
    public class SoicialMidiaLink
    {
        [Key]
        public int SoicialMidiaLinkId { get; set; }
        public string Facebook { get; set; }
        public string WhatsApp { get; set; }
        public string LinkedIn { get; set; }
        public string Instgram { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }
    }
}

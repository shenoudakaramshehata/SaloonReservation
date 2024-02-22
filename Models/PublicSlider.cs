using System.ComponentModel.DataAnnotations;
namespace SaloonReservation.Models
{
    public class PublicSlider
    {
        [Key]
        public int PublicSliderId { get; set; }
        public string Title1Ar { get; set; }
        public string Title1En { get; set; }
        public string Title2En { get; set; }
        public string Title2Ar { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Background { get; set; }
        public bool IsImage { get; set; }
    }
}

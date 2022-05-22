using System.ComponentModel.DataAnnotations;

namespace UrunListem.Models
{
    public class Resim
    {
        public int Id {get;set;}
        public string ResimAdi {get;set;}
        public int UrunuId { get; set; }
        [Required]
        public Urun Urunu { get; set; }

    }
}
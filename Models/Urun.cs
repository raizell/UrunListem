using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace UrunListem.Models
{
    public class Urun
    {
        public Urun()
        {
            Resimler = new List<Resim>();
        }

        [Display(Name="Ürün No")]
        public int Id {get; set;}
        [Required (ErrorMessage="{0} alanı boş bırakılamaz.")]
        [StringLength( 50, ErrorMessage = "{0} 50 karakterden uzun olamaz." )]
        [Display(Name="Adı")]
        public string Adi {get; set;}
        [Display(Name="Tür")]
        public string Tur {get;set;}
        [Display(Name="Renk")]
        public string Renk {get; set;}
        [Display(Name="Açıklama")]
        public string Aciklama {get; set;}
        [Required (ErrorMessage="{0} alanı boş bırakılamaz.")]
        [Display(Name="Fiyat")]
        [DisplayFormat(ApplyFormatInEditMode=false, DataFormatString="{0:C}")]
        public decimal Fiyat {get; set;}

        [NotMapped]
        public IFormFile[] Dosyalar {get;set;}




        public List<Resim> Resimler {get;set;}
    }
}
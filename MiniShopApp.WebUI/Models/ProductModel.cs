using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        //[Required(ErrorMessage ="ürün ismi zorunludur!")]
        //[StringLength(50,MinimumLength =5,ErrorMessage = "ürün adı uzunluğu 5-50 karakter olmak  zorundadır!")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Lütfen Fiyat Bilgisi Giriniz!")]
        //[Range(1,50000, ErrorMessage = "Lütfen 1 ile 50000 arasında Fiyat Bilgisi Giriniz!")]
        public decimal? Price { get; set; }
        //[Required(ErrorMessage = "Açıklama zorunludur!")]
        //[StringLength(500, MinimumLength = 20, ErrorMessage = "Açıklama 20-500 karakter olmak  zorundadır!")]
        public string Description { get; set; }
        //[Required(ErrorMessage = "Ürün fotoğrafı zorunludur!")]
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}

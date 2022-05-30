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

        [Required(ErrorMessage = "Ürün ismi zorunludur!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Ürün ismi 5-50 karakter uzunluğunda olmalıdır!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen fiyat bilgisini giriniz!")]
        [Range(1, 50000, ErrorMessage = "Lütfen 1-50000 arasında bir değer giriniz!")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur!")]
        [StringLength(500, MinimumLength = 20, ErrorMessage = "Açıklama 20-500 karakter uzunluğunda olmalıdır!")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        //[Required(ErrorMessage ="Lütfen URL bilgisini giriniz!")]
        public string Url { get; set; }

        public bool IsApproved { get; set; }

        public bool IsHome { get; set; }

        public List<Category> SelectedCategories { get; set; }
    }
}

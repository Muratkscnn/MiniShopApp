using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Models
{
    public class ProductListViewModel
    {
        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }
    }

    public class PageInfo
    {
        public int TotalItems { get; set; }//Toplam item sayısı
        public int ItemsPerPage { get; set; }//Her sayfada gösterilecek item sayısı
        public int CurrentPage { get; set; }//O sırada geçerli sayfa numarası
        public string CurrentCategory { get; set; }//Geçerli kategori

        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
        }
    }
}

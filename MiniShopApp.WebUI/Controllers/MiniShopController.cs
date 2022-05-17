using Microsoft.AspNetCore.Mvc;
using MiniShopApp.Business.Abstract;
using MiniShopApp.Entity;
using MiniShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Controllers
{
    public class MiniShopController : Controller
    {
        IProductService _product;

        public MiniShopController(IProductService product)
        {
            _product = product;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List(string Category,int page=1)
        {
            ViewBag.AlertType = "Success";
            ViewBag.Message = "Ürün Bulunamadı";
            const int pageSize = 3;
            var totalItems = _product.GetCountByCategory(Category);
            var productListViewModel = new ProductListViewModel()
            {
                PageInfo = new PageInfo { TotalItems = totalItems, CurrentPage = page, ItemsPerPage = pageSize, CurrentCategory = Category },
                Products = _product.GetProductsByCategory(Category,page,pageSize)
            };
            return View(productListViewModel); 
        }
        public IActionResult Details(string url)
        {
            if (url==null)
            {
                return NotFound();
            }
            Product product = _product.GetProductDetails(url);
            if (product==null)
            {
                return NotFound();
            }
            ProductDetailModel productDetail = new ProductDetailModel() { Product = product, Categories = product.ProductCategories.Select(x => x.Category).ToList() };
            return View(productDetail);
        }
        [HttpPost]
        public IActionResult Search(string q)
        {
            
            return View(_product.GetSearchResult(q));
        }
    }
}

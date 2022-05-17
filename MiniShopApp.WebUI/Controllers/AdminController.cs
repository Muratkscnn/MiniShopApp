using Microsoft.AspNetCore.Mvc;
using MiniShopApp.Business.Abstract;
using MiniShopApp.Business.Concrete;
using MiniShopApp.Entity;
using MiniShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IProductService _productService;
        ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            return View(_productService.GetAll());
        }
        public IActionResult ProductCreate()
        {
            ViewBag.Categories = _categoryService.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(ProductModel model,int[] categoryIds)
        {
            var product = new Product()
            {
                Name = model.Name,
                Url = model.Url,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                IsApproved = model.IsApproved,
                IsHome = model.IsHome
            };
            _productService.Create(product,categoryIds);
            return RedirectToAction("ProductList");
        }
        public IActionResult ProductEdit(int? id)
        {
            var entity = _productService.GetByIdWithCategories((int)id);
            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(x => x.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }
        [HttpPost]
        public IActionResult ProductEdit(ProductModel model,int[] categoryIds)
        {
            var entity = _productService.GetById(model.ProductId);
            entity.Name = model.Name;
            entity.Price = model.Price;
            entity.Url = model.Url;
            entity.Description = model.Description;
            entity.IsApproved = model.IsApproved;
            entity.IsHome = model.IsHome;
            entity.ImageUrl = model.ImageUrl;
            _productService.Update(entity, categoryIds);
            return RedirectToAction("ProductList");
        }
        public IActionResult ProductDelete(int productId)
        {
            var entity=_productService.GetById(productId);
            _productService.Delete(entity);
            return RedirectToAction("ProductList");
        }
        public IActionResult IsApprovedActive(int productId)
        {
            var entity=_productService.GetById(productId);
            entity.IsApproved = true;
            _productService.Update(entity);
            return RedirectToAction("ProductList");
        }
        public IActionResult IsApprovedPassive(int productId)
        {
            var entity = _productService.GetById(productId);
            entity.IsApproved = false;
            _productService.Update(entity);
            return RedirectToAction("ProductList");
        }
    }
}

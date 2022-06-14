using Microsoft.EntityFrameworkCore;
using MiniShopApp.Data.Abstract;
using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product>, IProductRepository
    {
        public EfCoreProductRepository(MiniShopContext MiniShopContext) : base(MiniShopContext)
        {

        }
        private MiniShopContext MiniShopContext
        {
            get { return _context as MiniShopContext; }
        }
        private string ConvertLower(string text)
        {
            //İstanbul Irak Üzgün Şelaler Satırarası
            text = text.Replace("I", "i");//İstanbul irak Üzgün Şelaleler Satırarası
            text = text.Replace("İ", "i");//istanbul irak Üzgün Şelaleler Satırarası
            text = text.Replace("ı", "i");//istanbul irak Üzgün Şelaleler Satirarasi

            text = text.ToLower();//istanbul irak üzgün şelaleler satirarasi
            text = text.Replace("ç", "c");
            text = text.Replace("ö", "o");
            text = text.Replace("ü", "u");
            text = text.Replace("ş", "s");
            text = text.Replace("ğ", "g");
            return text;
        }

        public List<Product> GetSearchResult(string searchString)
        {

            searchString = ConvertLower(searchString);
            var products = MiniShopContext
                .Products
                .Where(i => i.IsApproved).ToList();
            foreach (var item in products)
            {
                item.Name = ConvertLower(item.Name);
                item.Description = ConvertLower(item.Description);
            }
            var products2 = products
                .Where(i => i.Name == searchString || i.Description == searchString)
                .ToList();

            return products2;
        }
        public List<Product> GetHomePageProducts()
        {
            return MiniShopContext
                .Products
                .Where(i => i.IsApproved && i.IsHome)
                .ToList();
        }

        public Product GetProductDetails(string url)
        {
            return MiniShopContext
                .Products
                .Where(i => i.Url == url)
                .Include(i => i.ProductCategories)
                .ThenInclude(i => i.Category)
                .FirstOrDefault();
        }


        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            var products = MiniShopContext
                .Products
                .Where(i => i.IsApproved)
                .AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                products = products
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .Where(i => i.ProductCategories.Any(a => a.Category.Url == name));
            }
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetCountByCategory(string category)
        {
            var products = MiniShopContext
                .Products
                .Where(i => i.IsApproved)
                .AsQueryable();
            if (!string.IsNullOrEmpty(category))
            {
                products = products
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .Where(i => i.ProductCategories.Any(a => a.Category.Url == category));
            }
            return products.Count();
        }

        public void Create(Product entity, int[] categoryIds)
        {
            MiniShopContext.Products.Add(entity);
            MiniShopContext.SaveChanges();
            entity.ProductCategories = categoryIds
                .Select(catId => new ProductCategory
                {
                    ProductId = entity.ProductId,
                    CategoryId = catId
                }).ToList();
        }

        public void Update(Product entity, int[] categoryIds)
        {
            var product = MiniShopContext
                .Products
                .Include(i => i.ProductCategories)
                .FirstOrDefault(i => i.ProductId == entity.ProductId);
            product.Name = entity.Name;
            product.Price = entity.Price;
            product.Description = entity.Description;
            product.Url = entity.Url;
            product.ImageUrl = entity.ImageUrl;
            product.IsApproved = entity.IsApproved;
            product.IsHome = entity.IsHome;
            product.ProductCategories = categoryIds
                .Select(catId => new ProductCategory()
                {
                    ProductId = entity.ProductId,
                    CategoryId = catId
                }).ToList();
        }

        public Product GetByIdWithCategories(int id)
        {
            return MiniShopContext
                .Products
                .Where(i => i.ProductId == id)
                .Include(i => i.ProductCategories)
                .ThenInclude(i => i.Category)
                .FirstOrDefault();
        }
    }
}

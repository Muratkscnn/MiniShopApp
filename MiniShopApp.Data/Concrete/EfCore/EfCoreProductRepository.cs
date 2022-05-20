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
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, MiniShopContext>, IProductRepository
    {
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
            // Burada metodun döndürdüğü değer string, ama biz linq sorgularıyla çalışırken
            // işimize yaramıyor!DÜZELTİLECEK
            using (var context = new MiniShopContext())
            {
                var products = context
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
        }
        public List<Product> GetHomePageProducts()
        {
            using (var context = new MiniShopContext())
            {
                return context
                    .Products
                    .Where(i => i.IsApproved && i.IsHome)
                    .ToList();
            }
        }

        public Product GetProductDetails(string url)
        {
            using (var context = new MiniShopContext())
            {
                return context
                    .Products
                    .Where(i => i.Url == url)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
        }

        //Burada görünmeseler de EfCoreGenericRepository classımızdaki tüm metotlar var.
        //Temel CRUD işlemlerini yapan 5 metot.
        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using (var context= new MiniShopContext())
            {
                var products = context
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

        }

        public int GetCountByCategory(string category)
        {
            using (var context = new MiniShopContext())
            {
                var products = context
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
        }

        public void Create(Product entity, int[] categoryIds)
        {
            using (var context = new MiniShopContext())
            {
                context.Products.Add(entity);
                context.SaveChanges();
                entity.ProductCategories = categoryIds
                    .Select(catId => new ProductCategory
                    {
                        ProductId = entity.ProductId,
                        CategoryId = catId
                    }).ToList();
                context.SaveChanges();
            }

        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new MiniShopContext())
            {
                var product = context
                    .Products
                    .Include(i => i.ProductCategories)
                    .FirstOrDefault(i=>i.ProductId==entity.ProductId);
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
                context.SaveChanges();
            }
        }

        public Product GetByIdWithCategories(int id)
        {
            using (var context = new MiniShopContext())
            {
                return context
                    .Products
                    .Where(i => i.ProductId == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
        }
    }
}

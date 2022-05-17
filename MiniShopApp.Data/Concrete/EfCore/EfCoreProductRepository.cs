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
        public List<Product> GetHomePageProducts()
        {
            using (var context = new MiniShopContext())
            {
                return context.Products.Where(x => x.IsApproved && x.IsHome).ToList();
            }
        }

        public Product GetProductDetails(string url)
        {
            using (var context=new MiniShopContext())
            {
                return context.Products.Where(x => x.Url == url).Include(x => x.ProductCategories).ThenInclude(x => x.Category).FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string name,int page,int pageSize)
        {
            using (var context=new MiniShopContext())
            {
                var products = context.Products.Where(x => x.IsApproved).AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    products = products.Include(x => x.ProductCategories).ThenInclude(x => x.Category).Where(x => x.ProductCategories.Any(i => i.Category.Url == name));
                }
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }
        private string CovertString(string text)
        {
            text = text.Replace("I", "i");
            text = text.Replace("İ", "i");
            text = text.Replace("ı", "i");
            text = text.ToLower();
            text = text.Replace("ç", "c");
            text = text.Replace("ş", "s");
            text = text.Replace("ö", "o");
            text = text.Replace("ü", "u");
            text = text.Replace("ğ", "g");
            return text;
        }

        public List<Product> GetSearchResult(string searchString)
        {

            searchString = searchString.ToLower();

            using (var context = new MiniShopContext())
            {
                var products = context.Products.Where(x => x.IsApproved && (x.Name.ToLower().Contains(searchString) || x.Description.ToLower().Contains(searchString))).ToList();
                return products;
            }
            
        }

        public int GetCountByCategory(string category)
        {
            using (var context =new MiniShopContext())
            {
                var products = context.Products.Where(x => x.IsApproved).AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products.Include(x => x.ProductCategories).ThenInclude(x => x.Category).Where(x => x.ProductCategories.Any(i => i.Category.Url == category));
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
                entity.ProductCategories = categoryIds.Select(catId => new ProductCategory
                {
                    ProductId = entity.ProductId,
                    CategoryId = catId
                }).ToList();
                context.SaveChanges();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context=new MiniShopContext())
            {
                var product = context.Products.Include(x => x.ProductCategories).FirstOrDefault(i=>i.ProductId==entity.ProductId);
                product.Name = entity.Name;
                product.Price = entity.Price;
                product.Url = entity.Url;
                product.Description = entity.Description;
                product.IsApproved = entity.IsApproved;
                product.IsHome = entity.IsHome;
                product.ImageUrl = entity.ImageUrl;
                product.ProductCategories = categoryIds.Select(catId => new ProductCategory()
                {
                    ProductId = entity.ProductId,
                    CategoryId = catId
                }).ToList();
                context.SaveChanges();
            }
        }

        public Product GetByIdWithCategories(int id)
        {
            using (var context=new MiniShopContext())
            {
              return  context.Products.Where(x => x.ProductId == id).Include(x => x.ProductCategories).ThenInclude(x=>x.Category).FirstOrDefault();
            }
        }
    }
}

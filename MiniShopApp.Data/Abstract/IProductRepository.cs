using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Data.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        //Product'a özgü memberlarımı buraya yazabilirim.
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        Product GetProductDetails(string url);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        int GetCountByCategory(string category);
        void Create(Product entity, int[] categoryIds);
        void Update(Product entity, int[] categoryIds);
        Product GetByIdWithCategories(int id);
    }
}

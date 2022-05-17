using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Business.Abstract
{
    public interface IProductService
    {
        void Create(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
        Product GetById(int id);
        List<Product> GetAll();
        List<Product> GetProductsByCategory(string name,int page, int pageSize);
        Product GetProductDetails(string url);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        int GetCountByCategory(string category);
        void Create(Product entity, int[] categoryIds);
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryIds);


    }
}

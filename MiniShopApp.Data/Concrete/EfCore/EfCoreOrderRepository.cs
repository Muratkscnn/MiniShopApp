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
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, MiniShopContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId=null)
        {
            using (MiniShopContext c=new MiniShopContext())
            {
                var orders= c.Orders.Include(x => x.OrderItems).ThenInclude(y => y.Product)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(x => x.UserId == userId);
                }
                return orders.ToList();
            }
        }
    }
}

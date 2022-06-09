using MiniShopApp.Data.Abstract;
using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order,MiniShopContext>,IOrderRepository
    {
    }
}

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
    public class EfCoreCardRepository : EfCoreGenericRepository<Card, MiniShopContext>, ICardRepository
    {
        public Card GetCardByUseId(string userId)
        {
            using (MiniShopContext c =new MiniShopContext())
            {
               return c.Cards.Include(x=>x.CardItems).ThenInclude(y=>y.Product).Where(x => x.UserId == userId).FirstOrDefault();
            }
        }
    }
}

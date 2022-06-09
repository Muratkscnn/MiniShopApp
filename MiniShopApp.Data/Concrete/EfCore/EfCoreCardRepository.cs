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
        public void DeleteFromCard(int cardId, int productId)
        {
            using (MiniShopContext c = new MiniShopContext())
            {
                var query = @"DELETE FROM CardItems WHERE CardId=@p0 AND ProductId=@p1";
                c.Database.ExecuteSqlRaw(query,cardId,productId);
                
            }
        }
        public void ClearCard(int cardId)
        {
            using (MiniShopContext c = new MiniShopContext())
            {
                var query = @"DELETE FROM CardItems WHERE CardId=@p0";
                c.Database.ExecuteSqlRaw(query, cardId);

            }
        }

        public Card GetCardByUseId(string userId)
        {
            using (MiniShopContext c =new MiniShopContext())
            {
               return c.Cards.Include(x=>x.CardItems).ThenInclude(y=>y.Product).Where(x => x.UserId == userId).FirstOrDefault();
            }
        }
        public override void Update(Card entity)
        {
            using (var context = new MiniShopContext())
            {
                context.Cards.Update(entity);
                context.SaveChanges();
            }
            base.Update(entity);
        }
    }
}

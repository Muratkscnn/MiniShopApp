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
    public class EfCoreCardRepository : EfCoreGenericRepository<Card>, ICardRepository
    {
        public EfCoreCardRepository(MiniShopContext context):base(context)
        {

        }
        private MiniShopContext MiniShopContext
        {
            get { return _context as MiniShopContext; }
        }
        public void ClearCard(int cardId)
        {
            var query = @"DELETE FROM CardItems WHERE CardId=@p0";
            MiniShopContext.Database.ExecuteSqlRaw(query, cardId);
        }

        public void DeleteFromCard(int cardId, int productId)
        {
            var query = @"DELETE FROM CardItems WHERE CardId=@p0 AND ProductId=@p1";
            MiniShopContext.Database.ExecuteSqlRaw(query, cardId, productId);
        }

        public Card GetCardByUserId(string userId)
        {
            return MiniShopContext
                .Cards
                .Include(i => i.CardItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(i => i.UserId == userId);
        }
        public override void Update(Card entity)
        {
            MiniShopContext.Cards.Update(entity);
        }
    }
}

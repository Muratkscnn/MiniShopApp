using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Data.Abstract
{
    public interface ICardRepository : IRepository<Card>
    {
        Card GetCardByUserId(string userId);
        void DeleteFromCard(int cardId, int productId);
        void ClearCard(int cardId);
    }
}

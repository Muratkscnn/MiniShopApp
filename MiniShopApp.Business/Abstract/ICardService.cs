using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Business.Abstract
{
    public interface ICardService
    {
        void InitializeCard(string userId);
        Card GetCardByUserId(string userId);
        void AddToCard(string userId, int productId, int quantity);
        void DeleteFromCard(string userId, int productId);
        void ClearCard(int cardId);
    }
}

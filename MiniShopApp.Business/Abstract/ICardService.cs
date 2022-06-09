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
        Card GetCardByUserId(string userId);
        void InitializeCard(string userId);
        void AddToCard(string userId,int ProductId,int Quantity);
        void DeleteFromCard(string userId, int productId);
        void ClearCard(string userId);

    }
}

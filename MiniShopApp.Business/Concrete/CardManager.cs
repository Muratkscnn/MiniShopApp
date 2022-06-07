using MiniShopApp.Business.Abstract;
using MiniShopApp.Data.Abstract;
using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Business.Concrete
{
    public class CardManager : ICardService
    {
        private ICardRepository _cardRepsitory;

        public CardManager(ICardRepository cardRepsitory)
        {
            _cardRepsitory = cardRepsitory;
        }

        public void AddToCard(string userId, int ProductId, int Quantity)
        {
            var card = GetCardByUserId(userId);
            if (card!=null)
            {
                var index = card.CardItems.FindIndex(i => i.ProductId == ProductId);
                if (index<0)
                {
                    card.CardItems.Add(new CardItem()
                    {
                        ProductId = ProductId,
                        Quantity = Quantity,
                        CardId = card.Id
                    });
                }
                else
                {
                    card.CardItems[index].Quantity += Quantity;
                }
                _cardRepsitory.Update(card);
            }
        }

        public void DeleteFromCard(string userId, int productId)
        {
            var card = GetCardByUserId(userId);
            if (card!=null)
            {
                _cardRepsitory.DeleteFromCard(card.Id,productId);
            }
        }

        public Card GetCardByUserId(string userId)
        {
            return _cardRepsitory.GetCardByUseId(userId);
        }

        public void InitializeCard(string userId)
        {
            var card = new Card() { UserId = userId };
            _cardRepsitory.Create(card);
        }
    }
}

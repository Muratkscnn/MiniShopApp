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
        private readonly IUnitOfWork _unitOfWork;

        public CardManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddToCard(string userId, int productId, int quantity)
        {
            var card = GetCardByUserId(userId);
            if (card!=null)
            {
                var index = card.CardItems.FindIndex(i => i.ProductId == productId);
                if (index<0)
                {
                    card.CardItems.Add(new CardItem()
                    {
                        ProductId=productId,
                        Quantity=quantity,
                        CardId=card.Id
                    });
                }
                else
                {
                    card.CardItems[index].Quantity += quantity;
                }
                _unitOfWork.Cards.Update(card);
                _unitOfWork.Save();
            }
        }

        public void ClearCard(int cardId)
        {
            _unitOfWork.Cards.ClearCard(cardId);
        }

        public void DeleteFromCard(string userId, int productId)
        {
            var card = GetCardByUserId(userId);
            if (card!=null)
            {
                _unitOfWork.Cards.DeleteFromCard(card.Id,productId);
            }
        }

        public Card GetCardByUserId(string userId)
        {
            return _unitOfWork.Cards.GetCardByUserId(userId);
        }

        public void InitializeCard(string userId)
        {
            var card = new Card() { UserId = userId };
            _unitOfWork.Cards.Create(card);
            _unitOfWork.Save();
        }
    }
}

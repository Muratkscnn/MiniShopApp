using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Models
{
    public class CardModel
    {
        public int CardId { get; set; }
        public List<CardItemModel> CardItems { get; set; }
        public double TotalPrice()
        {
            return CardItems.Sum(i => i.Price * i.Quantity);
        }
    }
}

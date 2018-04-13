using System.Collections.Generic;
using Vue.Domain.Cards;
using System.Linq;

namespace Vue.Domain
{
    public class User 
    {
        public User(string username)
        {
            Username = username;
        }
        public string Username { get; }

        public List<Card> AvailableCards { get; } = new List<Card>();
        public List<Card> Played { get; set; } = new List<Card>();
        public List<Card> PlayedFront => Played.Where(x => x.Row == Row.Front).ToList();
        public List<Card> PlayedBack => Played.Where(x => x.Row == Row.Back).ToList();
        public List<Card> Hand { get; set; } = new List<Card>();

        public void AddAvailableCard(Card card)
        {
            if (card == null) return;
            card.User = this;
            AvailableCards.Add(card);
        }

        public void AddPlayedCard(Card card)
        {
            if (card == null) return;
            card.User = this;
            Played.Add(card);
        }

        public void AddPlayedCards(List<Card> cards)
        {
            if (cards == null) return;
            foreach (var card in cards)
            {
                card.User = this;
            }
            Played.AddRange(cards);
        }

        public void AddHandCards()
        {
            var cards = Dealer.GetRandomHand(Played);
            foreach (var card in cards)
            {
                card.User = this;
            }
            Hand.AddRange(cards);
        }
    }
}

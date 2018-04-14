using System.Collections.Generic;
using Vue.Domain.Cards;
using System.Linq;
using Vue.Utility;

namespace Vue.Domain
{
    public class User 
    {
        public User(string username, string deck)
        {
            Username = username;
            CurrentDeck = deck;
        }
        public string Username { get; }
        public string CurrentDeck { get; set; }
        public List<Card> Played { get; set; } = new List<Card>();
        public List<Card> PlayedFront => Played.Where(x => x.Row == Row.Front).ToList();
        public List<Card> PlayedBack => Played.Where(x => x.Row == Row.Back).ToList();
        public List<Card> Hand { get; set; } = new List<Card>();

        public void AddPlayedCard(Card card)
        {
            if (card == null) return;
            card.User = this.BasicUser();
            Played.Add(card);
        }

        public void ResetHandCards()
        {
            Hand = new List<Card>();
            var cards = Dealer.GetRandomHand(this);
            foreach (var card in cards)
            {
                card.User = this.BasicUser();
            }
            Hand.AddRange(cards);
        }
    }

    public class BasicUser
    {
        public string Username { get; set; }
    }
}

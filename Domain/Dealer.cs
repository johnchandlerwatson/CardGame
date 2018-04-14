using System;
using System.Linq;
using System.Collections.Generic;
using Vue.Domain.Cards;

namespace Vue.Domain
{
    public static class Dealer 
    {
        public static List<IDeck> AllDecks => new List<IDeck>
        {
            new HumanDeck()
        };

        public static IDeck RandomDeck()
        {
            var rand = new Random();
            var index = rand.Next(AllDecks.Count);
            return AllDecks[index];
        }

        public static List<Card> DeckCards(string deckName) => AllDecks.First(x => x.Name == deckName).Cards;

        private static Card GetRandomCard(string deck)
        {
            var rand = new Random();
            var deckCards = DeckCards(deck);
            var index = rand.Next(deckCards.Count);
            return deckCards[index];
        }

        public static Card GetRandomCard(User user)
        {
            var randomCard = GetRandomCard(user.CurrentDeck);
            Card cardToAdd = null;
            
            while (cardToAdd == null)
            {
                cardToAdd = AddCardIfAllowed(user.Played, randomCard);
            }
            return cardToAdd;
        }

        public static List<Card> GetRandomHand(User user)
        {
            var hand = new List<Card>();
            for (var i = 0; i < 3; i++)
            {
                Card cardToAdd = null;
                var randomCard = GetRandomCard(user.CurrentDeck);
                if (!hand.Any(x => x.Name == randomCard.Name))
                {
                    cardToAdd = AddCardIfAllowed(hand, randomCard, user.Played);
                }

                if (cardToAdd == null)
                {
                    i = i - 1; //try again
                }
                else 
                {
                    hand.Add(cardToAdd);
                }
            }
            return hand;
        }

        private static Card AddCardIfAllowed(List<Card> hand, Card randomCard, List<Card> cardsPlayed = null)
        {
            if (cardsPlayed != null && cardsPlayed.Any())
            {
                return AddCardIfDuplicatesAllowed(cardsPlayed, hand, randomCard);
            }
            else 
            {
                return randomCard;
            }
        }

        private static Card AddCardIfDuplicatesAllowed(List<Card> cardsPlayed, List<Card> hand, Card randomCard)
        {
            if (cardsPlayed.Any(x => x.Name == randomCard.Name))
            {
                if (randomCard.CanHaveDuplicates)
                {
                    return randomCard;
                }
                else 
                {
                    return null;
                }
            }
            else 
            {
                return randomCard;
            }
        }
    }
}
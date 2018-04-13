using System;
using System.Linq;
using System.Collections.Generic;
using Vue.Domain.Cards;

namespace Vue.Domain
{
    public static class Dealer 
    {
        public static List<Card> AllCards => new List<Card>
        {
            new AssassinCard(),
            new ArcherCard(),
            new KnightCard(),
            new HealerCard(),
            new CannonCard(),
            new EmperorCard(),
            new TrollCard(),
            new DragonCard()
        };

        private static Card GetRandomCard()
        {
            var rand = new Random();
            var index = rand.Next(AllCards.Count);
            return AllCards[index];
        }

        public static Card GetRandomCard(List<Card> cardsPlayed)
        {
            var randomCard = GetRandomCard();
            Card cardToAdd = null;
            
            while (cardToAdd == null)
            {
                cardToAdd = AddCardIfAllowed(cardsPlayed, randomCard);
            }
            return cardToAdd;
        }

        public static List<Card> GetRandomHand(List<Card> cardsPlayed = null)
        {
            var hand = new List<Card>();
            for (var i = 0; i < 3; i++)
            {
                Card cardToAdd = null;
                var randomCard = GetRandomCard();
                if (!hand.Any(x => x.Name == randomCard.Name))
                {
                    cardToAdd = AddCardIfAllowed(hand, randomCard, cardsPlayed);
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
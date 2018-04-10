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
            new TrollCard(),
            new DragonCard()
        };

        public static Card GetRandomCard()
        {
            var rand = new Random();
            var index = rand.Next(AllCards.Count);
            return AllCards[index];
        }

        public static List<Card> GetRandomHand()
        {
            var cards = new List<Card>();
            for (var i = 0; i < 3; i++)
            {
                var randomCard = GetRandomCard();
                if (!cards.Any(x => x.Name == randomCard.Name))
                {
                    cards.Add(randomCard);
                }
                else 
                {
                    i = i-1;
                }
            }
            return cards;
        }
    }
}
using System.Collections.Generic;
using Vue.Domain.Cards;
using System;

namespace Vue.Domain.Cards
{
    public class HumanDeck : IDeck
    {
        public Guid Id => Guid.NewGuid();
        public string Name => "Human";
        public string Description => "Versatile for any situation";
        public List<Card> Cards => new List<Card>
        {
            new KnightCard(),
            new AssassinCard(),
            new ArcherCard(),
            new HealerCard(),
            new CannonCard(),
            new EmperorCard()
        };
    }
}
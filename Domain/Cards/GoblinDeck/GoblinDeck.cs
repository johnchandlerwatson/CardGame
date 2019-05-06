using System.Collections.Generic;
using Vue.Domain.Cards;
using System;

namespace Vue.Domain.Cards
{
    public class GoblinDeck : IDeck
    {
        public Guid Id => Guid.NewGuid();
        public string Name => "Goblin";
        public string Description => "Pack a punch";
        public List<Card> Cards => new List<Card>
        {
            new GoblinGruntCard(),
            new GoblinArcherCard(),
            new GoblinShamanCard(),
            new TrollCard(),
            new DragonCard(),
            new OrcCard(),
            new GoblinKingCard()
        };
    }
}
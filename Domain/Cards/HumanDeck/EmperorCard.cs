using System;
using System.Collections.Generic;
using System.Linq;

namespace Vue.Domain.Cards
{
    public class EmperorCard : Card
    {
        public override string Name => "Emperor";

        public override Row Row => Row.Back;

        public override Row Targets => Row.Both;

        public override Rarity Rarity => Rarity.Legendary;

        public override int Speed => 2;

        public override int Damage { get; set; } = 0;

        public override int Healing => 0;

        public override string Description => "Increases the damage of all played cards by 3 when played and spawns a random human card every 2 moves";

        public override int MaxHealth => 20;

        public override void ApplyMove(List<Card> enemyCards, List<Card> friendlyCards, List<CardAction> actions)
        {
            var actionOverride = string.Empty;
            if (RoundsPlayed == 1)
            {
                if (friendlyCards.Any())
                {
                    foreach (var ally in friendlyCards)
                    {
                        ally.Damage = ally.Damage + 3;
                    }
                    actionOverride = $"{User.Username}'s {Name} card increased all played cards damage by 3! ";
                }
            }

            if (RoundsPlayed % 2 == 0)
            {
                var humanCards = new HumanDeck().Cards.Where(x => x.CanHaveDuplicates).ToList();
                var rand = new Random();
                var index = rand.Next(humanCards.Count);
                var cardToAdd = humanCards[index];
                friendlyCards.Add(cardToAdd);
                actionOverride = $"{actionOverride}{Environment.NewLine}{User.Username}'s {Name} card added a {cardToAdd.Name} card to the field!";
            }

            if (!string.IsNullOrEmpty(actionOverride))
            {
                actions.Add(new CardAction(actionOverride));
            }
        }
    }
}
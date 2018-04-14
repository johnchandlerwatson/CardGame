using System;
using System.Collections.Generic;

namespace Vue.Domain.Cards
{
    public class JoshDeck : IDeck
    {
        public Guid Id => Guid.NewGuid();
        public string Name => "Josh's Lame Deck";
        public string Description => "its terrible";
        public List<Card> Cards => new List<Card>()
        {
            new BurtsBonfire(),
            new BurtsCoders(),
            new ColonolMax(),
            new CrustyGumps(),
            new GeneralGump(),
            new Gumpers(),
            new HansGump(),
            new JohnnyWaffle(),
            new JosephGump(),
            new Macythewerewolf()
        };
    }
}
using System;
using System.Collections.Generic;

namespace Vue.Domain.Cards
{
    public class DaveDeck : IDeck
    {
        public Guid Id => Guid.NewGuid();
        public string Name => "Dave!";
        public string Description => "Dave's fancy deck";
        public List<Card> Cards => new List<Card>
        {
            new ChucksMutt(),
            new ChucktheProtector(),
            new ChucktheWise(),
            new DaveinShiningArmor(),
            new DavetheTrollofGondor(),
            new DwarfDave(),
            new ElfDave(),
            new HobbitDave(),
            new TheKindestDragonyouwillEverMeet(),
            new TreeofDave()
        };
    }
}
using System.Collections.Generic;

namespace Vue.Domain.Cards
{
    public interface IDeck 
    {
        string Name { get; }
        string Description { get; }
        List<Card> Cards { get; }
    }
}
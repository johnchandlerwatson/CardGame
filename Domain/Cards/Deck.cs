using System;
using System.Collections.Generic;

namespace Vue.Domain.Cards
{
    public interface IDeck 
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }
        List<Card> Cards { get; }
    }
}
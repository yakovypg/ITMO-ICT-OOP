using OOP_ICT.Models;
using System.Diagnostics.CodeAnalysis;

namespace OOP_ICT.First.Tests
{
    internal class CardEqualityComparer : IEqualityComparer<Card>
    {
        public bool Equals(Card x, Card y)
        {
            return x == null && y == null ||
                (x != null && y != null &&
                 x.Suit == y.Suit &&
                 x.Nominal.Name == y.Nominal.Name &&
                 x.Nominal.Weight == y.Nominal.Weight);
        }

        public int GetHashCode([DisallowNull] Card obj)
        {
            return HashCode.Combine(obj.Suit, obj.Nominal);
        }
    }
}

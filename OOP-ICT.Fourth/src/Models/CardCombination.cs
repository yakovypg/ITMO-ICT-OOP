using OOP_ICT.Models;

namespace OOP_ICT.Fourth.Models
{
    public class CardCombination
    {
        public CardCombination(CombinationType combination, IEnumerable<Card> cards)
        {
            Combination = combination;
            Cards = new List<Card>(cards);
        }

        public CombinationType Combination { get; }
        public IReadOnlyList<Card> Cards { get; }
    }
}

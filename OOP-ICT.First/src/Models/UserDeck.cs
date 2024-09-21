using OOP_ICT.Interfaces;

namespace OOP_ICT.Models;

public class UserDeck : ICardDeck
{
    private readonly List<Card> _cards;

    public UserDeck(IEnumerable<Card> cards)
    {
        if (cards is null)
            throw new ArgumentNullException(nameof(cards));

        _cards = new List<Card>(cards);
    }

    public IReadOnlyList<Card> Cards => _cards;
}

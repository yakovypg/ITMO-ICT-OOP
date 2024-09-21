using OOP_ICT.Interfaces;

namespace OOP_ICT.Models;

public class CardDeck : ICardDeck
{
    private readonly List<Card> _cards;

    public CardDeck()
    {
        _cards = GenerateCardDeck();
    }

    internal CardDeck(IEnumerable<Card> cards)
    {
        if (cards is null)
            throw new ArgumentNullException(nameof(cards));

        _cards = new List<Card>(cards);
    }

    public IReadOnlyList<Card> Cards => _cards;

    public List<Card> GetCards(int cardsCount)
    {
        if (cardsCount <= 0)
            throw new ArgumentException("The cards count must be grater than zero.",
                nameof(cardsCount));

        if (cardsCount > _cards.Count)
            throw new ArgumentException("The deck contains an insufficient number of cards.",
                nameof(cardsCount));
        
        List<Card> cards = _cards.Take(cardsCount).ToList();
        _cards.RemoveRange(0, cardsCount);

        return cards;
    }

    private static List<Card> GenerateCardDeck()
    {
        var cards = new List<Card>();

        var nominals = new List<CardNominal>()
        {
            new CardNominal("Ace", 0),
            new CardNominal("King", 0),
            new CardNominal("Queen", 0),
            new CardNominal("Jack", 0),
            new CardNominal("Ten", 0),
            new CardNominal("Nine", 0),
            new CardNominal("Eight", 0),
            new CardNominal("Seven", 0),
            new CardNominal("Six", 0),
            new CardNominal("Five", 0),
            new CardNominal("Four", 0),
            new CardNominal("Three", 0),
            new CardNominal("Two", 0)
        };

        for (int i = 0; i < nominals.Count; i++)
        {
            CardNominal currNominal = nominals[i];
            currNominal.Weight = int.MaxValue - i;

            cards.Add(new Card(CardSuit.Hearts, currNominal));
            cards.Add(new Card(CardSuit.Spades, currNominal));
            cards.Add(new Card(CardSuit.Clubs, currNominal));
            cards.Add(new Card(CardSuit.Diamonds, currNominal));
        }

        return cards;
    }
}

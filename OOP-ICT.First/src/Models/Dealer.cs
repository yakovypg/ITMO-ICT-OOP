using OOP_ICT.Interfaces;

namespace OOP_ICT.Models;

public class Dealer : IDealer
{
    private CardDeck _cardDeck;

    public void InitializeCardDeck()
    {
        _cardDeck = new CardDeck();
    }

    public void ShuffleCardDeck()
    {
        IReadOnlyList<Card> cards = _cardDeck.Cards;

        int half = cards.Count / 2;
        Card[] shuffledDeckCards = new Card[cards.Count];

        for (int i = 0; i < half; i++)
        {
            shuffledDeckCards[i * 2] = cards[i + half];
            shuffledDeckCards[i * 2 + 1] = cards[i];
        }

        _cardDeck = new CardDeck(shuffledDeckCards);
    }

    public UserDeck CreateShuffledUserDeck(int cardsCount)
    {
        List<Card> cards = _cardDeck.GetCards(cardsCount);
        return new UserDeck(cards);
    }
}
using OOP_ICT.Fourth.Infrastructure.Exceptions;
using OOP_ICT.Models;

namespace OOP_ICT.Fourth.Models
{
    public class PokerDealer : Dealer
    {
        private readonly List<Card> _cards;

        public PokerDealer()
        {
            _cards = new List<Card>();
        }

        public IReadOnlyList<Card> Cards => _cards;

        public void TakeCards(UserDeck deck)
        {
            if (deck is null)
                throw new ArgumentNullException(nameof(deck));

            if (deck.Cards.Count != Poker.PLAYER_CARDS_COUNT)
                throw new IncorrectCardsCountException($"The dealer can only take {Poker.PLAYER_CARDS_COUNT} cards.");

            _cards.Clear();
            _cards.AddRange(deck.Cards);
        }

        public void FoldCards()
        {
            _cards.Clear();
        }
    }
}

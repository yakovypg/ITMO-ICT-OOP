using OOP_ICT.Models;
using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Second.Models
{
    public class Player : IPlayer
    {
        private readonly List<Card> _cards;
        
        public Player(string name, int money)
        {
            Name = name;
            Money = money;

            _cards = new List<Card>();
        }

        public string Name { get; }
        public int Money { get; set; }

        public IReadOnlyList<Card> Cards => _cards;

        public void TakeCard(Card card)
        {
            if (card is null)
                throw new ArgumentNullException(nameof(card));

            _cards.Add(card);
        }

        public void TakeDeck(UserDeck deck)
        {
            if (deck is null)
                throw new ArgumentNullException(nameof(deck));

            _cards.Clear();
            _cards.AddRange(deck.Cards);
        }
    }
}

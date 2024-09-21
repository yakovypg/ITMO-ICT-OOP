using OOP_ICT.Models;

namespace OOP_ICT.Second.Interfaces
{
    public interface IPlayer
    {
        string Name { get; }
        int Money { get; set; }
        IReadOnlyList<Card> Cards { get; }

        void TakeCard(Card card);
        void TakeDeck(UserDeck deck);
    }
}

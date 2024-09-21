using OOP_ICT.Models;

namespace OOP_ICT.Interfaces;

public interface ICardDeck
{
    IReadOnlyList<Card> Cards { get; }
}
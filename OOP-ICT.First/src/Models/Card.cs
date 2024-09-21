namespace OOP_ICT.Models;

public enum CardSuit
{
    Hearts,
    Spades,
    Clubs,
    Diamonds
}

public class Card
{
    public CardSuit Suit { get; }
    public CardNominal Nominal { get; }

    public Card(CardSuit suit, CardNominal nominal)
    {
        if (nominal is null)
            throw new ArgumentNullException(nameof(nominal));

        Suit = suit;
        Nominal = nominal;
    }
}
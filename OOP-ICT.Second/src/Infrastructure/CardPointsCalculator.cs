using OOP_ICT.Models;
using OOP_ICT.Second.Models;

namespace OOP_ICT.Second.Infrastructure
{
    public static class CardPointsCalculator
    {
        public static int CalculatePoints(IReadOnlyList<Card> cards)
        {
            if (cards is null)
                throw new ArgumentNullException(nameof(cards));

            int currSum = cards.SkipLast(1).Sum(CalculateCardPoints);

            Card lastCard = cards[^1];

            return lastCard.Nominal.Name == "Ace" &&
                   currSum + Blackjack.DEFAULT_ACE_POINTS > Blackjack.WINNING_PLAYER_POINTS
                ? currSum + Blackjack.REDUCED_ACE_POINTS
                : currSum + CalculateCardPoints(lastCard);
        }

        public static int CalculateCardPoints(Card card)
        {
            if (card is null)
                throw new ArgumentNullException(nameof(card));

            return card.Nominal.Name switch
            {
                "Ace" => Blackjack.DEFAULT_ACE_POINTS,
                "Two" => 2,
                "Three" => 3,
                "Four" => 4,
                "Five" => 5,
                "Six" => 6,
                "Seven" => 7,
                "Eight" => 8,
                "Nine" => 9,
                _ => 10
            };
        }
    }
}

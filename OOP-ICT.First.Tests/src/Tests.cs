using Xunit;
using OOP_ICT.Models;

namespace OOP_ICT.First.Tests;

public class Tests
{
    private const int CARDS_COUNT = 52;

    [Fact]
    public void DealerShuffledDeck_True()
    {
        var dealer = new Dealer();

        dealer.InitializeCardDeck();
        dealer.ShuffleCardDeck();

        UserDeck userDeck = dealer.CreateShuffledUserDeck(CARDS_COUNT);

        bool result = false;

        for (int i = 0; i < userDeck.Cards.Count - 1; i++)
        {
            int currWeight = userDeck.Cards[i].Nominal.Weight;
            int nextWeight = userDeck.Cards[i + 1].Nominal.Weight;

            if (currWeight < nextWeight)
            {
                result = true;
                break;
            }
        }

        Assert.True(result);
    }

    [Fact]
    public void ShuffledDeckNotContainsIdenticalCards_True()
    {
        var dealer = new Dealer();

        dealer.InitializeCardDeck();
        dealer.ShuffleCardDeck();

        UserDeck userDeck = dealer.CreateShuffledUserDeck(CARDS_COUNT);

        int cardsCount = userDeck.Cards.Distinct(new CardEqualityComparer()).Count();

        Assert.Equal(CARDS_COUNT, cardsCount);
    }

    [Fact]
    public void ShuffledDeckContains52Cards_True()
    {
        var dealer = new Dealer();
        dealer.InitializeCardDeck();
        dealer.ShuffleCardDeck();

        UserDeck userDeck = dealer.CreateShuffledUserDeck(CARDS_COUNT);

        Assert.Equal(CARDS_COUNT, userDeck.Cards.Count);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(12)]
    public void ShuffledDeckContainsCorrectNumberOfCards_True(int userDeckCardsCount)
    {
        var dealer = new Dealer();
        dealer.InitializeCardDeck();
        dealer.ShuffleCardDeck();

        int remainingCardsCount = CARDS_COUNT;

        while (remainingCardsCount >= userDeckCardsCount)
        {
            UserDeck userDeck = dealer.CreateShuffledUserDeck(userDeckCardsCount);
            Assert.Equal(userDeckCardsCount, userDeck.Cards.Count);

            remainingCardsCount -= userDeckCardsCount;
        }

        Assert.Throws<ArgumentException>(() => 
            dealer.CreateShuffledUserDeck(userDeckCardsCount));
    }
}
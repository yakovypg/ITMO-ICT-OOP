using OOP_ICT.Models;
using OOP_ICT.Second.Infrastructure;
using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Second.Models
{
    public class BlackjackDealer : Dealer
    {
        private readonly List<Card> _cards;

        public BlackjackDealer()
        {
            _cards = new List<Card>();
        }

        public IReadOnlyList<Card> Cards => _cards;

        public void HandOutCards(IEnumerable<IPlayer> players)
        {
            if (players is null)
                throw new ArgumentNullException(nameof(players));
            
            InitializeCardDeck();
            ShuffleCardDeck();

            foreach (var player in players)
            {
                UserDeck playerDeck = CreateShuffledUserDeck(Blackjack.PLAYER_START_CARDS_COUNT);
                player.TakeDeck(playerDeck);
            }

            UserDeck dealerDeck = CreateShuffledUserDeck(Blackjack.DEALER_START_CARDS_COUNT);
            _cards.AddRange(dealerDeck.Cards);
        }

        public void TakeCards()
        {
            int dealerPoints = CardPointsCalculator.CalculatePoints(_cards);

            while (dealerPoints < Blackjack.MIN_DEALER_POINTS)
            {
                Card card = CreateShuffledUserDeck(1).Cards[0];
                _cards.Add(card);

                dealerPoints = CardPointsCalculator.CalculatePoints(_cards);
            }
        }

        public void FoldCards()
        {
            _cards.Clear();
        }
    }
}

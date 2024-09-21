using OOP_ICT.Models;
using OOP_ICT.Second.Infrastructure;
using OOP_ICT.Second.Infrastructure.Exceptions;
using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Second.Models
{
    public class Blackjack
    {
        #region Constants

        public const int MIN_DEALER_POINTS = 17;
        public const int WINNING_PLAYER_POINTS = 21;

        public const int DEALER_START_CARDS_COUNT = 1;
        public const int PLAYER_START_CARDS_COUNT = 2;

        public const int DEFAULT_ACE_POINTS = 11;
        public const int REDUCED_ACE_POINTS = 1;

        public const int WINNING_RATIO = 2;

        #endregion

        protected readonly Dictionary<IPlayer, int> _players; // Player -> Bet
        private readonly BlackjackDealer _dealer;

        public Blackjack(BlackjackBuilder builder)
        {
            _dealer = builder.BlackjackDealer ?? throw new DealerNotFoundException();
            _players = new Dictionary<IPlayer, int>(builder.Players);
        }

        public bool IsGameStarted { get; private set; }
        public IReadOnlyDictionary<IPlayer, int> Players => _players;

        /// <summary>
        /// Makes hit.
        /// </summary>
        /// <param name="player">The player who will make the hit.</param>
        /// <returns>Player points after hit</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public int MakeHit(IPlayer player)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if (!IsGameStarted)
                throw new GameNotStartedException();

            if (!_players.ContainsKey(player))
                throw new ArgumentException("The player is not playing the game.", nameof(player));

            Card card = _dealer.CreateShuffledUserDeck(1).Cards[0];
            player.TakeCard(card);

            int playerPoints = CardPointsCalculator.CalculatePoints(player.Cards);

            if (playerPoints > WINNING_PLAYER_POINTS)
                RemoveLosingPlayer(player);

            return playerPoints;
        }

        public void StartDealerCardTaking()
        {
            if (!IsGameStarted)
                throw new GameNotStartedException();

            _dealer.TakeCards();

            ComparePoints();
            EndGame();
        }

        public virtual void StartGame()
        {
            if (IsGameStarted || _players.Count == 0)
                return;

            if (_players.Any(t => t.Value <= 0))
            {
                var playerWithoutBet = _players.First(t => t.Value <= 0).Key;
                throw new PlayerNotPlaceBetException(playerWithoutBet);
            }

            _dealer.HandOutCards(_players.Keys);
            IsGameStarted = true;
        }

        protected virtual void RemoveLosingPlayer(IPlayer player)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if (_players.TryGetValue(player, out int bet))
            {
                player.Money -= bet;
                _players.Remove(player);
            }
        }

        protected virtual void GiveWinningsToPlayer(IPlayer player)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if (_players.TryGetValue(player, out int bet))
                player.Money += bet * WINNING_RATIO;
        }

        private void EndGame()
        {
            _players.Clear();
            _dealer.FoldCards();

            IsGameStarted = false;
        }

        private void ComparePoints()
        {
            int dealerPoints = CardPointsCalculator.CalculatePoints(_dealer.Cards);

            foreach (var player in _players.Keys)
            {
                int playerPoints = CardPointsCalculator.CalculatePoints(player.Cards);

                if (playerPoints == dealerPoints)
                {
                    RemoveLosingPlayer(player);
                    continue;
                }

                if (dealerPoints > WINNING_PLAYER_POINTS)
                {
                    GiveWinningsToPlayer(player);
                    continue;
                }

                if (playerPoints < dealerPoints)
                    RemoveLosingPlayer(player);
                else
                    GiveWinningsToPlayer(player);
            }
        }
    }
}

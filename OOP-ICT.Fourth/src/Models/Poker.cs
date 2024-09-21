using OOP_ICT.Fourth.Infrastructure;
using OOP_ICT.Fourth.Infrastructure.Exceptions;
using OOP_ICT.Fourth.Interfaces;
using OOP_ICT.Interfaces;
using OOP_ICT.Models;
using OOP_ICT.Second.Infrastructure.Exceptions;
using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Fourth.Models
{
    public class Poker
    {
        public const int PLAYER_CARDS_COUNT = 2;
        public const int INITIAL_CARDS_ON_TABLE = 3;
        public const int MAX_CARDS_ON_TABLE = 5;

        private readonly List<IPlayer> _players;
        private readonly PokerDealer _dealer;
        private readonly PlayerMoneyFlow _moneyFlow;

        private readonly List<Card> _cards;
        private readonly Queue<IPlayer> _bettingQueue;

        public Poker(PokerBuilder builder)
        {
            _dealer = builder.PokerDealer ?? throw new DealerNotFoundException();
            _players = new List<IPlayer>(builder.Players);
            _moneyFlow = builder.PlayerMoneyFlow;

            _cards = new List<Card>();
            _bettingQueue = new Queue<IPlayer>();
        }

        public IReadOnlyList<Card> Cards => _cards;
        public IReadOnlyList<IPlayer> Players => _players;

        public int LastBet { get; private set; }
        public int BettingBank { get; private set; }
        public bool IsGameStarted { get; private set; }
        public bool IsBetsAccepted { get; private set; }

        public void StartGame()
        {
            if (IsGameStarted || _players.Count == 0)
                return;

            IsGameStarted = true;
            IsBetsAccepted = true;

            _dealer.InitializeCardDeck();
            _dealer.ShuffleCardDeck();

            DealCards();
        }

        public void DealCards()
        {
            if (_dealer.Cards.Count > 0)
                throw new CardsAlreadyDealtException();
            
            UserDeck dealerDeck = _dealer.CreateShuffledUserDeck(PLAYER_CARDS_COUNT);
            _dealer.TakeCards(dealerDeck);

            foreach (var player in _players)
            {
                UserDeck deck = _dealer.CreateShuffledUserDeck(PLAYER_CARDS_COUNT);
                player.TakeDeck(deck);
            }
        }

        public void TakeBet(IPlayer player, IBetStrategy strategy)
        {
            if (IsBetsAccepted)
                throw new BetsAlreadyAcceptedException();

            if (_bettingQueue.Count == 0)
                FillBettingQueue();

            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if (!player.Equals(_bettingQueue.Peek()))
                throw new AnotherPlayerTurnException();

            int bet = strategy.GetBet();

            if (strategy.IsPlayerRemainsInGame)
            {
                if (strategy.IsPlayerSkipMove)
                {
                    _bettingQueue.Enqueue(player);
                    _bettingQueue.Dequeue();
                    return;
                }

                if (bet <= 0)
                    throw new IncorrectBetException("Bet must be greater than zero.");

                if (bet < LastBet)
                    throw new IncorrectBetException("Bet can't be less than the previous player's bet.");

                WithdrawBetFromPlayer(player, bet);

                LastBet = bet;
                BettingBank += bet;
            }
            else
            {
                _players.Remove(player);

                if (_players.Count == 0)
                {
                    EndGame();
                    return;
                }
            }

            _bettingQueue.Dequeue();

            if (_bettingQueue.Count == 0)
            {
                TakeDealerBet();

                LastBet = 0;
                IsBetsAccepted = true;
            }
        }

        public IPlayer[] DetermineWinners()
        {
            if (!IsGameStarted)
                throw new GameNotStartedException();

            if (!IsBetsAccepted || _cards.Count != MAX_CARDS_ON_TABLE)
                throw new GameInProgressException("It is not yet possible to determine the winner.");

            var playersWithStrongestCombination = _players
                .ToDictionary(t => t, t => new CardCombinationDetector(t.Cards.Concat(Cards)).DetectMaxCombination())
                .GroupBy(t => t.Value.Combination)
                .OrderByDescending(t => t.Key)
                .First();

            CardCombinationDetector dealerCombinationDetector = new(_dealer.Cards.Concat(Cards));
            CardCombination dealerMaxCombination = dealerCombinationDetector.DetectMaxCombination();

            if (dealerMaxCombination.Combination > playersWithStrongestCombination.Key)
            {
                EndGame();
                return Array.Empty<IPlayer>();
            }

            int winnersCount = _players.Count;

            if (dealerMaxCombination.Combination == playersWithStrongestCombination.Key)
                winnersCount++;

            int winnings = BettingBank / winnersCount;

            foreach (var winnerInfo in playersWithStrongestCombination)
            {
                AddWinningsToPlayer(winnerInfo.Key, BettingBank);
            }

            EndGame();

            return playersWithStrongestCombination
                .Select(t => t.Key)
                .ToArray();
        }

        public void AddCardsToTable()
        {
            if (!IsBetsAccepted)
                throw new BetsNotAcceptedException();

            if (_cards.Count == MAX_CARDS_ON_TABLE)
                throw new CardsOnTableOverflowException();

            int cardsCount = _cards.Count == 0
                ? INITIAL_CARDS_ON_TABLE
                : 1;

            UserDeck deck = _dealer.CreateShuffledUserDeck(cardsCount);
            _cards.AddRange(deck.Cards);

            LastBet = 0;
            IsBetsAccepted = false;
        }

        private void AddWinningsToPlayer(IPlayer player, int sum)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            _moneyFlow.DepositMoney(player, sum);
        }

        private void WithdrawBetFromPlayer(IPlayer player, int sum)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            _moneyFlow.WithdrawMoney(player, sum);
        }

        private void TakeDealerBet()
        {
            if (IsBetsAccepted)
                throw new BetsAlreadyAcceptedException();

            // The dealer always makes 'Call' and has an infinite number of money.
            BettingBank += LastBet;
        }

        private void FillBettingQueue()
        {
            foreach (var player in _players)
            {
                _bettingQueue.Enqueue(player);
            }
        }

        private void EndGame()
        {
            _cards.Clear();
            _dealer.FoldCards();

            LastBet = 0;
            _bettingQueue.Clear();

            BettingBank = 0;
            IsGameStarted = false;
            IsBetsAccepted = false;
        }
    }
}

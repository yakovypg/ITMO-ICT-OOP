using OOP_ICT.Fourth.Infrastructure;
using OOP_ICT.Fourth.Infrastructure.Exceptions;
using OOP_ICT.Fourth.Models;
using OOP_ICT.Models;
using OOP_ICT.Second.Infrastructure;
using OOP_ICT.Second.Infrastructure.Exceptions;
using OOP_ICT.Second.Interfaces;
using OOP_ICT.Second.Models;
using OOP_ICT.Third.Infrastructure.Exceptions;

namespace OOP_ICT.Fourth.Tests
{
    public class Tests
    {
        [Fact]
        public void AddAccountsToPokerBank()
        {
            const int clientsNumber = 10;

            PokerBank bank = new();
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(clientsNumber);

            foreach (var player in clients)
                bank.AddAccount(player);

            Assert.Equal(clientsNumber, bank.Accounts.Count);
        }

        [Fact]
        public void AddAccountsToPokerBankWithRecurringClients()
        {
            const int clientsNumber = 10;

            PokerBank bank = new();

            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(clientsNumber);
            clients.AddRange(clients);

            Assert.Equal(clientsNumber * 2, clients.Count);

            foreach (var player in clients)
                bank.AddAccount(player);

            Assert.Equal(clientsNumber, bank.Accounts.Count);
        }

        [Fact]
        public void PokerBankAddWinnings_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new PokerBank(clients);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = 1000;
            int expectedMoney = sourceMoney + sum;

            bank.AddWinningsToPlayer(client, sum);

            Assert.Equal(expectedMoney, client.Money);
        }

        [Fact]
        public void PokerBankAddWinnings_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new PokerBank(clients);

            Player unknownClient = new Player("Unknown", 10000);

            int sourceMoney = unknownClient.Money;
            int sum = 1000;

            Assert.Throws<ClientNotFoundException>(() => bank.AddWinningsToPlayer(unknownClient, sum));
        }

        [Fact]
        public void PokerBankWindrawBet_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new PokerBank(clients);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = 100;
            int expectedMoney = sourceMoney - sum;

            bank.WithdrawBetFromPlayer(client, sum);

            Assert.Equal(expectedMoney, client.Money);
        }

        [Fact]
        public void PokerBankWindrawBet_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new PokerBank(clients);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = client.Money + 1;

            Assert.Throws<ArgumentException>(() => bank.WithdrawBetFromPlayer(client, sum));
        }

        [Fact]
        public void PokerBankCheckPlayerMoneyForBet_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new PokerBank(clients);

            Player client = clients[^1];
            int sum = client.Money - 1;

            Assert.True(bank.CheckIfPlayerHasEnoughMoneyForBet(client, sum));
        }

        [Fact]
        public void PokerBankCheckPlayerMoneyForBet_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new PokerBank(clients);

            Player client = clients[^1];
            int sum = client.Money + 1;

            Assert.False(bank.CheckIfPlayerHasEnoughMoneyForBet(client, sum));
        }

        [Fact]
        public void PlayerMoneyFlowDepositMoney_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            
            var bank = new PokerBank(clients);
            var moneyFlow = new PlayerMoneyFlow(bank);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = 1000;
            int expectedMoney = sourceMoney + sum;

            moneyFlow.DepositMoney(client, sum);

            Assert.Equal(expectedMoney, client.Money);
        }

        [Fact]
        public void PlayerMoneyFlowDepositMoney_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);

            var bank = new PokerBank(clients);
            var moneyFlow = new PlayerMoneyFlow(bank);

            Player unknownClient = new Player("Unknown", 10000);

            int sourceMoney = unknownClient.Money;
            int sum = 1000;

            Assert.Throws<ClientNotFoundException>(() => moneyFlow.DepositMoney(unknownClient, sum));
        }

        [Fact]
        public void PlayerMoneyFlowWindrawMoney_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            
            var bank = new PokerBank(clients);
            var moneyFlow = new PlayerMoneyFlow(bank);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = 100;
            int expectedMoney = sourceMoney - sum;

            moneyFlow.WithdrawMoney(client, sum);

            Assert.Equal(expectedMoney, client.Money);
        }

        [Fact]
        public void PlayerMoneyFlowWindrawMoney_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            
            var bank = new PokerBank(clients);
            var moneyFlow = new PlayerMoneyFlow(bank);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = client.Money + 1;

            Assert.Throws<ArgumentException>(() => moneyFlow.WithdrawMoney(client, sum));
        }

        [Fact]
        public void PokerDealerTakeCards_True()
        {
            var dealer = new PokerDealer();

            dealer.InitializeCardDeck();
            dealer.ShuffleCardDeck();

            var deck = dealer.CreateShuffledUserDeck(Poker.PLAYER_CARDS_COUNT);

            Assert.Equal(0, dealer.Cards.Count);
            dealer.TakeCards(deck);
            Assert.Equal(Poker.PLAYER_CARDS_COUNT, dealer.Cards.Count);
        }

        [Fact]
        public void PokerDealerTakeCards_False()
        {
            var dealer = new PokerDealer();

            dealer.InitializeCardDeck();
            dealer.ShuffleCardDeck();

            var deck1 = dealer.CreateShuffledUserDeck(Poker.PLAYER_CARDS_COUNT - 1);
            var deck2 = dealer.CreateShuffledUserDeck(Poker.PLAYER_CARDS_COUNT + 1);

            Assert.Throws<IncorrectCardsCountException>(() => dealer.TakeCards(deck1));
            Assert.Throws<IncorrectCardsCountException>(() => dealer.TakeCards(deck2));
        }

        [Fact]
        public void DetectRoyalFlush_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Clubs, new CardNominal("Queen", 12)),
                new Card(CardSuit.Clubs, new CardNominal("Six", 6)),
                new Card(CardSuit.Clubs, new CardNominal("Ten", 10)),
                new Card(CardSuit.Clubs, new CardNominal("Jack", 11)),
                new Card(CardSuit.Clubs, new CardNominal("Ace", 14)),
                new Card(CardSuit.Clubs, new CardNominal("King", 13)),
                new Card(CardSuit.Spades, new CardNominal("Queen", 12)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.RoyalFlush, combination.Combination);
        }

        [Fact]
        public void DetectStraightFlush_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Spades, new CardNominal("Five", 5)),
                new Card(CardSuit.Spades, new CardNominal("Six", 6)),
                new Card(CardSuit.Spades, new CardNominal("Jack", 11)),
                new Card(CardSuit.Clubs, new CardNominal("Eight", 8)),
                new Card(CardSuit.Spades, new CardNominal("Eight", 8)),
                new Card(CardSuit.Spades, new CardNominal("Seven", 7)),
                new Card(CardSuit.Spades, new CardNominal("Nive", 9)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.StraightFlush, combination.Combination);
        }

        [Fact]
        public void DetectFourOfKind_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Hearts, new CardNominal("Four", 4)),
                new Card(CardSuit.Diamonds, new CardNominal("Seven", 7)),
                new Card(CardSuit.Spades, new CardNominal("Four", 4)),
                new Card(CardSuit.Clubs, new CardNominal("Four", 4)),
                new Card(CardSuit.Hearts, new CardNominal("Seven", 7)),
                new Card(CardSuit.Spades, new CardNominal("Seven", 7)),
                new Card(CardSuit.Diamonds, new CardNominal("Four", 4)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.FourOfKind, combination.Combination);
        }

        [Fact]
        public void DetectFullHouse_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Hearts, new CardNominal("Four", 4)),
                new Card(CardSuit.Diamonds, new CardNominal("Seven", 7)),
                new Card(CardSuit.Spades, new CardNominal("Four", 4)),
                new Card(CardSuit.Clubs, new CardNominal("Four", 4)),
                new Card(CardSuit.Hearts, new CardNominal("Seven", 7)),
                new Card(CardSuit.Spades, new CardNominal("Seven", 7)),
                new Card(CardSuit.Diamonds, new CardNominal("Five", 5)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.FullHouse, combination.Combination);
        }

        [Fact]
        public void DetectFlush_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Hearts, new CardNominal("Three", 3)),
                new Card(CardSuit.Hearts, new CardNominal("Seven", 7)),
                new Card(CardSuit.Hearts, new CardNominal("Four", 4)),
                new Card(CardSuit.Hearts, new CardNominal("Jack", 11)),
                new Card(CardSuit.Spades, new CardNominal("Seven", 7)),
                new Card(CardSuit.Diamonds, new CardNominal("Five", 5)),
                new Card(CardSuit.Hearts, new CardNominal("Ace", 14)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.Flush, combination.Combination);
        }

        [Fact]
        public void DetectStraight_True()
        {
            var cardsWithStraight = new[]
            {
                new Card(CardSuit.Spades, new CardNominal("Three", 3)),
                new Card(CardSuit.Diamonds, new CardNominal("Seven", 7)),
                new Card(CardSuit.Spades, new CardNominal("Four", 4)),
                new Card(CardSuit.Clubs, new CardNominal("Jack", 11)),
                new Card(CardSuit.Spades, new CardNominal("Two", 2)),
                new Card(CardSuit.Diamonds, new CardNominal("Five", 5)),
                new Card(CardSuit.Hearts, new CardNominal("Six", 6)),
            };

            var cardsWithStraightByInverseAce = new[]
            {
                new Card(CardSuit.Spades, new CardNominal("Three", 3)),
                new Card(CardSuit.Diamonds, new CardNominal("Seven", 7)),
                new Card(CardSuit.Spades, new CardNominal("Four", 4)),
                new Card(CardSuit.Clubs, new CardNominal("Jack", 11)),
                new Card(CardSuit.Spades, new CardNominal("Two", 2)),
                new Card(CardSuit.Diamonds, new CardNominal("Five", 5)),
                new Card(CardSuit.Hearts, new CardNominal("Ace", 14)),
            };

            CardCombinationDetector detector1 = new(cardsWithStraight);
            CardCombination combination1 = detector1.DetectMaxCombination();

            Assert.Equal(CombinationType.Straight, combination1.Combination);

            CardCombinationDetector detector2 = new(cardsWithStraightByInverseAce);
            CardCombination combination2 = detector2.DetectMaxCombination();

            Assert.Equal(CombinationType.Straight, combination2.Combination);
        }

        [Fact]
        public void DetectThreeOfKind_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Hearts, new CardNominal("Three", 3)),
                new Card(CardSuit.Hearts, new CardNominal("Seven", 7)),
                new Card(CardSuit.Hearts, new CardNominal("Four", 4)),
                new Card(CardSuit.Diamonds, new CardNominal("Three", 3)),
                new Card(CardSuit.Spades, new CardNominal("Ace", 14)),
                new Card(CardSuit.Diamonds, new CardNominal("Jack", 11)),
                new Card(CardSuit.Spades, new CardNominal("Three", 3)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.ThreeOfKind, combination.Combination);
        }

        [Fact]
        public void DetectTwoPair_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Hearts, new CardNominal("Three", 3)),
                new Card(CardSuit.Hearts, new CardNominal("Seven", 7)),
                new Card(CardSuit.Hearts, new CardNominal("Four", 4)),
                new Card(CardSuit.Diamonds, new CardNominal("Three", 3)),
                new Card(CardSuit.Spades, new CardNominal("Ace", 14)),
                new Card(CardSuit.Diamonds, new CardNominal("Jack", 11)),
                new Card(CardSuit.Spades, new CardNominal("Seven", 7)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.TwoPair, combination.Combination);
        }

        [Fact]
        public void DetectPair_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Hearts, new CardNominal("Three", 3)),
                new Card(CardSuit.Hearts, new CardNominal("Seven", 7)),
                new Card(CardSuit.Hearts, new CardNominal("Four", 4)),
                new Card(CardSuit.Diamonds, new CardNominal("Three", 3)),
                new Card(CardSuit.Spades, new CardNominal("Ace", 14)),
                new Card(CardSuit.Diamonds, new CardNominal("Jack", 11)),
                new Card(CardSuit.Spades, new CardNominal("King", 13)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.Pair, combination.Combination);
        }

        [Fact]
        public void DetectHighCard_True()
        {
            var cards = new[]
            {
                new Card(CardSuit.Clubs, new CardNominal("Two", 2)),
                new Card(CardSuit.Hearts, new CardNominal("Seven", 7)),
                new Card(CardSuit.Hearts, new CardNominal("Four", 4)),
                new Card(CardSuit.Diamonds, new CardNominal("Three", 3)),
                new Card(CardSuit.Spades, new CardNominal("Ace", 14)),
                new Card(CardSuit.Diamonds, new CardNominal("Jack", 11)),
                new Card(CardSuit.Spades, new CardNominal("King", 13)),
            };

            CardCombinationDetector detector = new(cards);
            CardCombination combination = detector.DetectMaxCombination();

            Assert.Equal(CombinationType.HighCard, combination.Combination);
        }

        [Fact]
        public void PokerStartGame_True()
        {
            var builder = new PokerBuilder();
            var dealer = new PokerDealer();

            const int playersCount = 5;
            List<Player> players = new PremiumBlackjackRoomFactory().CreatePlayers(playersCount);

            foreach (var player in players)
            {
                builder.AddPlayer(player);
            }

            Poker game = builder.AddPokerDealer(dealer).Build();

            Assert.False(game.IsGameStarted);
            game.StartGame();
            Assert.True(game.IsGameStarted);

            Assert.Equal(0, game.Cards.Count);
            Assert.Equal(0, game.BettingBank);
            Assert.Equal(playersCount, game.Players.Count);

            foreach (var player in game.Players)
            {
                Assert.Equal(Poker.PLAYER_CARDS_COUNT, player.Cards.Count);
            }
        }

        [Fact]
        public void PokerAddCardsToTable_True()
        {
            var builder = new PokerBuilder();
            var dealer = new PokerDealer();

            const int playersCount = 5;
            List<Player> players = new PremiumBlackjackRoomFactory().CreatePlayers(playersCount);

            foreach (var player in players)
            {
                builder.AddPlayer(player);
            }

            Poker game = builder.AddPokerDealer(dealer).Build();
            game.StartGame();

            Assert.Equal(0, game.Cards.Count);
            game.AddCardsToTable();
            Assert.Equal(Poker.INITIAL_CARDS_ON_TABLE, game.Cards.Count);

            foreach (var player in game.Players)
            {
                game.TakeBet(player, new CallBetStrategy(100));
            }

            game.AddCardsToTable();
            Assert.Equal(Poker.INITIAL_CARDS_ON_TABLE + 1, game.Cards.Count);

            foreach (var player in game.Players)
            {
                game.TakeBet(player, new CallBetStrategy(100));
            }

            game.AddCardsToTable();
            Assert.Equal(Poker.INITIAL_CARDS_ON_TABLE + 2, game.Cards.Count);

            foreach (var player in game.Players)
            {
                game.TakeBet(player, new CallBetStrategy(100));
            }

            Assert.Throws<CardsOnTableOverflowException>(game.AddCardsToTable);
        }

        [Fact]
        public void PokerTakeBet_True()
        {
            var builder = new PokerBuilder();
            var dealer = new PokerDealer();

            const int playersCount = 2;
            List<Player> players = new PremiumBlackjackRoomFactory().CreatePlayers(playersCount);

            foreach (var player in players)
            {
                builder.AddPlayer(player);
            }

            Poker game = builder.AddPokerDealer(dealer).Build();
            game.StartGame();
            game.AddCardsToTable();

            int player0SourceMoney = players[0].Money;
            int player1SourceMoney = players[1].Money;

            const int player0Bet1 = 1000;
            const int player1Bet1 = 2000;

            int player0MoneyAfterBet1 = player0SourceMoney - player0Bet1;
            int player1MoneyAfterBet1 = player1SourceMoney - player1Bet1;

            game.TakeBet(players[0], new RaiseBetStrategy(player0Bet1));

            Assert.Throws<AnotherPlayerTurnException>(() => game.TakeBet(players[0], new RaiseBetStrategy(player0Bet1)));
            Assert.Throws<IncorrectBetException>(() => game.TakeBet(players[1], new RaiseBetStrategy(player0Bet1 - 1)));
            
            game.TakeBet(players[1], new RaiseBetStrategy(player1Bet1));

            Assert.Equal(player0MoneyAfterBet1, players[0].Money);
            Assert.Equal(player1MoneyAfterBet1, players[1].Money);

            game.AddCardsToTable();

            const int player0Bet2 = 3000;
            const int player1Bet2 = 3000;

            int player0MoneyAfterBet2 = player0MoneyAfterBet1 - player0Bet2;
            int player1MoneyAfterBet2 = player1MoneyAfterBet1 - player1Bet2;

            game.TakeBet(players[0], new RaiseBetStrategy(player0Bet2));
            game.TakeBet(players[1], new CallBetStrategy(player1Bet2));

            Assert.Equal(player0MoneyAfterBet2, players[0].Money);
            Assert.Equal(player1MoneyAfterBet2, players[1].Money);

            const int player0Bet3 = 3000;
            const int player1Bet3 = 3000;

            int player0MoneyAfterBet3 = player0MoneyAfterBet2 - player0Bet3;
            int player1MoneyAfterBet3 = player1MoneyAfterBet2 - player1Bet3;

            game.AddCardsToTable();

            game.TakeBet(players[0], new CheckBetStrategy());
            Assert.Throws<AnotherPlayerTurnException>(() => game.TakeBet(players[0], new RaiseBetStrategy(player0Bet1)));
            
            game.TakeBet(players[1], new RaiseBetStrategy(player1Bet3));
            Assert.Equal(player0MoneyAfterBet2, players[0].Money);

            game.TakeBet(players[0], new CallBetStrategy(player0Bet3));

            Assert.Equal(player0MoneyAfterBet3, players[0].Money);
            Assert.Equal(player1MoneyAfterBet3, players[1].Money);
        }

        [Fact]
        public void TestExampleGame()
        {
            var builder = new PokerBuilder();
            var dealer = new PokerDealer();

            const int playersCount = 3;
            List<Player> players = new PremiumBlackjackRoomFactory().CreatePlayers(playersCount);

            foreach (var player in players)
            {
                builder.AddPlayer(player);
            }

            Poker game = builder.AddPokerDealer(dealer).Build();

            Assert.Equal(playersCount, game.Players.Count);
            Assert.Throws<GameNotStartedException>(game.DetermineWinners);

            Assert.False(game.IsGameStarted);
            game.StartGame();
            Assert.True(game.IsGameStarted);

            Assert.Equal(0, game.Cards.Count);
            game.AddCardsToTable();
            Assert.Equal(Poker.INITIAL_CARDS_ON_TABLE, game.Cards.Count);

            Assert.Throws<BetsNotAcceptedException>(game.AddCardsToTable);
            Assert.Throws<GameInProgressException>(game.DetermineWinners);

            int player0SourceMoney = players[0].Money;
            int player1SourceMoney = players[1].Money;
            int player2SourceMoney = players[2].Money;

            const int player0Bet1 = 100;
            const int player1Bet1 = 200;
            const int player2Bet1 = 200;

            int player0MoneyAfterBet1 = player0SourceMoney - player0Bet1;
            int player1MoneyAfterBet1 = player1SourceMoney - player1Bet1;
            int player2MoneyAfterBet1 = player2SourceMoney - player2Bet1;

            game.TakeBet(players[0], new RaiseBetStrategy(player0Bet1));
            Assert.Throws<AnotherPlayerTurnException>(() => game.TakeBet(players[2], new RaiseBetStrategy(player2Bet1)));
            
            game.TakeBet(players[1], new RaiseBetStrategy(player1Bet1));
            Assert.Throws<IncorrectBetException>(() => game.TakeBet(players[2], new RaiseBetStrategy(1)));
            
            game.TakeBet(players[2], new CallBetStrategy(player2Bet1));
            Assert.Throws<BetsAlreadyAcceptedException>(() => game.TakeBet(players[2], new RaiseBetStrategy(player2Bet1)));

            Assert.Equal(player0MoneyAfterBet1, players[0].Money);
            Assert.Equal(player1MoneyAfterBet1, players[1].Money);
            Assert.Equal(player2MoneyAfterBet1, players[2].Money);

            int cardsCount1 = game.Cards.Count;
            game.AddCardsToTable();
            Assert.Equal(cardsCount1 + 1, game.Cards.Count);

            const int player0Bet2 = 500;
            const int player2Bet2 = 1000;

            int player0MoneyAfterBet2 = player0MoneyAfterBet1 - player0Bet2;
            int player1MoneyAfterBet2 = player1MoneyAfterBet1;
            int player2MoneyAfterBet2 = player2MoneyAfterBet1 - player2Bet2;

            game.TakeBet(players[0], new RaiseBetStrategy(player0Bet2));
            game.TakeBet(players[1], new FoldBetStrategy());
            game.TakeBet(players[2], new RaiseBetStrategy(player2Bet2));

            Assert.Equal(playersCount - 1, game.Players.Count);

            Assert.Equal(player0MoneyAfterBet2, players[0].Money);
            Assert.Equal(player1MoneyAfterBet2, players[1].Money);
            Assert.Equal(player2MoneyAfterBet2, players[2].Money);

            int cardsCount2 = game.Cards.Count;
            game.AddCardsToTable();
            Assert.Equal(cardsCount2 + 1, game.Cards.Count);

            const int player0Bet3 = 1000;
            const int player2Bet3 = 1000;

            int player0MoneyAfterBet3 = player0MoneyAfterBet2 - player0Bet3;
            int player1MoneyAfterBet3 = player1MoneyAfterBet2;
            int player2MoneyAfterBet3 = player2MoneyAfterBet2 - player2Bet3;

            game.TakeBet(players[0], new CheckBetStrategy());
            game.TakeBet(players[2], new RaiseBetStrategy(player2Bet3));

            Assert.Equal(player0MoneyAfterBet2, players[0].Money);

            Assert.Throws<BetsNotAcceptedException>(game.AddCardsToTable);
            game.TakeBet(players[0], new CallBetStrategy(player0Bet3));
            Assert.Throws<CardsOnTableOverflowException>(game.AddCardsToTable);

            Assert.Equal(player0MoneyAfterBet3, players[0].Money);
            Assert.Equal(player1MoneyAfterBet3, players[1].Money);
            Assert.Equal(player2MoneyAfterBet3, players[2].Money);

            IPlayer[] winners = game.DetermineWinners();

            Assert.Single(winners);

            IPlayer winner = winners[0];
            Assert.Equal(players[2], winner);

            int dealerBet1 = new[] { player0Bet1, player1Bet1, player2Bet1 }.Max();
            int dealerBet2 = new[] { player0Bet2, player2Bet2 }.Max();
            int dealerBet3 = new[] { player0Bet3, player2Bet3 }.Max();

            int bettingBank = player0Bet1 + player1Bet1 + player2Bet1 + dealerBet1 +
                player0Bet2 + player2Bet2 + dealerBet2 +
                player0Bet3 + player2Bet3 + dealerBet3;

            int winnerExpectedMoney = player2SourceMoney + bettingBank -
                player2Bet1 - player2Bet2 - player2Bet3;

            Assert.Equal(winnerExpectedMoney, winner.Money);
        }
    }
}
using OOP_ICT.Models;
using OOP_ICT.Second.Infrastructure;
using OOP_ICT.Second.Interfaces;
using OOP_ICT.Second.Models;
using OOP_ICT.Third.Infrastructure;
using OOP_ICT.Third.Infrastructure.Exceptions;
using OOP_ICT.Third.Models;

namespace OOP_ICT.Third.Tests
{
    public class Tests
    {
        [Fact]
        public void AddAccountsToBank()
        {
            const int clientsNumber = 10;

            Bank bank = new();
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(clientsNumber);

            foreach (var player in clients)
                bank.AddAccount(player);

            Assert.Equal(clientsNumber, bank.Accounts.Count);
        }

        [Fact]
        public void AddAccountsToBankWithRecurringClients()
        {
            const int clientsNumber = 10;

            Bank bank = new();

            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(clientsNumber);
            clients.AddRange(clients);

            Assert.Equal(clientsNumber * 2, clients.Count);

            foreach (var player in clients)
                bank.AddAccount(player);

            Assert.Equal(clientsNumber, bank.Accounts.Count);
        }

        [Fact]
        public void BankAdapterDepositMoney_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bankAdapter = new BankAdapter(clients);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = 1000;
            int expectedMoney = sourceMoney + sum;

            bankAdapter.DepositMoney(client, sum);

            Assert.Equal(expectedMoney, client.Money);
        }

        [Fact]
        public void BankAdapterDepositMoney_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bankAdapter = new BankAdapter(clients);

            Player unknownClient = new Player("Unknown", 10000);

            int sourceMoney = unknownClient.Money;
            int sum = 1000;

            Assert.Throws<ClientNotFoundException>(() => bankAdapter.DepositMoney(unknownClient, sum));
        }

        [Fact]
        public void BankAdapterWindrawMoney_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bankAdapter = new BankAdapter(clients);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = 100;
            int expectedMoney = sourceMoney - sum;

            bankAdapter.WithdrawMoney(client, sum);

            Assert.Equal(expectedMoney, client.Money);
        }

        [Fact]
        public void BankAdapterWindrawMoney_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bankAdapter = new BankAdapter(clients);

            Player client = clients[^1];

            int sourceMoney = client.Money;
            int sum = client.Money + 1;

            Assert.Throws<ArgumentException>(() => bankAdapter.WithdrawMoney(client, sum));
        }

        [Fact]
        public void BankAddMoneyToAccount_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new Bank(clients);

            Player client = clients[^1];
            BankAccount account = bank.Accounts.First(t => t.Client == client);

            int sourceMoney = client.Money;
            int sum = 1000;
            int expectedMoney = sourceMoney + sum;

            bank.AddMoneyToAccount(account.Id, sum);

            Assert.Equal(expectedMoney, client.Money);
        }

        [Fact]
        public void BankAddMoneyToAccount_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new Bank(clients);

            IEnumerable<int> ids = bank.Accounts.Select(t => t.Id);
            int incorrectId = Enumerable.Range(1, clients.Count + 1).Except(ids).First();

            Assert.Throws<KeyNotFoundException>(() => bank.AddMoneyToAccount(incorrectId, 10));
        }

        [Fact]
        public void BankWindrawMoneyFromAccount_True()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new Bank(clients);

            Player client = clients[^1];
            BankAccount account = bank.Accounts.First(t => t.Client == client);

            int sourceMoney = client.Money;
            int sum = 1000;
            int expectedMoney = sourceMoney - sum;

            bank.WithdrawMoneyFromAccount(account.Id, sum);

            Assert.Equal(expectedMoney, client.Money);
        }

        [Fact]
        public void BankWindrawMoneyFromAccount_False()
        {
            List<Player> clients = new PremiumBlackjackRoomFactory().CreatePlayers(5);
            var bank = new Bank(clients);

            Player client = clients[^1];
            BankAccount account = bank.Accounts.First(t => t.Client == client);

            int sourceMoney = client.Money;
            int sum = client.Money + 1;

            Assert.Throws<ArgumentException>(() => bank.WithdrawMoneyFromAccount(account.Id, sum));
        }

        [Fact]
        public void BlackjackCasinoGiveOutWinnings_True()
        {
            List<Player> players = new PremiumBlackjackRoomFactory().CreatePlayers(5);

            Dictionary<IPlayer, int> playersDict = players
                .Select(t => t as IPlayer)
                .ToDictionary(t => t, t => 1000); 

            var bankAdapter = new BankAdapter(players);
            var casino = new BlackjackCasino(playersDict, bankAdapter);

            IPlayer player = playersDict.Last().Key;
            int bet = playersDict.Last().Value;

            int sourceMoney = player.Money;
            int expectedMoney = sourceMoney + bet * Blackjack.WINNING_RATIO;

            casino.GiveOutWinnings(player);

            Assert.Equal(expectedMoney, player.Money);
        }

        [Fact]
        public void BlackjackCasinoPickUpLoss_True()
        {
            List<Player> players = new PremiumBlackjackRoomFactory().CreatePlayers(5);

            Dictionary<IPlayer, int> playersDict = players
                .Select(t => t as IPlayer)
                .ToDictionary(t => t, t => 100);

            var bankAdapter = new BankAdapter(players);
            var casino = new BlackjackCasino(playersDict, bankAdapter);

            IPlayer player = playersDict.Last().Key;
            int bet = playersDict.Last().Value;

            int sourceMoney = player.Money;
            int expectedMoney = sourceMoney - bet;

            casino.PickUpLoss(player);

            Assert.Equal(expectedMoney, player.Money);
        }

        [Fact]
        public void BlackjackCasinoCheckBlackjack()
        {
            List<Player> players = new PremiumBlackjackRoomFactory().CreatePlayers(5);

            var cardKingHearts = new Card(CardSuit.Hearts, new CardNominal("King", int.MaxValue));
            var cardKingSpades = new Card(CardSuit.Spades, new CardNominal("King", int.MaxValue));
            var cardQueenHearts = new Card(CardSuit.Hearts, new CardNominal("Queen", int.MaxValue - 1));
            var cardSevenHearts = new Card(CardSuit.Hearts, new CardNominal("Seven", int.MaxValue - 2));
            var cardSevenDiamonds = new Card(CardSuit.Diamonds, new CardNominal("Seven", int.MaxValue - 2));
            var cardSixHearts = new Card(CardSuit.Hearts, new CardNominal("Six", int.MaxValue - 3));
            var cardSixSpades = new Card(CardSuit.Spades, new CardNominal("Six", int.MaxValue - 3));
            var cardSixDiamonds = new Card(CardSuit.Diamonds, new CardNominal("Six", int.MaxValue - 3));
            var cardFiveHearts = new Card(CardSuit.Hearts, new CardNominal("Five", int.MaxValue - 4));
            var cardFiveDiamonds = new Card(CardSuit.Diamonds, new CardNominal("Five", int.MaxValue - 4));
            var cardFiveSpades = new Card(CardSuit.Spades, new CardNominal("Five", int.MaxValue - 4));

            Player player0 = players[0];
            player0.TakeCard(cardKingHearts);
            player0.TakeCard(cardSixHearts);
            player0.TakeCard(cardFiveDiamonds);

            Player player1 = players[1];
            player1.TakeCard(cardFiveHearts);
            player1.TakeCard(cardSixSpades);
            player1.TakeCard(cardQueenHearts);

            Player player2 = players[2];
            player2.TakeCard(cardFiveSpades);

            Player player3 = players[3];
            player3.TakeCard(cardKingSpades);

            Player player4 = players[4];
            player4.TakeCard(cardSevenHearts);
            player4.TakeCard(cardSevenDiamonds);
            player4.TakeCard(cardSixDiamonds);

            const int bet = 100;

            Dictionary<IPlayer, int> playersDict = players
                .Select(t => t as IPlayer)
                .ToDictionary(t => t, t => bet);

            int expectedPlayer0Money = player0.Money + bet * Blackjack.WINNING_RATIO;
            int expectedPlayer1Money = player1.Money + bet * Blackjack.WINNING_RATIO;

            var bankAdapter = new BankAdapter(players);
            var casino = new BlackjackCasino(playersDict, bankAdapter);

            var remainingPlayers = players.Where(t => t != player0 && t != player1).ToList();

            Assert.Equal(casino.Players.Count, players.Count);
            casino.CheckBlackjack();
            Assert.Equal(casino.Players.Count, players.Count - 2);

            foreach (var player in remainingPlayers)
                Assert.True(casino.Players.ContainsKey(player));

            Assert.False(casino.Players.ContainsKey(player0));
            Assert.False(casino.Players.ContainsKey(player1));

            Assert.Equal(expectedPlayer0Money, player0.Money);
            Assert.Equal(expectedPlayer1Money, player1.Money);
        }

        [Fact]
        public void TestExampleGame()
        {
            var roomFactory = new LowGradeBlackjackRoomFactory();

            List<Player> players = roomFactory.CreatePlayers(5);
            BlackjackDealer dealer = roomFactory.CreateDealer();

            var builder = new BlackjackGameBuilder();
            players.ForEach(t => builder.AddPlayer(t));

            const int player0Bet = 100;
            const int player1Bet = 200;
            const int player2Bet = 300;
            const int player3Bet = 400;
            const int player4Bet = 500;

            int player0InitialMoney = players[0].Money;
            int player1InitialMoney = players[1].Money;
            int player2InitialMoney = players[2].Money;
            int player3InitialMoney = players[3].Money;
            int player4InitialMoney = players[4].Money;

            BlackjackGame game = builder
                .AddBet(players[0], player0Bet)
                .AddBet(players[1], player1Bet)
                .AddBet(players[2], player2Bet)
                .AddBet(players[3], player3Bet)
                .AddBet(players[4], player4Bet)
                .AddBlackjackDealer(dealer)
                .Build();

            game.StartGame();
            Assert.True(game.Players.Count == 5);

            game.MakeHit(players[0]);
            game.MakeHit(players[1]);

            Assert.True(game.Players.Count == 3);

            Assert.Throws<ArgumentException>(() => game.MakeHit(players[0]));
            Assert.Throws<ArgumentException>(() => game.MakeHit(players[1]));

            game.StartDealerCardTaking();

            int expectedPlayer0Money = player0InitialMoney - player0Bet;
            int expectedPlayer1Money = player1InitialMoney - player1Bet;
            int expectedPlayer2Money = player2InitialMoney + player2Bet * Blackjack.WINNING_RATIO;
            int expectedPlayer3Money = player3InitialMoney + player3Bet * Blackjack.WINNING_RATIO;
            int expectedPlayer4Money = player4InitialMoney - player4Bet; // Draw => lose money

            Assert.Equal(expectedPlayer0Money, players[0].Money);
            Assert.Equal(expectedPlayer1Money, players[1].Money);
            Assert.Equal(expectedPlayer2Money, players[2].Money);
            Assert.Equal(expectedPlayer3Money, players[3].Money);
            Assert.Equal(expectedPlayer4Money, players[4].Money);
        }
    }
}
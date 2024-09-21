using OOP_ICT.Second.Infrastructure;
using OOP_ICT.Second.Models;

namespace OOP_ICT.Second.Tests
{
    public class Tests
    {
        [Fact]
        public void PlayerLosesDueToOverkillOfPoints_True()
        {
            var roomFactory = new PremiumBlackjackRoomFactory();

            List<Player> players = roomFactory.CreatePlayers(1);
            BlackjackDealer dealer = roomFactory.CreateDealer();

            var builder = new BlackjackBuilder();
            players.ForEach(t => builder.AddPlayer(t));

            const int bet = 5000;

            Blackjack game = builder
                .AddBet(players[0], bet)
                .AddBlackjackDealer(dealer)
                .Build();

            Assert.True(game.Players.Count == 1);

            game.StartGame();

            while (true)
            {
                if (game.MakeHit(players[0]) > Blackjack.WINNING_PLAYER_POINTS)
                    break;
            }

            Assert.True(game.Players.Count == 0);
        }

        [Fact]
        public void PlayerMoneyDeacrease_True()
        {
            var roomFactory = new PremiumBlackjackRoomFactory();

            List<Player> players = roomFactory.CreatePlayers(1);
            BlackjackDealer dealer = roomFactory.CreateDealer();

            var builder = new BlackjackBuilder();
            players.ForEach(t => builder.AddPlayer(t));

            const int bet = 5000;
            int initialMoney = players[0].Money;

            Blackjack game = builder
                .AddBet(players[0], bet)
                .AddBlackjackDealer(dealer)
                .Build();

            game.StartGame();

            while (true)
            {
                if (game.MakeHit(players[0]) > Blackjack.WINNING_PLAYER_POINTS)
                    break;
            }

            int expectedMoney = initialMoney - bet;
            Assert.Equal(expectedMoney, players[0].Money);
        }

        [Fact]
        public void PlayerMoneyIncrease_True()
        {
            var roomFactory = new PremiumBlackjackRoomFactory();

            List<Player> players = roomFactory.CreatePlayers(1);
            BlackjackDealer dealer = roomFactory.CreateDealer();

            var builder = new BlackjackBuilder();
            players.ForEach(t => builder.AddPlayer(t));

            const int bet = 5000;
            int initialMoney = players[0].Money;

            Blackjack game = builder
                .AddBet(players[0], bet)
                .AddBlackjackDealer(dealer)
                .Build();

            Assert.True(game.Players.Count == 1);

            game.StartGame();
            game.MakeHit(players[0]);

            Assert.True(game.Players.Count == 1);

            game.StartDealerCardTaking();

            int expectedMoney = initialMoney + bet * Blackjack.WINNING_RATIO;
            Assert.Equal(expectedMoney, players[0].Money);
        }

        [Fact]
        public void TestExampleGame_1()
        {
            var roomFactory = new PremiumBlackjackRoomFactory();

            List<Player> players = roomFactory.CreatePlayers(3);
            BlackjackDealer dealer = roomFactory.CreateDealer();

            var builder = new BlackjackBuilder();
            players.ForEach(t => builder.AddPlayer(t));

            const int player0Bet = 5000;
            const int player1Bet = 9000;
            const int player2Bet = 900000;

            int player0InitialMoney = players[0].Money;
            int player1InitialMoney = players[1].Money;
            int player2InitialMoney = players[2].Money;

            Blackjack game = builder
                .AddBet(players[0], player0Bet)
                .AddBet(players[1], player1Bet)
                .AddBet(players[2], player2Bet)
                .AddBlackjackDealer(dealer)
                .Build();

            game.StartGame();
            Assert.True(game.Players.Count == 3);

            game.MakeHit(players[0]);
            Assert.True(game.Players.Count == 3);

            game.MakeHit(players[1]);
            Assert.True(game.Players.Count == 2);

            while (true)
            {
                if (game.MakeHit(players[2]) > Blackjack.WINNING_PLAYER_POINTS)
                    break;
            }

            Assert.True(game.Players.Count == 1);
            game.StartDealerCardTaking();

            int expectedPlayer0Money = player0InitialMoney + player0Bet * Blackjack.WINNING_RATIO;
            int expectedPlayer1Money = player1InitialMoney - player1Bet;
            int expectedPlayer2Money = player2InitialMoney - player2Bet;

            Assert.Equal(expectedPlayer0Money, players[0].Money);
            Assert.Equal(expectedPlayer1Money, players[1].Money);
            Assert.Equal(expectedPlayer2Money, players[2].Money);
        }

        [Fact]
        public void TestExampleGame_2()
        {
            var roomFactory = new LowGradeBlackjackRoomFactory();

            List<Player> players = roomFactory.CreatePlayers(5);
            BlackjackDealer dealer = roomFactory.CreateDealer();

            var builder = new BlackjackBuilder();
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

            Blackjack game = builder
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
using OOP_ICT.Second.Interfaces;
using OOP_ICT.Third.Infrastructure.Exceptions;
using OOP_ICT.Third.Models;

namespace OOP_ICT.Fourth.Models
{
    public class PokerBank : Bank
    {
        public PokerBank(IEnumerable<IPlayer> clients = null) : base(clients)
        {
        }

        public void AddWinningsToPlayer(IPlayer player, int sum)
        {
            BankAccount account = FindClientAccount(player);
            AddMoneyToAccount(account.Id, sum);
        }

        public void WithdrawBetFromPlayer(IPlayer player, int sum)
        {
            if (!CheckIfPlayerHasEnoughMoneyForBet(player, sum))
                throw new ArgumentException("There are not enough money in the account.", nameof(sum));

            BankAccount account = FindClientAccount(player);
            WithdrawMoneyFromAccount(account.Id, sum);
        }

        public bool CheckIfPlayerHasEnoughMoneyForBet(IPlayer player, int sum)
        {
            BankAccount account = FindClientAccount(player);
            return CheckIfClientHasEnoughFunds(account, sum);
        }

        private BankAccount FindClientAccount(IPlayer client)
        {
            BankAccount account = _accounts.FirstOrDefault(x => x.Client.Equals(client));

            return account is null
                ? throw new ClientNotFoundException("Client does not have a bank account.")
                : account;
        }
    }
}

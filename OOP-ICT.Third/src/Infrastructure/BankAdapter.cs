using OOP_ICT.Second.Interfaces;
using OOP_ICT.Third.Infrastructure.Exceptions;
using OOP_ICT.Third.Models;

namespace OOP_ICT.Third.Infrastructure
{
    public class BankAdapter : Bank, IPlayerMoneyFlow
    {
        public BankAdapter(IEnumerable<IPlayer> clients = null) : base(clients)
        {
        }

        public void DepositMoney(IPlayer player, int sum)
        {
            BankAccount account = FindClientAccount(player);
            AddMoneyToAccount(account.Id, sum);
        }

        public void WithdrawMoney(IPlayer player, int sum)
        {
            BankAccount account = FindClientAccount(player);
            WithdrawMoneyFromAccount(account.Id, sum);
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

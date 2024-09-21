namespace OOP_ICT.Second.Interfaces
{
    public interface IPlayerMoneyFlow
    {
        public void DepositMoney(IPlayer player, int sum);
        public void WithdrawMoney(IPlayer player, int sum);
    }
}

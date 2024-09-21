namespace OOP_ICT.Second.Infrastructure.Exceptions
{
    public class DealerNotFoundException : ApplicationException
    {
        public DealerNotFoundException(string message = null, Exception innerException = null) :
            base(message ?? "Dealer not found.", innerException)
        {
        }
    }
}

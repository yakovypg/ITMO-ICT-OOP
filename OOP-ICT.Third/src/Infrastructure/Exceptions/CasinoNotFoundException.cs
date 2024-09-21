namespace OOP_ICT.Third.Infrastructure.Exceptions
{
    public class CasinoNotFoundException : ApplicationException
    {
        public CasinoNotFoundException(string message = null, Exception innerException = null) :
            base(message ?? "Casino not found.", innerException)
        {
        }
    }
}

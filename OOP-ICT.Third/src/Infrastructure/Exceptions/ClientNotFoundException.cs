namespace OOP_ICT.Third.Infrastructure.Exceptions
{
    public class ClientNotFoundException : ApplicationException
    {
        public ClientNotFoundException(string message = null, Exception innerException = null) :
            base(message ?? "Client not found.", innerException)
        {
        }
    }
}

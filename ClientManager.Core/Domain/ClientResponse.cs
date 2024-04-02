namespace ClientManager.Core.Domain
{
    public class ClientResponse : Client
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
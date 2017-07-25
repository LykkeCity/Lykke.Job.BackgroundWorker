namespace Lykke.Job.BackgroundWorker.Contract.Contexts
{
    public class SetGeolocationContext
    {
        public SetGeolocationContext(string clientId, string ip)
        {
            ClientId = clientId;
            Ip = ip;
        }

        public string ClientId { get; set; }
        public string Ip { get; set; }
    }
}
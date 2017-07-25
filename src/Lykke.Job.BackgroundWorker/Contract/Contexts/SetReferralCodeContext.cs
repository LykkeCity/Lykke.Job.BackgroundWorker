namespace Lykke.Job.BackgroundWorker.Contract.Contexts
{
    public class SetReferralCodeContext
    {
        public SetReferralCodeContext(string clientId, string ip)
        {
            ClientId = clientId;
            Ip = ip;
        }

        public string ClientId { get; set; }
        public string Ip { get; set; }
    }
}
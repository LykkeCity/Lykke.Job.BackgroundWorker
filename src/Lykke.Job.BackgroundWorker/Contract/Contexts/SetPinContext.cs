namespace Lykke.Job.BackgroundWorker.Contract.Contexts
{
    public class SetPinContext
    {
        public SetPinContext(string clientId, string pin)
        {
            ClientId = clientId;
            Pin = pin;
        }

        public string ClientId { get; set; }
        public string Pin { get; set; }
    }
}
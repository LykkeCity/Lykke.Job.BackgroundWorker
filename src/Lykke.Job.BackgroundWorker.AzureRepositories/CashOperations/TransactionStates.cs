namespace Lykke.Job.BackgroundWorker.AzureRepositories.CashOperations
{
    public enum TransactionStates
    {
        InProcessOnchain,
        SettledOnchain,
        InProcessOffchain,
        SettledOffchain,
        SettledNoChain
    }
}
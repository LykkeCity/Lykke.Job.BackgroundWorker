using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Lykke.Job.BackgroundWorker.Contract.Contexts;
using Lykke.Job.BackgroundWorker.Core.Domain.BitCoin;
using Lykke.Service.OperationsRepository.Client.Abstractions.CashOperations;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class UpdateHashForOperationsWorker : IWorker
    {
        private readonly ITradeOperationsRepositoryClient _clientTradesRepositoryClient;
        private readonly ITransferOperationsRepositoryClient _transferEventsRepositoryClient;
        private readonly ICashOperationsRepositoryClient _cashOperationsRepositoryClient;

        private UpdateHashForOperationsContext _context;
        private readonly Dictionary<string, Func<string, string, Task>> _handlers =
            new Dictionary<string, Func<string, string, Task>>();

        public UpdateHashForOperationsWorker(ITradeOperationsRepositoryClient clientTradesRepositoryClient,
            ITransferOperationsRepositoryClient transferEventsRepositoryClient, ICashOperationsRepositoryClient cashOperationsRepositoryClient)
        {
            _clientTradesRepositoryClient = clientTradesRepositoryClient;
            _transferEventsRepositoryClient = transferEventsRepositoryClient;
            _cashOperationsRepositoryClient = cashOperationsRepositoryClient;

            RegisterHandler(BitCoinCommands.Issue, HandleIssueAsync);
            RegisterHandler(BitCoinCommands.CashOut, HandleCashOutAsync);
            RegisterHandler(BitCoinCommands.Swap, HandleSwapAsync);
            RegisterHandler(BitCoinCommands.Transfer, HandleTransferAsync);
            RegisterHandler(BitCoinCommands.TransferAll, HandleTransferAsync);
            RegisterHandler(BitCoinCommands.Destroy, HandleDestroyAsync);
        }

        public async Task DoWork()
        {
            if (_context == null)
                throw new Exception("context was not set");

            var cmdType = _context.CmdType;

            var handler = GetHandler(cmdType);

            if (handler != null)
            {
                await handler(_context.ContextData, _context.Hash);
            }
        }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<UpdateHashForOperationsContext>();
            if (_context == null)
                throw new ArgumentException(nameof(contextJson));
        }

        private async Task HandleDestroyAsync(string operationContext, string hash)
        {
            var contextData = operationContext.DeserializeJson<UncolorContextData>();

            await _cashOperationsRepositoryClient.UpdateBlockchainHashAsync(contextData.ClientId,
                contextData.CashOperationId, hash);
        }

        private async Task HandleIssueAsync(string operationContext, string hash)
        {
            var contextData = operationContext.DeserializeJson<IssueContextData>();

            await _cashOperationsRepositoryClient.UpdateBlockchainHashAsync(contextData.ClientId,
                contextData.CashOperationId, hash);
        }

        private async Task HandleCashOutAsync(string operationContext, string hash)
        {
            var contextData = operationContext.DeserializeJson<CashOutContextData>();

            await _cashOperationsRepositoryClient.UpdateBlockchainHashAsync(contextData.ClientId,
                contextData.CashOperationId, hash);
        }

        private async Task HandleSwapAsync(string operationContext, string hash)
        {
            var contextData = operationContext.DeserializeJson<SwapContextData>();
            foreach (var item in contextData.Trades)
            {
                await _clientTradesRepositoryClient.UpdateBlockchainHashAsync(item.ClientId, item.TradeId,
                    hash);
            }
        }

        private async Task HandleTransferAsync(string operationContext, string hash)
        {
            var contextData = operationContext.DeserializeJson<TransferContextData>();

            foreach (var transfer in contextData.Transfers)
            {
                await _transferEventsRepositoryClient.UpdateBlockChainHashAsync(transfer.ClientId, transfer.OperationId, hash);
            }
        }

        #region Tools

        public Func<string, string, Task> GetHandler(string command)
        {
            if (_handlers.ContainsKey(command))
                return _handlers[command];

            return null;
        }

        public void RegisterHandler(string operation, Func<string, string, Task> handler)
        {
            _handlers.Add(operation, handler);
        }

        #endregion
    }
}

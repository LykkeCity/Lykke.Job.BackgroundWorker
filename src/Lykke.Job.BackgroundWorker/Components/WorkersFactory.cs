using System;
using Lykke.Job.BackgroundWorker.Components.Workers;
using Lykke.Job.BackgroundWorker.Contract;

namespace Lykke.Job.BackgroundWorker.Components
{
    public class WorkersFactory : IWorkersFactory
    {
        private readonly Lazy<SetPinWorker> _setPinWorker;
        private readonly Lazy<SetAuthLogGeolocationWorker> _authLogGeoWorker;
        private readonly Lazy<UpdateHashForOperationsWorker> _updateHashWorker;
        private readonly Lazy<SetPartnerAccountInfoWorker> _setPartnerAccountInfoWorker;
        private readonly Lazy<CheckPersonWorker> _checkPersonWorker;

        public WorkersFactory(
            Lazy<SetPinWorker> setPinWorker,
            Lazy<SetAuthLogGeolocationWorker> authLogGeoWorker,
            Lazy<UpdateHashForOperationsWorker> updateHashWorker,
            Lazy<SetPartnerAccountInfoWorker> setPartnerAccountInfoWorker,
            Lazy<CheckPersonWorker> checkPersonWorker
        )

        {
            _setPinWorker = setPinWorker;
            _authLogGeoWorker = authLogGeoWorker;
            _updateHashWorker = updateHashWorker;
            _setPartnerAccountInfoWorker = setPartnerAccountInfoWorker;
            _checkPersonWorker = checkPersonWorker;
        }

        public IWorker GetWorker(WorkType workType, string contextJson)
        {
            IWorker worker = null;

            switch (workType)
            {
                case WorkType.SetPin:
                    worker = _setPinWorker.Value;
                    break;
                case WorkType.SetAuthLogGeolocation:
                    worker = _authLogGeoWorker.Value;
                    break;
                case WorkType.SetPartnerClientAccountInfo:
                    worker = _setPartnerAccountInfoWorker.Value;
                    break;
                case WorkType.UpdateHashForOperations:
                    worker = _updateHashWorker.Value;
                    break;
                case WorkType.CheckPerson:
                    worker = _checkPersonWorker.Value;
                    break;
            }

            worker?.SetContext(contextJson);
            return worker;
        }
    }
}
using System;
using Lykke.Job.BackgroundWorker.Components.Workers;
using Lykke.Job.BackgroundWorker.Contract;

namespace Lykke.Job.BackgroundWorker.Components
{
    public class WorkersFactory : IWorkersFactory
    {
        private readonly Lazy<SetPinWorker> _setPinWorker;
        private readonly Lazy<SetAuthLogGeolocationWorker> _authLogGeoWorker;
        
        public WorkersFactory(
            Lazy<SetPinWorker> setPinWorker,
            Lazy<SetAuthLogGeolocationWorker> authLogGeoWorker
        )

        {
            _setPinWorker = setPinWorker;
            _authLogGeoWorker = authLogGeoWorker;
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
            }

            worker?.SetContext(contextJson);
            return worker;
        }
    }
}
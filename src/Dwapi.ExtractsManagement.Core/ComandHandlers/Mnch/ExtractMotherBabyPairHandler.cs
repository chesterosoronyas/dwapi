﻿using System.Threading;
using System.Threading.Tasks;
using Dwapi.ExtractsManagement.Core.Commands.Mnch;
using Dwapi.ExtractsManagement.Core.Interfaces.Extratcors.Mnch;
using Dwapi.ExtractsManagement.Core.Interfaces.Loaders.Mnch;
using Dwapi.ExtractsManagement.Core.Interfaces.Repository;
using Dwapi.ExtractsManagement.Core.Interfaces.Utilities;
using Dwapi.ExtractsManagement.Core.Interfaces.Validators;
using Dwapi.ExtractsManagement.Core.Model.Destination.Mnch;
using Dwapi.ExtractsManagement.Core.Model.Source.Mnch;
using Dwapi.ExtractsManagement.Core.Notifications;
using Dwapi.SharedKernel.Enum;
using Dwapi.SharedKernel.Events;
using Dwapi.SharedKernel.Model;
using MediatR;

namespace Dwapi.ExtractsManagement.Core.ComandHandlers.Mnch
{
    public class ExtractMotherBabyPairHandler :IRequestHandler<ExtractMotherBabyPair,bool>
    {
        private readonly IMotherBabyPairSourceExtractor _motherBabyPairSourceExtractor;
        private readonly IExtractValidator _extractValidator;
        private readonly IMotherBabyPairLoader _motherBabyPairLoader;
        private readonly IExtractHistoryRepository _extractHistoryRepository;

        public ExtractMotherBabyPairHandler(IMotherBabyPairSourceExtractor motherBabyPairSourceExtractor, IExtractValidator extractValidator, IMotherBabyPairLoader motherBabyPairLoader, IExtractHistoryRepository extractHistoryRepository)
        {
            _motherBabyPairSourceExtractor = motherBabyPairSourceExtractor;
            _extractValidator = extractValidator;
            _motherBabyPairLoader = motherBabyPairLoader;
            _extractHistoryRepository = extractHistoryRepository;
        }

        public async Task<bool> Handle(ExtractMotherBabyPair request, CancellationToken cancellationToken)
        {
            //Extract
            int found = await _motherBabyPairSourceExtractor.Extract(request.Extract, request.DatabaseProtocol);

            //Validate
            await _extractValidator.Validate(request.Extract.Id, found, nameof(MotherBabyPairExtract), $"{nameof(TempMotherBabyPairExtract)}s");

            //Load
            int loaded = await _motherBabyPairLoader.Load(request.Extract.Id, found, request.DatabaseProtocol.SupportsDifferential);

            int rejected =
                _extractHistoryRepository.ProcessRejected(request.Extract.Id, found - loaded, request.Extract);


            _extractHistoryRepository.ProcessExcluded(request.Extract.Id, rejected, request.Extract);

            //notify loaded
            DomainEvents.Dispatch(
                new ExtractActivityNotification(request.Extract.Id, new DwhProgress(
                    nameof(MotherBabyPairExtract),
                    nameof(ExtractStatus.Loaded),
                    found, loaded, rejected, loaded, 0)));

            return true;
        }
    }
}

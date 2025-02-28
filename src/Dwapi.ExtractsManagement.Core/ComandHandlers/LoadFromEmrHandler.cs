﻿using Dwapi.ExtractsManagement.Core.Commands;
using Dwapi.ExtractsManagement.Core.Commands.Dwh;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dwapi.ExtractsManagement.Core.ComandHandlers
{
    public class LoadFromEmrHandler : IRequestHandler<LoadFromEmrCommand, bool>
    {
        private IMediator _mediator;

        public LoadFromEmrHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(LoadFromEmrCommand request, CancellationToken cancellationToken)
        {
            var extractIds = request.Extracts.Select(x => x.Extract.Id).ToList();

            await _mediator.Send(new ClearAllExtracts(extractIds), cancellationToken);


            var patientProfile = request.Extracts.FirstOrDefault(x => x.Extract.IsPriority);
            //Generate extract patient command
            if (patientProfile != null)
            {
                var extractPatient = new ExtractPatient()
                {
                    //todo check if extracts from all emrs are loaded
                    Extract = patientProfile.Extract,
                    DatabaseProtocol = patientProfile.DatabaseProtocol
                };

                var result = await _mediator.Send(extractPatient, cancellationToken);

                if (!result)
                {
                    return false;
                }
            }

            return await ExtractAll(request, cancellationToken);
        }

        private async Task<bool> ExtractAll(LoadFromEmrCommand request, CancellationToken cancellationToken)
        {

            var ts1 = new List<Task<bool>>();
            var ts2 = new List<Task<bool>>();
            var ts3 = new List<Task<bool>>();
            var ts4 = new List<Task<bool>>();
            var ts5 = new List<Task<bool>>();
            var ts6 = new List<Task<bool>>();

            // ExtractPatientART
            var patientArtProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "PatientArtExtract");
            if (null != patientArtProfile)
            {
                var patientArtCommand = new ExtractPatientArt()
                {
                    Extract = patientArtProfile?.Extract,
                    DatabaseProtocol = patientArtProfile?.DatabaseProtocol
                };
                ts1.Add(_mediator.Send(patientArtCommand, cancellationToken));
            }

            // ExtractPatientBaselines
            var patientBaselinesProfile =
                request.Extracts.FirstOrDefault(x => x.Extract.Name == "PatientBaselineExtract");
            if (null != patientBaselinesProfile)
            {
                var patientBaselinesCommand = new ExtractPatientBaselines()
                {
                    Extract = patientBaselinesProfile?.Extract,
                    DatabaseProtocol = patientBaselinesProfile?.DatabaseProtocol
                };
                ts1.Add( _mediator.Send(patientBaselinesCommand, cancellationToken));
            }

            // ExtractPatientLaboratory
            var patientLaboratoryProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "PatientLabExtract");
            if (null != patientLaboratoryProfile)
            {
                var patientLaboratoryCommand = new ExtractPatientLaboratory()
                {
                    Extract = patientLaboratoryProfile?.Extract,
                    DatabaseProtocol = patientLaboratoryProfile?.DatabaseProtocol
                };
                ts1.Add( _mediator.Send(patientLaboratoryCommand, cancellationToken));
            }

            // ExtractPatientPharmacy
            var patientPharmacyProfile =
                request.Extracts.FirstOrDefault(x => x.Extract.Name == "PatientPharmacyExtract");
            if (null != patientPharmacyProfile)
            {
                var patientPharmacyCommand = new ExtractPatientPharmacy()
                {
                    Extract = patientPharmacyProfile?.Extract,
                    DatabaseProtocol = patientPharmacyProfile?.DatabaseProtocol
                };
                ts2.Add(_mediator.Send(patientPharmacyCommand, cancellationToken));
            }

            // ExtractPatientStatus
            var patientStatusProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "PatientStatusExtract");
            if (null != patientStatusProfile)
            {
                var patientStatusCommand = new ExtractPatientStatus()
                {
                    Extract = patientStatusProfile?.Extract,
                    DatabaseProtocol = patientStatusProfile?.DatabaseProtocol
                };
                ts2.Add( _mediator.Send(patientStatusCommand, cancellationToken));
            }

            // ExtractPatientVisit
            var patientVisitProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "PatientVisitExtract");
            if (null != patientVisitProfile)
            {
                var patientVisitCommand = new ExtractPatientVisit()
                {
                    Extract = patientVisitProfile?.Extract,
                    DatabaseProtocol = patientVisitProfile?.DatabaseProtocol
                };
                ts2.Add(_mediator.Send(patientVisitCommand, cancellationToken));
            }

            // ExtractPatientAdverseEvent
            var patientAdverseEventProfile =
                request.Extracts.FirstOrDefault(x => x.Extract.Name == "PatientAdverseEventExtract");
            if (null != patientAdverseEventProfile)
            {
                var patientAdverseEventCommand = new ExtractPatientAdverseEvent()
                {
                    Extract = patientAdverseEventProfile?.Extract,
                    DatabaseProtocol = patientAdverseEventProfile?.DatabaseProtocol
                };
                ts3.Add( _mediator.Send(patientAdverseEventCommand, cancellationToken));
            }


            // ExtractAllergiesChronicIllness
            var allergiesChronicIllnessProfile =
                request.Extracts.FirstOrDefault(x => x.Extract.Name == "AllergiesChronicIllnessExtract");
            if (null != allergiesChronicIllnessProfile)
            {
                var allergiesChronicIllnessCommand = new ExtractAllergiesChronicIllness()
                {
                    Extract = allergiesChronicIllnessProfile?.Extract,
                    DatabaseProtocol = allergiesChronicIllnessProfile?.DatabaseProtocol
                };
                ts3.Add(_mediator.Send(allergiesChronicIllnessCommand, cancellationToken));
            }

            // ExtractContactListing
            var contactListingProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "ContactListingExtract");
            if (null != contactListingProfile)
            {
                var contactListingCommand = new ExtractContactListing()
                {
                    Extract = contactListingProfile?.Extract,
                    DatabaseProtocol = contactListingProfile?.DatabaseProtocol
                };
                ts3.Add(_mediator.Send(contactListingCommand, cancellationToken));
            }

            // ExtractDepressionScreening
            var depressionScreeningProfile =
                request.Extracts.FirstOrDefault(x => x.Extract.Name == "DepressionScreeningExtract");
            if (null != depressionScreeningProfile)
            {
                var depressionScreeningCommand = new ExtractDepressionScreening()
                {
                    Extract = depressionScreeningProfile?.Extract,
                    DatabaseProtocol = depressionScreeningProfile?.DatabaseProtocol
                };
                ts4.Add( _mediator.Send(depressionScreeningCommand, cancellationToken));
            }

            // ExtractDrugAlcoholScreening
            var drugAlcoholScreeningProfile =
                request.Extracts.FirstOrDefault(x => x.Extract.Name == "DrugAlcoholScreeningExtract");
            if (null != drugAlcoholScreeningProfile)
            {
                var drugAlcoholScreeningCommand = new ExtractDrugAlcoholScreening()
                {
                    Extract = drugAlcoholScreeningProfile?.Extract,
                    DatabaseProtocol = drugAlcoholScreeningProfile?.DatabaseProtocol
                };
                ts4.Add( _mediator.Send(drugAlcoholScreeningCommand, cancellationToken));
            }

            // ExtractEnhancedAdherenceCounselling
            var enhancedAdherenceCounsellingProfile =
                request.Extracts.FirstOrDefault(x => x.Extract.Name == "EnhancedAdherenceCounsellingExtract");
            if (null != enhancedAdherenceCounsellingProfile)
            {
                var enhancedAdherenceCounsellingCommand = new ExtractEnhancedAdherenceCounselling()
                {
                    Extract = enhancedAdherenceCounsellingProfile?.Extract,
                    DatabaseProtocol = enhancedAdherenceCounsellingProfile?.DatabaseProtocol
                };
                ts4.Add(_mediator.Send(enhancedAdherenceCounsellingCommand, cancellationToken));
            }

            // ExtractGbvScreening
            var gbvScreeningProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "GbvScreeningExtract");
            if (null != gbvScreeningProfile)
            {
                var gbvScreeningCommand = new ExtractGbvScreening()
                {
                    Extract = gbvScreeningProfile?.Extract,
                    DatabaseProtocol = gbvScreeningProfile?.DatabaseProtocol
                };
                ts5.Add( _mediator.Send(gbvScreeningCommand, cancellationToken));
            }

            // ExtractPatientVisit
            var iptProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "IptExtract");
            if (null != iptProfile)
            {
                var iptCommand = new ExtractIpt()
                {
                    Extract = iptProfile?.Extract,
                    DatabaseProtocol = iptProfile?.DatabaseProtocol
                };
                ts5.Add( _mediator.Send(iptCommand, cancellationToken));
            }


            // ExtractOtz
            var otzProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "OtzExtract");
            if (null != otzProfile)
            {
                var otzCommand = new ExtractOtz()
                {
                    Extract = otzProfile?.Extract,
                    DatabaseProtocol = otzProfile?.DatabaseProtocol
                };
                ts5.Add(_mediator.Send(otzCommand, cancellationToken));
            }

            // ExtractOvc
            var ovcProfile = request.Extracts.FirstOrDefault(x => x.Extract.Name == "OvcExtract");
            if (null != ovcProfile)
            {
                var ovcCommand = new ExtractOvc()
                {
                    Extract = ovcProfile?.Extract,
                    DatabaseProtocol = ovcProfile?.DatabaseProtocol
                };
                ts6.Add( _mediator.Send(ovcCommand, cancellationToken));
            }

            var result1 = await Task.WhenAll(ts1);
            var result2 = await Task.WhenAll(ts2);
            var result3 = await Task.WhenAll(ts3);
            var result4 = await Task.WhenAll(ts4);
            var result5 = await Task.WhenAll(ts5);
            var result6 = await Task.WhenAll(ts6);

            var result = new List<bool>();

            result.AddRange(result1);
            result.AddRange(result2);
            result.AddRange(result3);
            result.AddRange(result4);
            result.AddRange(result5);
            result.AddRange(result6);

            return result.All(x => x);
        }
    }
}

using Dwapi.ExtractsManagement.Core.Interfaces.Repository;
using Dwapi.ExtractsManagement.Core.Notifications;
using Dwapi.SharedKernel.Enum;
using Dwapi.SharedKernel.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dwapi.NotificationHandlers
{
    public class MnchExtractActivityNotificationHandler : IHandler<MnchExtractActivityNotification>
    {
        private IExtractHistoryRepository _extractHistoryRepository;

        public async void Handle(MnchExtractActivityNotification domainEvent)
        {
            _extractHistoryRepository = Startup.ServiceProvider.GetService<IExtractHistoryRepository>();

            SaveExtractHistory(domainEvent);
            await Startup.MnchHubContext.Clients.All.SendAsync("ShowMnchProgress", domainEvent);
        }

        private void SaveExtractHistory(MnchExtractActivityNotification domainEvent)
        {
            int count = 0;
            var status = (ExtractStatus)Enum.Parse(typeof(ExtractStatus), domainEvent.Progress.Status);

            switch (status)
            {
                case ExtractStatus.Found:
                    count = domainEvent.Progress.Found;
                    break;

                case ExtractStatus.Loaded:
                    count = domainEvent.Progress.Loaded;
                    break;
                case ExtractStatus.Sending:
                    count = domainEvent.Progress.Sent;
                    break;
                case ExtractStatus.Sent:
                    count = domainEvent.Progress.Sent;
                    break;
            }

            if (count == 0)
            {
                _extractHistoryRepository.DwhUpdateStatus(domainEvent.ExtractId, status);
            }
            else
            {
                _extractHistoryRepository.DwhUpdateStatus(domainEvent.ExtractId, status, count);
            }
        }
    }
}

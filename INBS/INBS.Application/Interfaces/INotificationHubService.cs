using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Application.Interfaces
{
    public interface INotificationHubService
    {
        Task NotifyBookingCreated(Guid artistId, string title, string message, object bookingData);
        Task NotifyBookingUpdated(Guid artistId, string title, string message, object bookingData);
        Task NotifyBookingCanceled(Guid artistId, string title, string message, Guid bookingId);
        Task NotifyArtistStoreAccepted(Guid artistId, string title, string message);
        Task NotifyArtistStoreRejected(Guid artistId, string title, string message);
        Task NotifyArtistStoreUpdated(Guid artistId, string title, string message);
        Task NotifyArtistStoreIsCreated(Guid adminId, string title, string message);
    }
}

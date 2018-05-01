using System.Collections.Generic;
using System.Threading.Tasks;
using RideSharing.RideApi.Controllers;

namespace RideSharing.RideApi
{
    public interface IRideEventsApi
    {
        Task<IReadOnlyList<StoredEventReadModel>> GetUnprocessedMessages();
    }
}
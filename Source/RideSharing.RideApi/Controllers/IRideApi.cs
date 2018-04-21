using System.Collections.Generic;
using System.Threading.Tasks;

namespace RideSharing.RideApi.Controllers
{
    public interface IRideApi
    {
        Task<IReadOnlyList<RideResourceModel>> Get();
    }
}
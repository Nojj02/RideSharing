using System.Threading.Tasks;
using RideSharing.DriverApi.Model;

namespace RideSharing.DriverApi.Controllers
{
    public interface IDriverRequestsApi
    {
        Task Post(DriverRequestPostModel driverRequestPostModel);
    }
}
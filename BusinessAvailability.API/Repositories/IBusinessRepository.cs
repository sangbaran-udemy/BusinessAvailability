using BusinessAvailability.API.Models.Domain;

namespace BusinessAvailability.API.Repositories
{
    public interface IBusinessRepository
    {
        IEnumerable<BusinessService> GetAll();
        IEnumerable<BusinessService> GetAvailableBusinesses(string startTime, string endTime);
    }
}

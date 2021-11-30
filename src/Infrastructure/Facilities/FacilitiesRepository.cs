using Domain.Facilities.Models;
using Domain.Facilities.Repositories;

namespace Infrastructure.Facilities;

public class FacilitiesRepository : IFacilitiesRepository
{
    public Task<bool> FacilityExistsAsync(ApbFacilityId facilityId)
    {
        throw new NotImplementedException();
    }

    public Task<Facility?> GetFacilityAsync(ApbFacilityId facilityId)
    {
        throw new NotImplementedException();
    }
}

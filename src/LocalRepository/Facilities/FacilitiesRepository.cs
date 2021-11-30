using Domain.Facilities.Repositories;
using static LocalRepository.Data.FacilityData;

namespace LocalRepository.Facilities;

public class FacilitiesRepository : IFacilitiesRepository
{
    public Task<bool> FacilityExistsAsync(ApbFacilityId facilityId) =>
        Task.FromResult(GetFacilities.Any(e => e.Id == facilityId));

    public async Task<Facility?> GetFacilityAsync(ApbFacilityId facilityId) =>
        await FacilityExistsAsync(facilityId)
        ? GetFacilities.SingleOrDefault(e => e.Id == facilityId)
        : null;       
}

using Domain.Facilities.Repositories;
using static LocalRepository.Data.FacilityData;

namespace LocalRepository.Facilities;

public class FacilitiesRepository : IFacilitiesRepository
{
    public Task<bool> FacilityExistsAsync(string id) =>
        Task.FromResult(FacilityId.IsValidFormat(id) && FacilityExists(id));

    private static bool FacilityExists(FacilityId id) =>
        FacilityData.Facilities.Any(e => e.Id == id);

    public Task<Facility?> GetFacilityAsync(FacilityId id) =>
        Task.FromResult(FacilityExists(id) ? GetFacility(id) : null);
}

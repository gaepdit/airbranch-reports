namespace Domain.Facilities.Repositories;

public interface IFacilitiesRepository
{
    Task<bool> FacilityExistsAsync(string id);
    Task<Facility?> GetFacilityAsync(FacilityId id);
}

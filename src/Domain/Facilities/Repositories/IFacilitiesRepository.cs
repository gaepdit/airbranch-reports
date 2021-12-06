namespace Domain.Facilities.Repositories
{
    public interface IFacilitiesRepository
    {
        Task<bool> FacilityExistsAsync(ApbFacilityId facilityId);
        Task<Facility?> GetFacilityAsync(ApbFacilityId facilityId);
    }
}

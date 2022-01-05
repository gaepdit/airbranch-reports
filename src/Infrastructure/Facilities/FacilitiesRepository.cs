using Dapper;
using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Domain.ValueObjects;
using System.Data;

namespace Infrastructure.Facilities;

public class FacilitiesRepository : IFacilitiesRepository
{
    private readonly IDbConnection db;
    public FacilitiesRepository(IDbConnection conn) => db = conn;

    public Task<bool> FacilityExistsAsync(ApbFacilityId facilityId)
    {
        return db.ExecuteScalarAsync<bool>(FacilitiesQueries.FacilityExists,
            new { AirsNumber = facilityId.DbFormattedString });
    }

    public async Task<Facility?> GetFacilityAsync(ApbFacilityId facilityId)
    {
        if (!await FacilityExistsAsync(facilityId)) return null;

        using var multi = await db.QueryMultipleAsync(FacilitiesQueries.GetFacility,
            new { AirsNumber = facilityId.DbFormattedString });

        var facility = multi.Read<Facility, Address, GeoCoordinates, FacilityHeaderData, Facility>(
            (facility, FacilityAddress, GeoCoordinates, HeaderData) =>
            {
                facility.FacilityAddress = FacilityAddress;
                facility.GeoCoordinates = GeoCoordinates;
                facility.HeaderData = HeaderData;
                return facility;
            }).Single();

        facility.HeaderData!.AirPrograms.AddRange(multi.Read<string>());
        facility.HeaderData!.ProgramClassifications.AddRange(multi.Read<string>());

        return facility;
    }
}

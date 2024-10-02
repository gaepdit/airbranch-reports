using Dapper;
using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Domain.ValueObjects;
using Infrastructure.DbConnection;
using System.Data;

namespace Infrastructure.Facilities;

public class FacilitiesRepository(IDbConnectionFactory dbf) : IFacilitiesRepository
{
    public async Task<bool> FacilityExistsAsync(string id) =>
        FacilityId.IsValidFormat(id) && await FacilityExistsAsync((FacilityId)id);

    private async Task<bool> FacilityExistsAsync(FacilityId id)
    {
        using var db = dbf.Create();

        return await db.ExecuteScalarAsync<bool>("air.FacilityExists",
            new { AirsNumber = id.DbFormattedString },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Facility?> GetFacilityAsync(FacilityId id)
    {
        using var db = dbf.Create();

        var varMultiTask = db.QueryMultipleAsync("air.GetFacility",
            new { AirsNumber = id.DbFormattedString },
            commandType: CommandType.StoredProcedure);

        if (!await FacilityExistsAsync(id)) return null;

        await using var multi = await varMultiTask;
        var facility = multi.Read<Facility, Address, GeoCoordinates, RegulatoryData, Facility>(
            (facility, facilityAddress, geoCoordinates, regulatoryData) =>
            {
                facility.FacilityAddress = facilityAddress;
                facility.GeoCoordinates = geoCoordinates;
                facility.RegulatoryData = regulatoryData;
                return facility;
            }).Single();

        facility.RegulatoryData!.AirPrograms.AddRange(await multi.ReadAsync<string>());
        facility.RegulatoryData!.ProgramClassifications.AddRange(await multi.ReadAsync<string>());

        return facility;
    }
}

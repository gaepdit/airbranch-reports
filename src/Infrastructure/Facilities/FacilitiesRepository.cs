using Dapper;
using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Domain.ValueObjects;
using System.Data;

namespace Infrastructure.Facilities;

public class FacilitiesRepository : IFacilitiesRepository
{
    private readonly IDbConnection _db;
    public FacilitiesRepository(IDbConnection conn) => _db = conn;

    public Task<bool> FacilityExistsAsync(ApbFacilityId facilityId) =>
        _db.ExecuteScalarAsync<bool>("air.FacilityExists",
            new { AirsNumber = facilityId.DbFormattedString },
            commandType: CommandType.StoredProcedure);

    public async Task<Facility?> GetFacilityAsync(ApbFacilityId facilityId)
    {
        if (!await FacilityExistsAsync(facilityId)) return null;

        using var multi = await _db.QueryMultipleAsync("air.GetFacility",
            new { AirsNumber = facilityId.DbFormattedString },
            commandType: CommandType.StoredProcedure);

        var facility = multi.Read<Facility, Address, GeoCoordinates, FacilityHeaderData, Facility>(
            (facility, facilityAddress, geoCoordinates, headerData) =>
            {
                facility.FacilityAddress = facilityAddress;
                facility.GeoCoordinates = geoCoordinates;
                facility.HeaderData = headerData;
                return facility;
            }).Single();

        facility.HeaderData!.AirPrograms.AddRange(multi.Read<string>());
        facility.HeaderData!.ProgramClassifications.AddRange(multi.Read<string>());

        return facility;
    }
}

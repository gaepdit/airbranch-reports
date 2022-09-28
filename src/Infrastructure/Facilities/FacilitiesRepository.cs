using Dapper;
using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Domain.ValueObjects;
using Infrastructure.DbConnection;
using System.Data;

namespace Infrastructure.Facilities;

public class FacilitiesRepository : IFacilitiesRepository
{
    private readonly IDbConnectionFactory _db;
    public FacilitiesRepository(IDbConnectionFactory db) => _db = db;

    public async Task<bool> FacilityExistsAsync(ApbFacilityId facilityId)
    {
        using var db = _db.Create();
        return await db.ExecuteScalarAsync<bool>("air.FacilityExists",
            new { AirsNumber = facilityId.DbFormattedString },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Facility?> GetFacilityAsync(ApbFacilityId facilityId)
    {
        using var db = _db.Create();

        var varMultiTask = db.QueryMultipleAsync("air.GetFacility",
            new { AirsNumber = facilityId.DbFormattedString },
            commandType: CommandType.StoredProcedure);

        if (!await FacilityExistsAsync(facilityId)) return null;

        using var multi = await varMultiTask;
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

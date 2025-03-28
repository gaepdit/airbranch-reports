﻿using Dapper;
using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Domain.ValueObjects;
using Infrastructure.DbConnection;
using System.Data;

namespace Infrastructure.Facilities;

public class FacilitiesRepository(IDbConnectionFactory dbf) : IFacilitiesRepository
{
    public async Task<bool> FacilityExistsAsync(ApbFacilityId facilityId)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<bool>("air.FacilityExists",
            new { AirsNumber = facilityId.DbFormattedString },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Facility?> GetFacilityAsync(ApbFacilityId facilityId)
    {
        using var db = dbf.Create();

        var varMultiTask = db.QueryMultipleAsync("air.GetFacility",
            new { AirsNumber = facilityId.DbFormattedString },
            commandType: CommandType.StoredProcedure);

        if (!await FacilityExistsAsync(facilityId)) return null;

        await using var multi = await varMultiTask;
        var facility = multi.Read<Facility, Address, GeoCoordinates, FacilityHeaderData, Facility>(
            (facility, facilityAddress, geoCoordinates, headerData) =>
            {
                facility.FacilityAddress = facilityAddress;
                facility.GeoCoordinates = geoCoordinates;
                facility.HeaderData = headerData;
                return facility;
            }).Single();

        facility.HeaderData!.AirPrograms.AddRange(await multi.ReadAsync<string>());
        facility.HeaderData!.ProgramClassifications.AddRange(await multi.ReadAsync<string>());

        return facility;
    }
}

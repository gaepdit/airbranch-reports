﻿using Domain.Monitoring.Models;
using Domain.Monitoring.Models.StackTestData;

namespace LocalRepository.Data;

public static class MonitoringData
{
    public static IEnumerable<StackTestReport> GetStackTestReports => new List<StackTestReport>
    {
        new StackTestReportOneStack() {
            ReferenceNumber = 201100541,
            Facility = FacilityData.GetFacilities.Single(e => e.Id.ShortString == "12100021"),
            Pollutant = "Total Reduced Sulfur Compounds",
            Source = "Process No. 1",
            ReportType = ReportType.SourceTest,
            DocumentType = DocumentType.OneStackThreeRuns,
            ApplicableRequirement = "Permit Condition 3.4.1",
            Comments = "N/A",
            TestDates = new DateTimeRange(
                new DateTime(2020, 10, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 1).Name,
            WitnessedByStaff = new List<PersonName>
            {
                StaffData.GetStaff.Single(s => s.Id == 2).Name,
                StaffData.GetStaff.Single(s => s.Id == 3).Name,
            },
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,
            MaxOperatingCapacity = new ValueWithUnits("100", "tons/hr"),
            OperatingCapacity = new ValueWithUnits("90", "tons/hr"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new ValueWithUnits("0.018", "lb/ton"),
            },
            ControlEquipmentInfo = "Scrubber pressure drop: 1 in. H2O\n\rScrubber recirculation rate: 200 gpm",
            TestRuns = new List<TestRun>
            {
                new TestRun
                {
                    RunNumber = "1",
                    GasTemperature = "175",
                    GasMoisture = "50",
                    GasFlowRateAscfm = "30000",
                    GasFlowRateDscfm = "13000",
                    PollutantConcentration = "17.1",
                    EmissionRate = "0.013",
                },
                new TestRun
                {
                    RunNumber = "2",
                    GasTemperature = "176",
                    GasMoisture = "51",
                    GasFlowRateAscfm = "30100",
                    GasFlowRateDscfm = "13100",
                    PollutantConcentration = "17.2",
                    EmissionRate = "0.014",
                },
                new TestRun
                {
                    RunNumber = "3",
                    GasTemperature = "174",
                    GasMoisture = "49",
                    GasFlowRateAscfm = "29900",
                    GasFlowRateDscfm = "12900",
                    PollutantConcentration = "17.0",
                    EmissionRate = "0.012",
                },
            },
            AvgPollutantConcentration = new ValueWithUnits("17.1","ppm"),
            AvgEmissionRate = new ValueWithUnits("0.013", "lb/ton"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "1",
        },
        new StackTestReportOneStack() {
            ReferenceNumber = 202001297,
            Facility = FacilityData.GetFacilities.Single(e => e.Id.ShortString == "17900001"),
            Pollutant = "Total Reduced Sulfur Compounds",
            Source = "Process No. 1",
            ReportType = ReportType.SourceTest,
            DocumentType = DocumentType.OneStackThreeRuns,
            ApplicableRequirement = "Permit Condition 3.4.1",
            Comments = "N/A",
            TestDates = new DateTimeRange(
                new DateTime(2020, 10, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 1).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,
            MaxOperatingCapacity = new ValueWithUnits("100", "tons/hr"),
            OperatingCapacity = new ValueWithUnits("90", "tons/hr"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new ValueWithUnits("0.018", "lb/ton"),
            },
            ControlEquipmentInfo = "Scrubber pressure drop: 1 in. H2O\n\rScrubber recirculation rate: 200 gpm",
            TestRuns = new List<TestRun>
            {
                new TestRun
                {
                    RunNumber = "1",
                    GasTemperature = "175",
                    GasMoisture = "50",
                    GasFlowRateAscfm = "30000",
                    GasFlowRateDscfm = "13000",
                    PollutantConcentration = "17.1",
                    EmissionRate = "0.013",
                },
                new TestRun
                {
                    RunNumber = "2",
                    GasTemperature = "176",
                    GasMoisture = "51",
                    GasFlowRateAscfm = "30100",
                    GasFlowRateDscfm = "13100",
                    PollutantConcentration = "17.2",
                    EmissionRate = "0.014",
                },
                new TestRun
                {
                    RunNumber = "3",
                    GasTemperature = "174",
                    GasMoisture = "49",
                    GasFlowRateAscfm = "29900",
                    GasFlowRateDscfm = "12900",
                    PollutantConcentration = "17.0",
                    EmissionRate = "0.012",
                },
            },
            AvgPollutantConcentration = new ValueWithUnits("17.1","ppm"),
            AvgEmissionRate = new ValueWithUnits("0.013", "lb/ton"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "1",
        },
    };
}
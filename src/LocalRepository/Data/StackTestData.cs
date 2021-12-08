using Domain.Monitoring.Models;
using Domain.Monitoring.Models.Partials;
using Domain.Personnel;

namespace LocalRepository.Data;

public static class StackTestData
{
    public static IEnumerable<StackTestReport> GetStackTestReports => new List<StackTestReport>
    {
        new() {
            ReferenceNumber = 202099999,
            Facility = FacilityData.GetFacilities.First(),
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
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 1),
            WitnessedByStaff = new List<Staff>
            {
                StaffData.GetStaff.Single(s => s.Id == 2),
                StaffData.GetStaff.Single(s => s.Id == 3),
            },
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4),
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5),
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

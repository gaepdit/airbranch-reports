using Domain.StackTest.Models;
using Domain.StackTest.Models.TestRun;

namespace LocalRepository.Data;

public static class StackTestData
{
    public static IEnumerable<BaseStackTestReport> GetStackTestReports => new List<BaseStackTestReport>
    {
        new StackTestReportOneStack {
            DocumentType = DocumentType.OneStackThreeRuns,
            ReferenceNumber = 201100541,
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "12100021"),
            Pollutant = "Total Reduced Sulfur Compounds",
            Source = "Process No. 1",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.4.1",
            Comments = "N/A",
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
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
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns = new List<StackTestRun>
            {
                new StackTestRun
                {
                    RunNumber = "1",
                    GasTemperature = "175",
                    GasMoisture = "50",
                    GasFlowRateAscfm = "30000",
                    GasFlowRateDscfm = "13000",
                    PollutantConcentration = "17.1",
                    EmissionRate = "0.013",
                    ConfidentialParametersCode = "",
                },
                new StackTestRun
                {
                    RunNumber = "2",
                    GasTemperature = "176",
                    GasMoisture = "51",
                    GasFlowRateAscfm = "30100",
                    GasFlowRateDscfm = "13100",
                    PollutantConcentration = "17.2",
                    EmissionRate = "0.014",
                    ConfidentialParametersCode = "0000000",
                },
                new StackTestRun
                {
                    RunNumber = "3",
                    GasTemperature = "174",
                    GasMoisture = "49",
                    GasFlowRateAscfm = "29900",
                    GasFlowRateDscfm = "12900",
                    PollutantConcentration = "17.0",
                    EmissionRate = "0.012",
                    ConfidentialParametersCode = "0101010",
                },
            },
            AvgPollutantConcentration = new ValueWithUnits("17.1", "ppm"),
            AvgEmissionRate = new ValueWithUnits("0.013", "lb/ton"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "1B000000000000000000000001100000000000011000001100000110101",
        },
        new StackTestReportOneStack {
            DocumentType = DocumentType.OneStackThreeRuns,
            ReferenceNumber = 202001297,
            Facility = FacilityData.GetFacilities.Single(e => e.Id!.ShortString == "17900001"),
            Pollutant = "Total Reduced Sulfur Compounds",
            Source = "Process No. 1",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.4.1",
            Comments = "N/A",
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 2).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,

            MaxOperatingCapacity = new ValueWithUnits("100", "tons/hr"),
            OperatingCapacity = new ValueWithUnits("90", "tons/hr"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new ValueWithUnits("0.018", "lb/ton"),
            },
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns = new List<StackTestRun>
            {
                new StackTestRun
                {
                    RunNumber = "1",
                    GasTemperature = "175",
                    GasMoisture = "50",
                    GasFlowRateAscfm = "30000",
                    GasFlowRateDscfm = "13000",
                    PollutantConcentration = "17.1",
                    EmissionRate = "0.013",
                    ConfidentialParametersCode = "",
                },
                new StackTestRun
                {
                    RunNumber = "2",
                    GasTemperature = "176",
                    GasMoisture = "51",
                    GasFlowRateAscfm = "30100",
                    GasFlowRateDscfm = "13100",
                    PollutantConcentration = "17.2",
                    EmissionRate = "0.014",
                    ConfidentialParametersCode = "",
                },
                new StackTestRun
                {
                    RunNumber = "3",
                    GasTemperature = "174",
                    GasMoisture = "49",
                    GasFlowRateAscfm = "29900",
                    GasFlowRateDscfm = "12900",
                    PollutantConcentration = "17.0",
                    EmissionRate = "0.012",
                    ConfidentialParametersCode = "",
                },
            },
            AvgPollutantConcentration = new ValueWithUnits("17.1", "ppm"),
            AvgEmissionRate = new ValueWithUnits("0.013", "lb/ton"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "0",
        },
        new StackTestReportTwoStack
        {
            DocumentType = DocumentType.TwoStackStandard,
            ReferenceNumber = 201600525,
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "24500002"),
            Pollutant = "Particulate Matter",
            Source = "Tower",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.Multiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2016, 9, 1),
                new DateTime(2016, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2016, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 6).Name,

            MaxOperatingCapacity = new ValueWithUnits("40", "ton/HR"),
            OperatingCapacity = new ValueWithUnits("30", "ton/HR"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new ValueWithUnits("1", "lb/TON"),
                new ValueWithUnits("20", "LB/HR"),
            },
            ControlEquipmentInfo = TextData.VeryShort,
            StackOneName = "Inner",
            StackTwoName = "Outer",
            TestRuns = new List<TwoStackTestRun>
            {
                new TwoStackTestRun
                {
                    RunNumber = "1",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAscfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAscfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                },
                new TwoStackTestRun
                {
                    RunNumber = "2",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAscfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAscfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                },
                new TwoStackTestRun
                {
                    RunNumber = "3",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAscfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAscfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                },
            },
            StackOneAvgPollutantConcentration = new ValueWithUnits("0.03","GR/DSCF"),
            StackTwoAvgPollutantConcentration = new ValueWithUnits("0.003","GR/DSCF"),
            StackOneAvgEmissionRate = new ValueWithUnits("4.00","LB/HR"),
            StackTwoAvgEmissionRate = new ValueWithUnits("3.00","LB/HR"),
            SumAvgEmissionRate = new ValueWithUnits("7.00", "LB/HR"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "0",
        },
        new StackTestReportTwoStack
        {
            DocumentType = DocumentType.TwoStackDre,
            ReferenceNumber = 200400473,
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "07300003"),
            Pollutant = "Particulate Matter",
            Source = "Tower",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.Multiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2016, 9, 1),
                new DateTime(2016, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2016, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 6).Name,

            MaxOperatingCapacity = new ValueWithUnits("40", "ton/HR"),
            OperatingCapacity = new ValueWithUnits("30", "ton/HR"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new ValueWithUnits("1", "lb/TON"),
                new ValueWithUnits("20", "LB/HR"),
            },
            ControlEquipmentInfo = TextData.VeryShort,
            StackOneName = "Inlet",
            StackTwoName = "Oulet",
            TestRuns = new List<TwoStackTestRun>
            {
                new TwoStackTestRun
                {
                    RunNumber = "1",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAscfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAscfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                },
                new TwoStackTestRun
                {
                    RunNumber = "2",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAscfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAscfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                },
                new TwoStackTestRun
                {
                    RunNumber = "3",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAscfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAscfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                },
            },
            StackOneAvgPollutantConcentration = new ValueWithUnits("0.03","GR/DSCF"),
            StackTwoAvgPollutantConcentration = new ValueWithUnits("0.003","GR/DSCF"),
            StackOneAvgEmissionRate = new ValueWithUnits("4.00","LB/HR"),
            StackTwoAvgEmissionRate = new ValueWithUnits("3.00","LB/HR"),
            DestructionEfficiency = "75.0",
            ConfidentialParametersCode = "0",
        },
        new StackTestReportLoadingRack
        {
            DocumentType = DocumentType.LoadingRack,
            ReferenceNumber = 201901149,
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "05900071"),
            Pollutant = "Volatile Organic Compounds",
            Source = "Tank Truck Loading Rack",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.ShortMultiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 3).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,

            MaxOperatingCapacity = new ValueWithUnits("400,000,000", "GPY"),
            OperatingCapacity = new ValueWithUnits("90,000", "GPY"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new ValueWithUnits("18", "mg/L"),
            },
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestDuration = new ValueWithUnits("6", "Hours"),
            PollutantConcentrationIn = new ValueWithUnits("20", "%"),
            PollutantConcentrationOut = new ValueWithUnits("120", "PPM"),
            EmissionRate = new ValueWithUnits("9.9", "mg/L"),
            DestructionReduction = new ValueWithUnits("98.2", "%"),
            ConfidentialParametersCode = "1F000000000000000000000001000000010001",
        },
        new StackTestReportPondTreatment
        {
            DocumentType = DocumentType.PondTreatment,
            ReferenceNumber = 200400023,
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "11500021"),
            Pollutant = "Methanol",
            Source = "Pond",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.Multiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 3).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,

            MaxOperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            OperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns = new List<PondTreatmentTestRun>
            {
                new PondTreatmentTestRun
                {
                    RunNumber = "1",
                    PollutantCollectionRate = "7",
                    TreatmentRate = "7.0",
                    ConfidentialParametersCode = "",
                },
                new PondTreatmentTestRun
                {
                    RunNumber = "2",
                    PollutantCollectionRate = "7.4",
                    TreatmentRate = "37",
                    ConfidentialParametersCode = "7.2",
                },
                new PondTreatmentTestRun
                {
                    RunNumber = "3",
                    PollutantCollectionRate = "7.8",
                    TreatmentRate = "39",
                    ConfidentialParametersCode = "7.4",
                },
            },
            AvgPollutantCollectionRate = new ValueWithUnits("7.4", "lb/ODTP"),
            AvgTreatmentRate = new ValueWithUnits("7.2", "lb/ODTP"),
            DestructionEfficiency = "97.3",
            ConfidentialParametersCode = "",
        },
        new StackTestReportGasConcentration
        {
            DocumentType = DocumentType.GasConcentration,
            ReferenceNumber = 200400009,
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "15300040"),
            Pollutant = "Nitrogen Oxides",
            Source = "Combustion Turbine",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.Short,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 3).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,

            MaxOperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            OperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new ValueWithUnits("25", "PPM @ 15% O2"),
            },
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns = new List<GasConcentrationTestRun>
            {
                new GasConcentrationTestRun
                {
                    RunNumber = "1",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "",
                },
                new GasConcentrationTestRun
                {
                    RunNumber = "2",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "7.2",
                },
                new GasConcentrationTestRun
                {
                    RunNumber = "3",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "7.4",
                },
            },
            AvgPollutantConcentration = new ValueWithUnits("25", "PPM"),
            AvgEmissionRate = new ValueWithUnits("22", "PPM @ 15% O2"),
            PercentAllowable= "90",
            ConfidentialParametersCode = "",
        },
        new StackTestReportFlare
        {
            DocumentType = DocumentType.Flare,
            ReferenceNumber = 200400407,
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "05700040"),
            Pollutant = "Volatile Organic Compounds",
            Source = "Flare",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.Multiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 3).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,
            MaxOperatingCapacity = new ValueWithUnits("100", "%"),
            OperatingCapacity = new ValueWithUnits("100", "%"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new ValueWithUnits("80", "ft/sec", "Velocity less than"),
                new ValueWithUnits("200", "BTU/scf", "Heat Content greater than or equal to"),
            },
            ControlEquipmentInfo = TextData.Short,
            TestRuns = new List<FlareTestRun>
            {
                new FlareTestRun
                {
                    RunNumber = "1",
                    HeatingValue = "400",
                    EmissionRateVelocity = "35",
                    ConfidentialParametersCode = "",
                },
                new FlareTestRun
                {
                    RunNumber = "2",
                    HeatingValue = "450",
                    EmissionRateVelocity = "37",
                    ConfidentialParametersCode = "",
                },
                new FlareTestRun
                {
                    RunNumber = "3",
                    HeatingValue = "425",
                    EmissionRateVelocity = "39",
                    ConfidentialParametersCode = "",
                },
            },
            AvgHeatingValue = new ValueWithUnits("425", "BTU/scf"),
            AvgEmissionRateVelocity = new ValueWithUnits("37", "ft/sec"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "",
        },
        new StackTestReportRata
        {
            DocumentType = DocumentType.Rata,
            ReferenceNumber = 201200095,
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "30500001"),
            Pollutant = "Nitrogen Oxides",
            Source = "Boiler",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.VeryShort,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff.Single(s => s.Id == 3).Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff.Single(s => s.Id == 4).Name,
            TestingUnitManager = StaffData.GetStaff.Single(s => s.Id == 5).Name,
            ApplicableStandard = TextData.Short,
            Diluent = "Oxygen",
            Units = "LB/MMBTU",
            RelativeAccuracyCode = "AppStandard",
            RelativeAccuracyPercent = "5.5",
            RelativeAccuracyRequiredPercent = "10",
            RelativeAccuracyRequiredLabel  = "% of the applicable standard (when the average of " +
            "the RM test data is less than 50% of the applicable standard).",
            ComplianceStatus = "Pass",
            TestRuns = new List<RataTestRun>
            {
                new RataTestRun
                {
                    RunNumber = "1",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },
                new RataTestRun
                {
                    RunNumber = "2",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = true,
                    ConfidentialParametersCode = "",
                },
                new RataTestRun
                {
                    RunNumber = "3",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },
                new RataTestRun
                {
                    RunNumber = "4",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },
                new RataTestRun
                {
                    RunNumber = "5",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },
                new RataTestRun
                {
                    RunNumber = "6",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = true,
                    ConfidentialParametersCode = "",
                },
            },
            ConfidentialParametersCode = "",
        },
    };
}

using Domain.StackTest.Models;
using Domain.StackTest.Models.TestRun;

namespace LocalRepository.Data;

public static class StackTestData
{
    public static IEnumerable<BaseStackTestReport> StackTestReports => new List<BaseStackTestReport>
    {
        new StackTestReportOneStack
        {
            DocumentType = DocumentType.OneStackThreeRuns,
            ReferenceNumber = 201100541,
            Facility = FacilityData.GetFacility("12100021"),
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
            ReviewedByStaff = StaffData.GetStaff(1)!.Value.Name,
            WitnessedByStaff = new List<PersonName>
            {
                StaffData.GetStaff(2)!.Value.Name,
                StaffData.GetStaff(3)!.Value.Name,
            },
            ComplianceManager = StaffData.GetStaff(4)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(5)!.Value.Name,

            MaxOperatingCapacity = new ValueWithUnits("100", "tons/hr"),
            OperatingCapacity = new ValueWithUnits("90", "tons/hr"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new("0.018", "lb/ton"),
            },
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns = new List<StackTestRun>
            {
                new()
                {
                    RunNumber = "1",
                    GasTemperature = "175",
                    GasMoisture = "50",
                    GasFlowRateAcfm = "30000",
                    GasFlowRateDscfm = "13000",
                    PollutantConcentration = "17.1",
                    EmissionRate = "0.013",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "2",
                    GasTemperature = "176",
                    GasMoisture = "51",
                    GasFlowRateAcfm = "30100",
                    GasFlowRateDscfm = "13100",
                    PollutantConcentration = "17.2",
                    EmissionRate = "0.014",
                    ConfidentialParametersCode = "0000000",
                },
                new()
                {
                    RunNumber = "3",
                    GasTemperature = "174",
                    GasMoisture = "49",
                    GasFlowRateAcfm = "29900",
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
        new StackTestReportOneStack
        {
            DocumentType = DocumentType.OneStackThreeRuns,
            ReferenceNumber = 202001297,
            Facility = FacilityData.GetFacility("17900001"),
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
            ReviewedByStaff = StaffData.GetStaff(2)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(4)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(5)!.Value.Name,

            MaxOperatingCapacity = new ValueWithUnits("100", "tons/hr"),
            OperatingCapacity = new ValueWithUnits("90", "tons/hr"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new("0.018", "lb/ton"),
            },
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns = new List<StackTestRun>
            {
                new()
                {
                    RunNumber = "1",
                    GasTemperature = "175",
                    GasMoisture = "50",
                    GasFlowRateAcfm = "30000",
                    GasFlowRateDscfm = "13000",
                    PollutantConcentration = "17.1",
                    EmissionRate = "0.013",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "2",
                    GasTemperature = "176",
                    GasMoisture = "51",
                    GasFlowRateAcfm = "30100",
                    GasFlowRateDscfm = "13100",
                    PollutantConcentration = "17.2",
                    EmissionRate = "0.014",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "3",
                    GasTemperature = "174",
                    GasMoisture = "49",
                    GasFlowRateAcfm = "29900",
                    GasFlowRateDscfm = "12900",
                    PollutantConcentration = "17.0",
                    EmissionRate = "0.012",
                    ConfidentialParametersCode = "",
                },
            },
            AvgPollutantConcentration = new ValueWithUnits("17.1", "µg/m3"),
            AvgEmissionRate = new ValueWithUnits("0.013", "lb/ton"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "0",
        },
        new StackTestReportTwoStack
        {
            DocumentType = DocumentType.TwoStackStandard,
            ReferenceNumber = 201600525,
            Facility = FacilityData.GetFacility("24500002"),
            Pollutant = "Particulate Matter",
            Source = "Tower",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.LongMultiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2016, 9, 1),
                new DateTime(2016, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2016, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(4)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(5)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(6)!.Value.Name,

            MaxOperatingCapacity = new ValueWithUnits("40", "ton/HR"),
            OperatingCapacity = new ValueWithUnits("30", "ton/HR"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new("1", "lb/TON"),
                new("20", "LB/HR"),
            },
            ControlEquipmentInfo = TextData.None,
            StackOneName = "Inner",
            StackTwoName = "Outer",
            TestRuns = new List<TwoStackTestRun>
            {
                new()
                {
                    RunNumber = "1",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "2",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "3",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    SumEmissionRate = "0.023",
                    ConfidentialParametersCode = "",
                },
            },
            StackOneAvgPollutantConcentration = new ValueWithUnits("0.03", "GR/DSCF"),
            StackTwoAvgPollutantConcentration = new ValueWithUnits("0.003", "GR/DSCF"),
            StackOneAvgEmissionRate = new ValueWithUnits("4.00", "LB/HR"),
            StackTwoAvgEmissionRate = new ValueWithUnits("3.00", "LB/HR"),
            SumAvgEmissionRate = new ValueWithUnits("7.00", "LB/HR"),
            PercentAllowable = "75.0",
            ConfidentialParametersCode = "0",
        },
        new StackTestReportTwoStack
        {
            DocumentType = DocumentType.TwoStackDre,
            ReferenceNumber = 200400473,
            Facility = FacilityData.GetFacility("07300003"),
            Pollutant = "Particulate Matter",
            Source = "Tower",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.LongMultiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2016, 9, 1),
                new DateTime(2016, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2016, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(4)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(5)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(6)!.Value.Name,

            MaxOperatingCapacity = new ValueWithUnits("40", "ton/HR"),
            OperatingCapacity = new ValueWithUnits("30", "ton/HR"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new("1", "lb/TON"),
                new("20", "LB/HR"),
            },
            ControlEquipmentInfo = TextData.None,
            StackOneName = "Inlet",
            StackTwoName = "Oulet",
            TestRuns = new List<TwoStackTestRun>
            {
                new()
                {
                    RunNumber = "1",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "2",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "3",
                    StackOneGasTemperature = "175",
                    StackOneGasMoisture = "50",
                    StackOneGasFlowRateAcfm = "30000",
                    StackOneGasFlowRateDscfm = "13000",
                    StackOnePollutantConcentration = "17.1",
                    StackOneEmissionRate = "0.013",
                    StackTwoGasTemperature = "75",
                    StackTwoGasMoisture = "40",
                    StackTwoGasFlowRateAcfm = "20000",
                    StackTwoGasFlowRateDscfm = "10000",
                    StackTwoPollutantConcentration = "14",
                    StackTwoEmissionRate = "0.01",
                    ConfidentialParametersCode = "",
                },
            },
            StackOneAvgPollutantConcentration = new ValueWithUnits("0.03", "GR/DSCF"),
            StackTwoAvgPollutantConcentration = new ValueWithUnits("0.003", "GR/DSCF"),
            StackOneAvgEmissionRate = new ValueWithUnits("4.00", "LB/HR"),
            StackTwoAvgEmissionRate = new ValueWithUnits("3.00", "LB/HR"),
            DestructionEfficiency = "75.0",
            ConfidentialParametersCode = "0",
        },
        new StackTestReportLoadingRack
        {
            DocumentType = DocumentType.LoadingRack,
            ReferenceNumber = 201901149,
            Facility = FacilityData.GetFacility("05900071"),
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
            ReviewedByStaff = StaffData.GetStaff(3)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(4)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(5)!.Value.Name,

            MaxOperatingCapacity = new ValueWithUnits("400,000,000", "GPY"),
            OperatingCapacity = new ValueWithUnits("90,000", "GPY"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new("18", "mg/L"),
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
            Facility = FacilityData.GetFacility("11500021"),
            Pollutant = "Methanol",
            Source = "Pond",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.LongMultiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(3)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(4)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(5)!.Value.Name,

            MaxOperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            OperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns = new List<PondTreatmentTestRun>
            {
                new()
                {
                    RunNumber = "1",
                    PollutantCollectionRate = "7",
                    TreatmentRate = "7.0",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "2",
                    PollutantCollectionRate = "7.4",
                    TreatmentRate = "37",
                    ConfidentialParametersCode = "7.2",
                },
                new()
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
            Facility = FacilityData.GetFacility("15300040"),
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
            ReviewedByStaff = StaffData.GetStaff(3)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(4)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(5)!.Value.Name,

            MaxOperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            OperatingCapacity = new ValueWithUnits("2000", "Tons/Day"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new("25", "PPM @ 15% O2"),
            },
            ControlEquipmentInfo = TextData.ShortMultiline,
            TestRuns = new List<GasConcentrationTestRun>
            {
                new()
                {
                    RunNumber = "1",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "2",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "7.2",
                },
                new()
                {
                    RunNumber = "3",
                    PollutantConcentration = "26",
                    EmissionRate = "22",
                    ConfidentialParametersCode = "7.4",
                },
            },
            AvgPollutantConcentration = new ValueWithUnits("25", "PPM"),
            AvgEmissionRate = new ValueWithUnits("22", "PPM @ 15% O2"),
            PercentAllowable = "90",
            ConfidentialParametersCode = "",
        },
        new StackTestReportFlare
        {
            DocumentType = DocumentType.Flare,
            ReferenceNumber = 200400407,
            Facility = FacilityData.GetFacility("05700040"),
            Pollutant = "Volatile Organic Compounds",
            Source = "Flare",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.LongMultiline,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(3)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(4)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(5)!.Value.Name,
            MaxOperatingCapacity = new ValueWithUnits("100", "%"),
            OperatingCapacity = new ValueWithUnits("100", "%"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new("80", "ft/sec", "Velocity less than"),
                new("200", "BTU/scf", "Heat Content greater than or equal to"),
            },
            ControlEquipmentInfo = TextData.Short,
            TestRuns = new List<FlareTestRun>
            {
                new()
                {
                    RunNumber = "1",
                    HeatingValue = "400",
                    EmissionRateVelocity = "35",
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "2",
                    HeatingValue = "450",
                    EmissionRateVelocity = "37",
                    ConfidentialParametersCode = "",
                },
                new()
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
            Facility = FacilityData.GetFacility("30500001"),
            Pollutant = "Nitrogen Oxides",
            Source = "Boiler",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.None,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(3)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(4)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(5)!.Value.Name,
            ApplicableStandard = TextData.Short,
            Diluent = "Oxygen",
            Units = "LB/MMBTU",
            RelativeAccuracyCode = "AppStandard",
            RelativeAccuracyPercent = "5.5",
            RelativeAccuracyRequiredPercent = "10",
            RelativeAccuracyRequiredLabel = "% of the applicable standard (when the average of " +
                "the RM test data is less than 50% of the applicable standard).",
            ComplianceStatus = "Pass",
            TestRuns = new List<RataTestRun>
            {
                new()
                {
                    RunNumber = "1",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "2",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = true,
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "3",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "4",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },
                new()
                {
                    RunNumber = "5",
                    ReferenceMethod = "0.023",
                    Cms = "0.022",
                    Omitted = false,
                    ConfidentialParametersCode = "",
                },
                new()
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
        new StackTestMemorandum
        {
            DocumentType = DocumentType.MemorandumStandard,
            ReferenceNumber = 200600289,
            Facility = FacilityData.GetFacility("17900001"),
            Pollutant = "Methanol",
            Source = "System",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(1)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(2)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(3)!.Value.Name,
            Comments = TextData.LongMultiline,
        },
        new StackTestMemorandum
        {
            DocumentType = DocumentType.MemorandumToFile,
            ReferenceNumber = 201500570,
            Facility = FacilityData.GetFacility("17900001"),
            Pollutant = "Opacity",
            Source = "Monitor",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(1)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(2)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(3)!.Value.Name,
            Comments = TextData.LongMultiline,
            MonitorManufacturer = TextData.Short,
            MonitorSerialNumber = TextData.VeryShort,
        },
        new StackTestMemorandum
        {
            DocumentType = DocumentType.PTE,
            ReferenceNumber = 200400476,
            Facility = FacilityData.GetFacility("07300003"),
            Pollutant = "VOC",
            Source = "System",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(1)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(2)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(3)!.Value.Name,
            Comments = TextData.LongMultiline,
            MaxOperatingCapacity = new ValueWithUnits("100000", "Units"),
            OperatingCapacity = new ValueWithUnits("50000", "Units"),
            AllowableEmissionRates = new List<ValueWithUnits>
            {
                new("100", "%"),
            },
            ControlEquipmentInfo = TextData.Long,
        },
        new StackTestReportOpacity
        {
            DocumentType = DocumentType.Method9Multi,
            ReferenceNumber = 201801068,
            Facility = FacilityData.GetFacility("11500021"),
            Pollutant = "Opacity",
            Source = "Kiln",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.VeryShort,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(3)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(4)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(5)!.Value.Name,
            ControlEquipmentInfo = TextData.ShortMultiline,
            ComplianceStatus = "In Compliance",
            OpacityStandard = "Highest 6-minute average",
            MaxOperatingCapacityUnits = "Tons/Day",
            OperatingCapacityUnits = "Tons/Day",
            AllowableEmissionRateUnits = "% Opacity",
            TestRuns =
            {
                new OpacityTestRun
                {
                    RunNumber = "1",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "230",
                    AllowableEmissionRate = "40",
                    Opacity = "15",
                    EquipmentItem = TextData.Short,
                },
                new OpacityTestRun
                {
                    RunNumber = "2",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "200",
                    AllowableEmissionRate = "40",
                    Opacity = "12",
                    EquipmentItem = TextData.Short,
                },
                new OpacityTestRun
                {
                    RunNumber = "3",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "210",
                    AllowableEmissionRate = "40",
                    Opacity = "20",
                    EquipmentItem = TextData.Short,
                },
                new OpacityTestRun
                {
                    RunNumber = "4",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "190",
                    AllowableEmissionRate = "40",
                    Opacity = "19",
                    EquipmentItem = TextData.Short,
                },
                new OpacityTestRun
                {
                    RunNumber = "5",
                    MaxOperatingCapacity = "270",
                    OperatingCapacity = "210",
                    AllowableEmissionRate = "40",
                    Opacity = "21",
                    EquipmentItem = TextData.Short,
                },
            },
        },
        new StackTestReportOpacity
        {
            DocumentType = DocumentType.Method22,
            ReferenceNumber = 200600052,
            Facility = FacilityData.GetFacility("31300062"),
            Pollutant = "Opacity",
            Source = "Bin",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.None,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(2)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(3)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(4)!.Value.Name,
            ControlEquipmentInfo = TextData.ShortMultiline,
            ComplianceStatus = "Not In Compliance",
            TestDuration = "60 minutes",
            MaxOperatingCapacityUnits = "Tons/HR",
            OperatingCapacityUnits = "Tons/HR",
            TestRuns =
            {
                new OpacityTestRun
                {
                    RunNumber = "1",
                    MaxOperatingCapacity = "3",
                    OperatingCapacity = "3",
                    AllowableEmissionRate = "0 % Opacity",
                    AccumulatedEmissionTime = "20:00",
                },
            },
        },
        new StackTestReportOpacity
        {
            DocumentType = DocumentType.Method9Single,
            ReferenceNumber = 200700192,
            Facility = FacilityData.GetFacility("24500002"),
            Pollutant = "Opacity",
            Source = "Scrubber",
            ReportType = ReportType.SourceTest,
            ApplicableRequirement = "Permit Condition 3.1",
            Comments = TextData.None,
            ReportStatement = TextData.ReportStatement,
            TestDates = new DateRange(
                new DateTime(2020, 9, 1),
                new DateTime(2020, 10, 1)
            ),
            DateReceivedByApb = new DateTime(2020, 11, 1),
            ReviewedByStaff = StaffData.GetStaff(1)!.Value.Name,
            WitnessedByStaff = new List<PersonName>(),
            ComplianceManager = StaffData.GetStaff(2)!.Value.Name,
            TestingUnitManager = StaffData.GetStaff(3)!.Value.Name,
            ControlEquipmentInfo = TextData.ShortMultiline,
            ComplianceStatus = "In Compliance",
            OpacityStandard = "30-minute average",
            TestDuration = "180 minutes",
            MaxOperatingCapacityUnits = "UNITS",
            OperatingCapacityUnits = "UNITS",
            AllowableEmissionRateUnits = "%",
            TestRuns =
            {
                new OpacityTestRun
                {
                    RunNumber = "1",
                    MaxOperatingCapacity = "10.1",
                    OperatingCapacity = "9.9",
                    AllowableEmissionRate = "40.0",
                    Opacity = "0.1",
                },
            },
        },
    };
}

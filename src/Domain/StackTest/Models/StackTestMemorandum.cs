using System.ComponentModel.DataAnnotations;

namespace Domain.StackTest.Models;

public record class StackTestMemorandum : BaseStackTestReport
{
    [Display(Name = "Memorandum")]
    public string Memorandum { get; set; } = "";

    // Only used by MemorandumToFile
    [Display(Name = "Monitor manufacturer and model")]
    public string MonitorManufacturer { get; set; } = "";

    [Display(Name = "Monitor serial number")]
    public string MonitorSerialNumber { get; set; } = "";

    // Only used by PTE
    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; set; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; set; }

    [Display(Name = "Allowable emission rates")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; } = new List<ValueWithUnits>();

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; set; } = "";

    #region Confidential info handling

    public override StackTestMemorandum RedactedStackTestReport() =>
        RedactedBaseStackTestReport<StackTestMemorandum>() with
        {
            Memorandum = CheckConfidential(Memorandum, nameof(Memorandum)),
            MonitorManufacturer = CheckConfidential(MonitorManufacturer, nameof(MonitorManufacturer)),
            MonitorSerialNumber = CheckConfidential(MonitorSerialNumber, nameof(MonitorSerialNumber)),
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
        };

    public override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();

        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;
        ParseBaseConfidentialParameters();

        switch (DocumentType)
        {
            case DocumentType.MemorandumStandard:
                AddIfConfidential(27, nameof(Memorandum));
                break;

            case DocumentType.MemorandumToFile:
                AddIfConfidential(29, nameof(Memorandum));
                AddIfConfidential(27, nameof(MonitorManufacturer));
                AddIfConfidential(28, nameof(MonitorSerialNumber));
                break;

            case DocumentType.PTE:
                AddIfConfidential(33, nameof(Memorandum));
                AddIfConfidential(27, nameof(MaxOperatingCapacity));
                AddIfConfidential(28, nameof(OperatingCapacity));
                AddIfConfidential(32, nameof(ControlEquipmentInfo));
                break;

            default:
                break;
        }
    }

    #endregion
}

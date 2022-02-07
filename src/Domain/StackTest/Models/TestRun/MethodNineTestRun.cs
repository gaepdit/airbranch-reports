﻿using System.ComponentModel.DataAnnotations;

namespace Domain.StackTest.Models.TestRun;

public record class MethodNineTestRun : BaseTestRun
{
    [Display(Name = "Maximum expected operating capacity")]
    public string MaxOperatingCapacity { get; set; } = "";

    [Display(Name = "Operating capacity")]
    public string OperatingCapacity { get; set; } = "";

    [Display(Name = "Allowable emission rate(s)")]
    public string AllowableEmissionRate { get; set; } = "";

    [Display(Name = "Opacity")]
    public string Opacity { get; init; } = "";

    // `EquipmentItem` is used by "Method 22"
    // but not by "Method 9 (Single)" or "Method 9 (Multi.)"
    [Display(Name = "Accumulated emission time")]
    public string AccumulatedEmissionTime { get; set; } = "";

    // `EquipmentItem` is used by "Method 9 (Multi.)"
    // but not by "Method 9 (Single)" or "Method 22"
    [Display(Name = "Equipment item")]
    public string EquipmentItem { get; init; } = "";

    #region Confidential info handling

    public override MethodNineTestRun RedactedTestRun() =>
       RedactedBaseTestRun<MethodNineTestRun>() with
       {
           MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
           OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
           EquipmentItem = CheckConfidential(EquipmentItem, nameof(EquipmentItem)),
       };

    protected override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(2, nameof(MaxOperatingCapacity));
        AddIfConfidential(3, nameof(OperatingCapacity));
        AddIfConfidential(4, nameof(EquipmentItem));
    }

    #endregion
}

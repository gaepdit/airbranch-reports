﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Monitoring.Models.TestRuns;

public record class FlareTestRun : BaseTestRun
{
    [Display(Name = "Heating value")]
    public string HeatingValue { get; init; } = "";

    [Display(Name = "Emission rate velocity")]
    public string EmissionRateVelocity { get; init; } = "";

    #region Confidential info handling

    public override FlareTestRun RedactedTestRun() =>
        RedactedBaseTestRun<FlareTestRun>() with
        {
            HeatingValue = CheckConfidential(HeatingValue, nameof(HeatingValue)),
            EmissionRateVelocity = CheckConfidential(EmissionRateVelocity, nameof(EmissionRateVelocity)),
        };

    protected override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(1, nameof(RunNumber));
        AddIfConfidential(2, nameof(HeatingValue));
        AddIfConfidential(3, nameof(EmissionRateVelocity));
    }

    #endregion
}

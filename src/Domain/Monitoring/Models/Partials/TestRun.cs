﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Monitoring.Models.Partials;

public record struct TestRun
{
    [Display(Name = "Run Number")]
    public string RunNumber { get; init; }

    [Display(Name = "Gas temperature (°F)")]
    public string GasTemperature { get; init; }

    [Display(Name = "Gas moisture (%)")]
    public string GasMoisture { get; init; }

    [Display(Name = "Gas flow rate (ASCFM)")]
    public string GasFlowRateAscfm { get; init; }

    [Display(Name = "Gas flow rate (DSCFM)")]
    public string GasFlowRateDscfm { get; init; }

    [Display(Name = "Pollutant concentration")]
    public string PollutantConcentration { get; init; }

    [Display(Name = "Emission rate")]
    public string EmissionRate { get; init; }
}

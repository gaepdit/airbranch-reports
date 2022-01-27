﻿using Domain.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Facilities.Models;

public record class FacilityHeaderData
{
    // Operating status

    [JsonIgnore]
    public FacilityOperatingStatus OperatingStatusCode { get; init; }

    [Display(Name = "Operating status")]
    public string OperatingStatus => OperatingStatusCode.GetDescription();


    [Display(Name = "Startup date")]
    public DateTime? StartupDate { get; init; }

    [Display(Name = "Permit revocation date")]
    public DateTime? PermitRevocationDate { get; init; }

    // Classifications

    [JsonIgnore]
    public FacilityClassification ClassificationCode { get; init; }
    
    [Display(Name = "Classification")]
    public string Classification => ClassificationCode.GetDescription();

    [JsonIgnore]
    public FacilityCmsClassification CmsClassificationCode { get; init; }

    [Display(Name = "CMS classification")]
    public string CmsClassification => CmsClassificationCode.GetDescription();

    // Industry

    /// <summary>
    /// Facility ownership type.
    /// </summary>
    /// <remarks>
    /// Currently we only track federally-owned facilities, represented by the 
    /// OwnershipTypeCode "FDF" and description "Federal Facility (U.S. Government)"
    /// </remarks>
    [Display(Name = "Ownership type")]
    public string? OwnershipType { get; init; }

    [Display(Name = "SIC")]
    [RegularExpression(SicCodePattern)]
    public string? Sic { get; init; }

    [Display(Name = "NAICS")]
    [RegularExpression(NaicsCodePattern)]
    public string? Naics { get; init; }

    [Display(Name = "RMP ID")]
    [RegularExpression(RmpIdPattern)]
    public string? RmpId { get; init; }

    // Nonattainment areas

    [Display(Name = "1-hr ozone")]
    public OneHourOzoneNonattainmentStatus OneHourOzoneNonattainment { get; init; }

    [Display(Name = "8-hr ozone")]
    public EightHourOzoneNonattainmentStatus EightHourOzoneNonattainment { get; init; }

    [Display(Name = "PM 2.5")]
    public PmFineNonattainmentStatus PmFineNonattainment { get; init; }

    // Regulations

    [Display(Name = "NSPS fee exempt")]
    public bool NspsFeeExempt { get; init; }

    /// <summary>
    /// List of air programs that apply to a facility.
    /// </summary>
    /// <remarks>
    /// Possible values:
    /// SIP, Federal SIP, Non-Federal SIP, CFC Tracking, 
    /// PSD, NSR, NESHAP, NSPS, FESOP, Acid Precipitation, 
    /// Native American, MACT, Title V, Risk Management Plan
    /// </remarks>
    [Display(Name = "Air programs")]
    public List<string> AirPrograms { get; init; } = new List<string>();

    /// <summary>
    /// List of air program classifications that apply to a facility.
    /// </summary>
    /// <remarks>
    /// Possible values:
    /// NSR/PSD Major, HAPs Major
    /// </remarks>
    [Display(Name = "Program classifications")]
    public List<string> ProgramClassifications { get; init; } = new List<string>();

    #region Regex patterns

    // Valid SIC codes are one to four digits
    private const string SicCodePattern = @"^\d{1,4}$";

    // Valid NAICS codes are two to six digits
    private const string NaicsCodePattern = @"^\d{2,6}$";

    // Valid RMP IDs are in the form 0000-0000-0000 (with the dashes)
    private const string RmpIdPattern = @"^\d{4}-\d{4}-\d{4}$";

    #endregion
}

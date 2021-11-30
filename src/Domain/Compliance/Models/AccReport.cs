using Domain.Facilities.Models;
using Domain.Personnel;

namespace Domain.Compliance.Models;

public record struct AccReport
{
    public int Id { get; init; }

    public Facility Facility { get; init; }
    public int AccReportingYear { get; init; }
    public Staff StaffResponsible { get; init; }

    public DateTime DatePostmarked { get; init; }
    public DateTime DateReceived { get; init; }
    public DateTime? DateComplete { get; init; }
    public DateTime? DateAcknowledgmentLetterSent { get; init; }

    public bool PostmarkedByDeadline { get; init; }
    public bool SignedByResponsibleOfficial { get; init; }
    public bool CorrectFormsUsed { get; init; }
    public bool AllTitleVConditionsListed { get; init; }
    public bool CorrectlyFilledOut { get; init; }
    public bool DeviationsReported { get; init; }
    public bool UnreportedDeviationsReported { get; init; }
    public bool EnforcementNeeded { get; init; }
    public bool AllDeviationsReported { get; init; }
    public bool ResubmittalRequested { get; init; }
}

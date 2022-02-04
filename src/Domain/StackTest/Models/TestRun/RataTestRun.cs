using System.ComponentModel.DataAnnotations;

namespace Domain.StackTest.Models.TestRun;

public record class RataTestRun : BaseTestRun
{
    [Display(Name = "Reference method")]
    public string ReferenceMethod { get; init; } = "";

    [Display(Name = "CMS")]
    public string Cms { get; init; } = "";

    public bool Omitted { get; init; } = false;

    #region Confidential info handling

    public override RataTestRun RedactedTestRun() =>
        RedactedBaseTestRun<RataTestRun>() with
        {
            ReferenceMethod = CheckConfidential(ReferenceMethod, nameof(ReferenceMethod)),
            Cms = CheckConfidential(Cms, nameof(Cms)),
        };

    protected override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(1, nameof(RunNumber));
        AddIfConfidential(2, nameof(ReferenceMethod));
        AddIfConfidential(3, nameof(Cms));
    }

    #endregion
}

using FluentAssertions;
using LocalRepository.Data;
using LocalRepository.Monitoring;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.Monitoring;

public class GetDocumentType
{
    [Test]
    public async Task ReturnsDocumentTypeIfExists()
    {
        var report = StackTestData.GetStackTestReports.First();

        var repo = new MonitoringRepository();
        var result = await repo.GetDocumentTypeAsync(report.ReferenceNumber);
        result.Should().Be(report.DocumentType);
    }

    [Test]
    public async Task ThrowsIfNotExists()
    {
        var repo = new MonitoringRepository();
        Func<Task> action = async () => await repo.GetDocumentTypeAsync(default);
        (await action.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("Sequence contains no matching element");
    }
}

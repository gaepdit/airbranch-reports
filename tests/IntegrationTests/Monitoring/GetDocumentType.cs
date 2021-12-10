using Domain.Monitoring.Models;
using FluentAssertions;
using Infrastructure.Monitoring;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.Monitoring;

public class GetDocumentType
{
    [Test]
    public async Task ReturnsDocumentTypeIfExists()
    {
        var referenceNumber = 201100541;
        var repo = new MonitoringRepository(Global.db!);
        var result = await repo.GetDocumentTypeAsync(referenceNumber);
        result.Should().Be(DocumentType.OneStackThreeRuns);
    }

    [Test]
    public async Task ThrowsIfNotExists()
    {
        var repo = new MonitoringRepository(Global.db!);
        Func<Task> action = async () => await repo.GetDocumentTypeAsync(default);
        (await action.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("Sequence contains no elements");
    }
}

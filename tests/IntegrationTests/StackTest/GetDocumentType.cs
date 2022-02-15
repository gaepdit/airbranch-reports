using Domain.StackTest.Models;
using FluentAssertions;
using Infrastructure.StackTest;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.StackTest;

public class GetDocumentType
{
    [Test]
    public async Task ReturnsDocumentTypeIfExists()
    {
        var referenceNumber = 201100541;
        var repo = new StackTestRepository(Global.DbConn!);
        var result = await repo.GetDocumentTypeAsync(referenceNumber);
        result.Should().Be(DocumentType.OneStackThreeRuns);
    }

    [Test]
    public async Task ThrowsIfNotExists()
    {
        var repo = new StackTestRepository(Global.DbConn!);
        Func<Task> action = async () => await repo.GetDocumentTypeAsync(default);
        (await action.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("Sequence contains no elements");
    }
}

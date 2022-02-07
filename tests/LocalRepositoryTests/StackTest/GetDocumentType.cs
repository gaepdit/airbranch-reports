using FluentAssertions;
using LocalRepository.Data;
using LocalRepository.StackTest;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalRepositoryTests.StackTest;

public class GetDocumentType
{
    [Test]
    public async Task ReturnsDocumentTypeIfExists()
    {
        var report = StackTestData.StackTestReports.First();

        var repo = new StackTestRepository();
        var result = await repo.GetDocumentTypeAsync(report.ReferenceNumber);
        result.Should().Be(report.DocumentType);
    }

    [Test]
    public async Task ThrowsIfNotExists()
    {
        var repo = new StackTestRepository();
        Func<Task> action = async () => await repo.GetDocumentTypeAsync(default);
        (await action.Should().ThrowAsync<InvalidOperationException>())
            .WithMessage("Sequence contains no matching element");
    }
}

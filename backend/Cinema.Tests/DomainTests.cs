using System;
using Cinema.Domain.Entities;
using Xunit;

namespace Cinema.Tests;

public sealed class DomainTests
{
    [Fact]
    public void Screening_Throws_When_EndTime_Is_Before_StartTime()
    {
        var startsAt = DateTimeOffset.UtcNow;
        var endsAt = startsAt.AddMinutes(-10);

        Assert.Throws<ArgumentException>(() =>
            new Screening(Guid.NewGuid(), Guid.NewGuid(), startsAt, endsAt));
    }

    [Fact]
    public void Movie_Throws_When_Title_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() =>
            new Movie(" ", 120, "PG-13"));
    }
}

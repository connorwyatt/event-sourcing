using ConnorWyatt.EventSourcing.UnitTests.Aggregates.Stubs;
using ConnorWyatt.EventSourcing.UnitTests.Aggregates.Stubs.Events;
using FluentAssertions;
using NodaTime;

namespace ConnorWyatt.EventSourcing.UnitTests.Aggregates;

public class AggregateTests
{
  [Fact]
  public void Given_An_Aggregate__When_Calling_Methods_To_Update_The_State__Then_It_Should_Have_Unsaved_Events()
  {
    var testAggregate = CreateTestAggregate();

    testAggregate.IncreaseCount();
    testAggregate.IncreaseCount();
    testAggregate.DecreaseCount();

    testAggregate.GetUnsavedEvents()
      .Should()
      .BeEquivalentTo(
        new IEvent[] { new CountIncreased(1), new CountIncreased(2), new CountDecreased(1), },
        options => options.RespectingRuntimeTypes());
  }

  [Fact]
  public void
    Given_An_Aggregate__When_Calling_Methods_To_Update_The_State__Then_The_State_Should_Be_Immediately_Updated()
  {
    var testAggregate = CreateTestAggregate();

    testAggregate.CurrentCount.Should().Be(0);

    testAggregate.IncreaseCount();

    testAggregate.CurrentCount.Should().Be(1);

    testAggregate.DecreaseCount();

    testAggregate.CurrentCount.Should().Be(0);
  }

  [Fact]
  public void
    Given_An_Aggregate__When_Calling_Methods_To_Update_The_State_That_Are_Not_Valid__Then_The_Exception_Should_Propagate()
  {
    var testAggregate = CreateTestAggregate();

    testAggregate.IncreaseCount();
    testAggregate.DecreaseCount();

    var action = () => testAggregate.DecreaseCount();

    action.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void
    Given_An_Aggregate__When_Applying_Existing_Events__Then_The_State_Should_Be_Up_To_Date()
  {
    var testAggregate = CreateTestAggregate();

    var events = new IEvent[]
    {
      new CountIncreased(1), new CountIncreased(2), new CountIncreased(3), new CountDecreased(2),
    };

    foreach (var eventEnvelope in WrapEvents(events))
    {
      testAggregate.ReplayEvent(eventEnvelope);
    }

    testAggregate.CurrentCount.Should().Be(2);
  }

  [Fact]
  public void
    Given_An_Aggregate_With_Existing_Events_Applied__When_Calling_Methods_To_Update_The_State__Then_It_Should_Have_Unsaved_Events_The_Do_Not_Include_The_Applied_Events()
  {
    var testAggregate = CreateTestAggregate();

    var events = new IEvent[]
    {
      new CountIncreased(1), new CountIncreased(2), new CountIncreased(3), new CountDecreased(2),
    };

    foreach (var eventEnvelope in WrapEvents(events))
    {
      testAggregate.ReplayEvent(eventEnvelope);
    }

    testAggregate.IncreaseCount();
    testAggregate.IncreaseCount();
    testAggregate.DecreaseCount();

    testAggregate.GetUnsavedEvents()
      .Should()
      .BeEquivalentTo(
        new IEvent[] { new CountIncreased(3), new CountIncreased(4), new CountDecreased(3), },
        options => options.RespectingRuntimeTypes());
  }

  private static TestAggregate CreateTestAggregate() =>
    new()
    {
      Id = Guid.NewGuid().ToString(),
    };

  private static IEnumerable<EventEnvelope<IEvent>> WrapEvents(IEnumerable<IEvent> events)
  {
    return events.Select(
      (@event, i) =>
      {
        var streamPosition = (ulong)i;
        return new EventEnvelope<IEvent>(
          @event,
          new EventMetadata(SystemClock.Instance.GetCurrentInstant(), streamPosition, streamPosition));
      });
  }
}

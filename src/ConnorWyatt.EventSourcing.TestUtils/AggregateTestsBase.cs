using ConnorWyatt.EventSourcing.Aggregates;
using FluentAssertions;
using FluentAssertions.Execution;
using NodaTime;

namespace ConnorWyatt.EventSourcing.TestUtils;

public class AggregateTestsBase<TAggregate> where TAggregate : Aggregate, new()
{
  private readonly TAggregate _aggregate;
  private Action<TAggregate>? _whenFunc;

  public AggregateTestsBase() =>
    _aggregate = new TAggregate
    {
      Id = Guid.NewGuid().ToString(),
    };

  public void Given(Func<string, IList<EventEnvelope<IEvent>>> eventsFunc)
  {
    var eventEnvelopes = eventsFunc(_aggregate.Id);

    foreach (var @event in eventEnvelopes)
    {
      _aggregate.ReplayEvent(@event);
    }
  }

  public void When(Action<TAggregate> func)
  {
    _whenFunc = func;
  }

  public void Then(Func<string, IList<IEvent>> expectedEventsFunc)
  {
    try
    {
      _whenFunc?.Invoke(_aggregate);
    }
    catch (Exception exception)
    {
      throw new AssertionFailedException($"An exception of type \"{exception.GetType().Name}\" was thrown.");
    }

    var expectedEvents = expectedEventsFunc.Invoke(_aggregate.Id);

    var unsavedEvents = _aggregate.GetUnsavedEvents();

    unsavedEvents.Should().BeEquivalentTo(expectedEvents, options => options.RespectingRuntimeTypes());
  }

  public void ThenThrows<TException>() where TException : Exception
  {
    try
    {
      _whenFunc?.Invoke(_aggregate);
    }
    catch (Exception exception)
    {
      exception.Should().BeOfType<TException>();
      return;
    }

    throw new AssertionFailedException("No exception was thrown.");
  }

  public EventEnvelope<IEvent> Envelope(IEvent @event) =>
    new(@event, new EventMetadata(SystemClock.Instance.GetCurrentInstant(), 0, 0));
}

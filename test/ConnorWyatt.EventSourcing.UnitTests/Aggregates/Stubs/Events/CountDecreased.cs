namespace ConnorWyatt.EventSourcing.UnitTests.Aggregates.Stubs.Events;

[Event("CountDecreased")]
public class CountDecreased : IEvent
{
  public int CurrentCount { get; }

  public CountDecreased(int currentCount) => CurrentCount = currentCount;
}

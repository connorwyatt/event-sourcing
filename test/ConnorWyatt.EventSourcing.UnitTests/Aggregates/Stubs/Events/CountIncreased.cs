namespace ConnorWyatt.EventSourcing.UnitTests.Aggregates.Stubs.Events;

[Event("CountIncreased")]
public class CountIncreased : IEvent
{
  public int CurrentCount { get; }

  public CountIncreased(int currentCount) => CurrentCount = currentCount;
}

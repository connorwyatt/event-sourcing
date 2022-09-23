using ConnorWyatt.EventSourcing.Aggregates;
using ConnorWyatt.EventSourcing.UnitTests.Aggregates.Stubs.Events;

namespace ConnorWyatt.EventSourcing.UnitTests.Aggregates.Stubs;

public class TestAggregate : Aggregate
{
  public int CurrentCount { get; private set; }

  public TestAggregate()
  {
    When<CountIncreased>(Apply);
    When<CountDecreased>(Apply);
  }

  public void IncreaseCount()
  {
    var newCount = CurrentCount + 1;

    RaiseEvent(new CountIncreased(newCount));
  }

  public void DecreaseCount()
  {
    var newCount = CurrentCount - 1;

    if (newCount < 0)
    {
      throw new InvalidOperationException();
    }

    RaiseEvent(new CountDecreased(newCount));
  }

  private void Apply(CountIncreased @event)
  {
    CurrentCount = @event.CurrentCount;
  }

  private void Apply(CountDecreased @event)
  {
    CurrentCount = @event.CurrentCount;
  }
}

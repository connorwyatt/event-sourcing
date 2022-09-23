namespace ConnorWyatt.EventSourcing.Subscriptions;

public interface ISubscriber
{
  public Task HandleEvent(EventEnvelope<IEvent> eventEnvelope);
}

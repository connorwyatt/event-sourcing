using EventStore.Client;
using NodaTime;
using NodaTime.Extensions;

namespace ConnorWyatt.EventSourcing;

internal static class EventRecordExtensions
{
  public static Instant CreatedInstant(this EventRecord eventRecord) => eventRecord.Created.ToInstant();
}

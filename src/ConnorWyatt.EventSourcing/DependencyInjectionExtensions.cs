using System.Reflection;
using ConnorWyatt.EventSourcing.Aggregates;
using ConnorWyatt.EventSourcing.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace ConnorWyatt.EventSourcing;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection
    AddEventStore(
      this IServiceCollection services,
      EventStoreClientOptions eventStoreClientOptions,
      IEnumerable<Assembly> assemblies) =>
    services.AddEventStoreClient(eventStoreClientOptions.ConnectionString)
      .AddTransient<EventStoreClient>()
      .AddSingleton(_ => new EventSerializer(assemblies))
      .AddAggregateRepository()
      .AddSubscriptions();
}

using Microsoft.Extensions.DependencyInjection;

namespace ConnorWyatt.EventSourcing.Aggregates;

internal static class DependencyInjectionExtensions
{
  public static IServiceCollection AddAggregateRepository(this IServiceCollection services) =>
    services.AddTransient<AggregateRepository>();
}

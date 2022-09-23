namespace ConnorWyatt.EventSourcing.Subscriptions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SubscriberNameAttribute : Attribute
{
  public string Value { get; }

  public SubscriberNameAttribute(string value) => Value = value;
}

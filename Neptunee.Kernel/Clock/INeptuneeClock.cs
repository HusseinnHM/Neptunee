
namespace Neptunee.Clock;

public interface INeptuneeClock
{
    DateTimeOffset UtcNow { get; }
    DateTimeOffset Now { get; }

}
namespace Neptunee.Clock;

public class NeptuneeClockImp : INeptuneeClock
{
    public DateTimeOffset UtcNow { get; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Now { get; } = DateTimeOffset.Now;
}
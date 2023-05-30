using MySqlX.XDevAPI.Relational;
using NCrontab;

namespace Server;

public class CronConvertor
{
    public long CronToMiliseconds(string interval)
    {
        DateTime now = DateTime.Now;
        DateTime nextOccurrence = CrontabSchedule.Parse(interval).GetNextOccurrence(now);
        TimeSpan timeUntilNextOccurrence = nextOccurrence - now;
        return (long)timeUntilNextOccurrence.TotalMilliseconds;
    }
}

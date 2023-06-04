using Server.DatabaseTables;
using Timer = System.Timers.Timer;

namespace Server;

public class UserTimer
{
    public User User { get; set; }

    public Timer Timer { get; set; }

    public DateTime LastSend { get; set; } = DateTime.Now;
}

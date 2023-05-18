namespace Server.Dtos;

public class DaemonTimeEndStatus
{
    public short Status { get; set; }

    public DateTime Time_End { get; set; }

    public int? Bytes { get; set; }
}

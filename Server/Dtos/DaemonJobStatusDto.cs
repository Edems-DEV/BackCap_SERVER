namespace Server.Dtos;

public class DaemonJobStatusDto
{
    public short Status { get; set; }

    public DateTime? Time_start { get; set; }

    public DateTime? Time_end { get; set; }

    public int? Bytes { get; set; }
}

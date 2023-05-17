using Server.DatabaseTables;

namespace Server.ParamClasses;

public class WebLogDto
{
    public int Id { get; set; }

    public int Id_Job { get; set; }

    public string Message { get; set; }

    public DateTime Time { get; set; }

    public WebLogDto() { }

    public WebLogDto(Log log)
    {
        this.Id = log.Id;
        this.Id_Job = log.Id_Job;
        this.Message = log.Message;
        this.Time = log.Time;
    }

    public Log GetLog()
    {
        return new Log()
        {
            Id = Id,
            Id_Job = Id_Job,
            Message = Message,
            Time = Time
        };
    }
}

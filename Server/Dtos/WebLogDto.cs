using Server.DatabaseTables;

namespace Server.ParamClasses;

public class WebLogDto
{
    public int Id { get; set; }

    public int Id_Job { get; set; }

    public string Message { get; set; }

    public DateTime Time { get; set; }

    public WebLogDto() { }

    public WebLogDto(int id, int id_Job, string message, DateTime time)
    {
        this.Id = id;
        this.Id_Job = id_Job;
        this.Message = message;
        this.Time = time;
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

using Server.DatabaseTables;

namespace Server.Dtos;

public class WebJobDto
{
    public int Id { get; set; }

    public short Status { get; set; }

    public string Target { get; set; }

    public DateTime? Time_Start { get; set; }

    public DateTime? Time_End { get; set; }

    public DateTime Time_Schedule { get; set; }

    public WebOthersDto Config { get; set; }

    public WebJobDto(Job jobData, MyContext context)
    {
        this.Id = jobData.Id;
        this.Status = jobData.Status;
        this.Time_Start = jobData.Time_start;
        this.Time_End = jobData.Time_end;
        this.Time_Schedule = jobData.Time_schedule;

        if (jobData.Id_Group != null)
            Target = context.Groups.Find(jobData.Id_Group).Name;
        else if (jobData.Id_Machine != null)
            Target = context.Machine.Find(jobData.Id_Machine).Name;
        else
            throw new Exception("Invalid Data");

        Config = new WebOthersDto(jobData.Id_Config, context.Config.Find(jobData.Id_Config).Name);
    }


}

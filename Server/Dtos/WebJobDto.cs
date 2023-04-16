using Server.DatabaseTables;

namespace Server.Dtos;

public class WebJobDto
{
    public int Id { get; set; }

    public short Status { get; set; }

    public string Target { get; set; }

    public WebOthersDto Config { get; set; }

    public WebJobDto(int id, short status, int? id_Group, int? id_Machine, MyContext context)
    {
        this.Id = id;
        this.Status = status;
        Job job = context.Job.Find(id);

        if (id_Group != null)
            Target = context.Groups.Where(x => x.Id == job.Id_Group).FirstOrDefault().Name;
        else if (id_Machine != null)
            Target = context.Machine.Where(x => x.Id == job.Id_Machine).FirstOrDefault().Name;
        else
            throw new Exception("Invalid Data");

        string name = context.Config.Where(x => x.Id == job.Id_Config).FirstOrDefault().Name;
        Config = new WebOthersDto(job.Id_Config, name);
    }


}

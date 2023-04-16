using Server.DatabaseTables;

namespace Server.Dtos;

public class WebGroupDto
{
    public int Id { get; set; } 

    public string Name { get; set; }

    public string? Description { get; set; }

    public List<WebOthersDto> Configs { get; set; } = new();

    public List<WebOthersDto> Machines { get; set; } = new();

    public WebGroupDto(int id, string name, string? description, MyContext context)
    {
        Id = id;
        Name = name;
        Description = description;

        Job job = context.Job.Where(x => x.Id_Group == id).FirstOrDefault();

        if (job == null)
            return;

        foreach (var configs in context.Config.Where(x => x.Id == job.Id_Config).ToList())
        {
            Configs.Add(new WebOthersDto(configs.Id, configs.Name));
        }

        foreach (var machines in context.Machine.Where(x => x.Id == job.Id_Machine).ToList())
        {
            Machines.Add(new WebOthersDto(machines.Id, machines.Name));
        }
    }
}

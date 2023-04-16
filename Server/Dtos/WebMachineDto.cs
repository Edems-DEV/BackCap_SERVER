using Server.DatabaseTables;

namespace Server.Dtos;

public class WebMachineDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Ip_Address { get; set; }

    public bool Is_Active { get; set; }

    public List<WebOthersDto> Config { get; set; } = new();

    public List<WebOthersDto> Groups { get; set; } = new();

    public WebMachineDto()
    {
        
    }

    public WebMachineDto(Machine machine, MyContext context)
    {
        this.Name = machine.Name;
        this.Description = machine.Description;
        this.Ip_Address = machine.Ip_Address;
        this.Is_Active = machine.Is_Active;

        Job job = context.Job.Where(x => x.Id_Machine == machine.Id).FirstOrDefault();
        Config config = context.Config.Find(job.Id_Config);
        Groups group = context.Groups.Find(job.Id_Group);

        //this.Config = new WebOthersDto(config.Id, config.Name);
        //this.Groups = new WebOthersDto(group.Id, group.Name);

        foreach (var configs in context.Config.Where(x => x.Id == job.Id_Config).ToList())
        {
            Config.Add(new WebOthersDto(configs.Id, configs.Name));
        }

        foreach (var groups in context.Groups.Where(x => x.Id == job.Id_Group).ToList())
        {
            Groups.Add(new WebOthersDto(groups.Id, groups.Name));
        }
    }
}

using Server.DatabaseTables;

namespace Server.Dtos;

public class WebMachineDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Ip_Adress { get; set; }

    public bool Is_Active { get; set; }

    public WebOthersDto Config { get; set; }

    public WebOthersDto Groups { get; set; }

    public WebMachineDto()
    {
        
    }

    public WebMachineDto(Machine machine, MyContext context)
    {
        this.Name = machine.Name;
        this.Description = machine.Description;
        this.Ip_Adress = machine.Ip_Address;
        this.Is_Active = machine.Is_Active;

        Job job = context.Job.Where(x => x.Id_Machine == machine.Id).FirstOrDefault();
        Config config = context.Config.Find(job.Id_Config);
        Groups group = context.Groups.Find(job.Id_Group);

        this.Config = new WebOthersDto(config.Id, config.Name);
        this.Groups = new WebOthersDto(group.Id, group.Name);
    }
}

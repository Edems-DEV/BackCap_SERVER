using Server.DatabaseTables;
using Server.Services;

namespace Server.Dtos;

public class WebMachineDto
{
    public int Id { get; set; }

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
        this.Id = machine.Id;
        this.Name = machine.Name;
        this.Description = machine.Description;
        this.Ip_Address = machine.Ip_Address;
        this.Is_Active = machine.Is_Active;

        Job job = context.Job.Where(x => x.Id_Machine == machine.Id).FirstOrDefault();
        Config config = context.Config.Find(job.Id_Config);
        Groups group = context.Groups.Find(job.Id_Group);


        foreach (var configs in context.Config.Where(x => x.Id == job.Id_Config).ToList())
        {
            Config.Add(new WebOthersDto(configs.Id, configs.Name));
        }

        foreach (var item in context.MachineGroup.Where(x => x.Id_Machine == Id).ToList())
        {
            Groups groups = context.Groups.Find(item.Id_Group);
            Groups.Add(new WebOthersDto(groups.Id, groups.Name));
        }
    }

    public Machine UpdateMachine(Machine machine)
    {

        machine.Name = this.Name;
        machine.Description = this.Description;
        machine.Ip_Address = this.Ip_Address;
        machine.Is_Active = this.Is_Active;

        return machine;
    }

    public void DatabaseUpdate(MyContext context)
    {
        foreach (var item in context.Job.Where(x => x.Id_Machine == Id))
        {

        }
    }
}

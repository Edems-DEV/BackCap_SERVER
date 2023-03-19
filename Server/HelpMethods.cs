using Server.DatabaseTables;

namespace Server;

public class HelpMethods
{
    private MyContext context;

    public HelpMethods(MyContext context)
    {
        this.context = context;
    }

    public Job AddToJob(Job job)
    {
        job.Machine = context.Machine.Find(job.Id_Machine);
        job.Groups = context.Groups.Find(job.Id_Group);
        job.Config = context.Config.Find(job.Id_Config);
        job.Config.Sources = context.Sources.Where(x => x.Id_Config == job.Id).ToList();
        job.Config.Destinations = context.Destination.Where(x => x.Id_Config == job.Id).ToList();
        return job;
    }
}

using Server.DatabaseTables;

namespace Server.Services;

public class DatabaseManager
{
    private MyContext context;

    public DatabaseManager(MyContext context)
    {
        this.context = context;
    }

    public void AddNotExistent(Destination destination)
    {
        if (context.Destination.Where(x => x.DestPath == destination.DestPath).FirstOrDefault() == null)
        {
            context.Destination.Add(destination);
            context.SaveChanges();
        }
    }

    public void AddNotExistent(Sources source)
    {
        if (context.Sources.Where(x => x.Path == source.Path).FirstOrDefault() == null)
        {
            context.Sources.Add(source);
            context.SaveChanges();
        }
    }

    public void AddNotExistent(Groups group)
    {
        if (context.Groups.Where(x => x.Name == group.Name).FirstOrDefault() == null)
        {
            context.Groups.Add(group);
            context.SaveChanges();
        }
    }
}

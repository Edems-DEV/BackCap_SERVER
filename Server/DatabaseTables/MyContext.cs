using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System;

namespace Server.DatabaseTables;

public class MyContext : DbContext
{
    public DbSet<Users> Users { get; set; }

    public DbSet<Machine> Machine { get; set; }

    public DbSet<MachineGroup> MachineGroup { get; set; }

    public DbSet<Job> Job { get; set; }

    public DbSet<Log> Log { get; set; }

    public DbSet<Config> Config { get; set; }

    public DbSet<Sources> Sources { get; set; }

    public DbSet<Destinations> Destinations { get; set; }

    public DbSet<Group> Groups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=sibrava_db1;user=sibravaread;password=123456;SslMode=none");
    }

}

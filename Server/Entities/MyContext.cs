﻿using Microsoft.EntityFrameworkCore;

namespace Server.DatabaseTables;

public class MyContext : DbContext
{
    public DbSet<User> User { get; set; }

    public DbSet<Machine> Machine { get; set; }

    public DbSet<MachineGroup> MachineGroup { get; set; }

    public DbSet<Job> Job { get; set; }

    public DbSet<Log> Log { get; set; }

    public DbSet<Config> Config { get; set; }

    public DbSet<Sources> Sources { get; set; }

    public DbSet<Destination> Destination { get; set; }

    public DbSet<Groups> Groups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=3b2_oveckacyril_db2;user=oveckacyril;Password=123456;SslMode=none;convert zero datetime=True");
    }
}

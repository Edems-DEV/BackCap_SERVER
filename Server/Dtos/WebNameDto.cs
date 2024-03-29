﻿using Server.DatabaseTables;

namespace Server.Dtos;

public class WebNameDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public WebNameDto()
    {

    }

    public WebNameDto(int id, string? name)
    {
        this.Id = id;
        this.Name = name;
    }

    public Sources GetSources(int configId)
    {
        return new Sources() { Id = this.Id, Id_Config = configId, Path = this.Name };
    }

    public Destination GetDestination(int configId)
    {
        return new Destination() { Id = this.Id, Id_Config = configId, DestPath = this.Name };
    }
}

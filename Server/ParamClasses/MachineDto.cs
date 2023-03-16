﻿namespace Server.ParamClasses;

public class MachineDto
{
    public string? Name { get; set; } = null;

    public string? Description { get; set; } = null;

    public string? Os { get; set; } = null;

    public string? Ip_Adress { get; set; } = null;

    public string? Mac_Adress { get; set; } = null;

    public bool Is_Active { get; set; } = false;
}
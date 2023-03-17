﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MachinesController : Controller
{
    private readonly MyContext context = new MyContext();

    // GET: api/jobs?limit=25&offset=50&orderBy=id&orderDirection=desc   => UI datagrid                   
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0) //string orderBy = "id", string orderDirection = "asc"
    {
        var machine = context.Machine
        .OrderBy(p => p.Id)
        .Skip(offset)
        .Take(limit)
        .ToList();

        if (machine == null || machine.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(machine); //200
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            return Ok(context.Machine.Find(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the jobs: {ex.Message}");
        }
    }

    [HttpGet("{id}/jobs")]
    public IActionResult GetJobs(int id)
    {
        try
        {
            return Ok(context.Job.Where(x => x.id_Machine == id).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the jobs: {ex.Message}");
        }
    }
    //TODO: add by status

    // GET: for stats
    [HttpGet("count")]
    public IActionResult GetCount(bool active = true)
    {
        return Ok(context.Machine.Where(x => x.Is_active == active).Count());
    }

    [HttpPost]
    public void Post([FromBody] MachineDto machine)
    {
        Machine NewMachine = new Machine()
        {
            Name = machine.Name,
            Description = machine.Description,
            Os = machine.Os,
            Ip_address = machine.Ip_Adress,
            Mac_address = machine.Mac_Adress,
            Is_active = machine.Is_Active
        };

        context.Machine.Add(NewMachine);
        context.SaveChanges();
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Machine machine)
    {
        Machine result = context.Machine.Find(id);
        if (machine.Name != "string")                  //what is this?
            result.Name = machine.Name;
        if (machine.Description != "string")
            result.Description = machine.Description;
        if (machine.Os != "string")
            result.Os = machine.Os;
        if (machine.Ip_address != "string")
            result.Ip_address = machine.Ip_address;
        if (machine.Mac_address != "string")
            result.Mac_address = machine.Mac_address;

        result.Is_active = machine.Is_active;

        context.SaveChanges();
    }
}

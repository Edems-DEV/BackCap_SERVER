﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Validator;

namespace Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MachinesController : Controller
{
    private readonly Validators validation;
    private readonly MyContext context = new MyContext();

    public MachinesController(Validators validation)
    {
        this.validation = validation;
    }

    [HttpGet]
    public ActionResult<List<WebMachineDto>> Get()
    {
        return Ok(context.Machine.ToList().Select(x => new WebMachineDto(x, context)));
    }

    [HttpGet("names")]
    public async Task<ActionResult<List<WebMachineDto>>> GetNames()
    {
        return Ok(await context.Machine.Select(x => new WebNameDto(x.Id, x.Name)).ToListAsync());
    }

    // GET: for stats
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount(bool active = true)
    {
        return Ok(await context.Machine.CountAsync(x => x.Is_Active == active));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WebMachineDto>> Get(int id)
    {
        var machine = await context.Machine.FindAsync(id);

        if (machine == null)
            return NotFound("Object does not exists");

        return Ok(new WebMachineDto(machine, context));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] WebMachineDto machine)
    {
        try
        {
            validation.IpValidator(machine.Ip_Address.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }

        var ExistingMachine = await context.Machine.FindAsync(id);

        if (ExistingMachine == null)
            return NotFound("Object does not exists");

        ExistingMachine = machine.UpdateMachine(ExistingMachine, context);

        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> PostRegister([FromBody] MachineDto machine)
    {
        try
        {
            validation.IpValidator(machine.Ip_Address.ToString());
            validation.MacValidator(machine.Mac_Address.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }

        Machine NewMachine = machine.GetMachine();

        NewMachine.Mac_Address = NewMachine.Mac_Address.Replace("-", string.Empty);

        await context.Machine.AddAsync(NewMachine);
        await context.SaveChangesAsync();

        return Ok(NewMachine.Id);
    }

    [HttpDelete("id")]
    public async Task<ActionResult> Delete(int id)
    {
        var machine = await context.Machine.FindAsync(id);

        if (machine == null)
            return NotFound();

        context.Machine.Remove(machine);
        context.SaveChanges();

        return Ok();
    }
}

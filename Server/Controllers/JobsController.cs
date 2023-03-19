﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : Controller
{
    private readonly MyContext context = new MyContext();

    //// GET: api/jobs?limit=25&offset=50&orderBy=Id&isAscending=false   => UI datagrid
    //[HttpGet]
    //public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    //{
    //    List<Job> query;
    //    if (orderBy != null)
    //    {
    //        query = isAscending ?
    //               context.Job.OrderBy          (s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList():
    //               context.Job.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
    //        query = query
    //                .Skip(offset)
    //                .Take(limit)
    //                .ToList();
    //    }
    //    else
    //    {
    //        query = context.Job
    //            .Skip(offset)
    //            .Take(limit)
    //            .ToList();
    //    }

    //    if (query == null || query.Count == 0)
    //    {
    //        return NoContent(); //204
    //    }

    //    return Ok(query); //200
    //}

    // GET: for stats
    [HttpGet("count")]
    public ActionResult<int> GetCount()
    {
        return Ok(context.Job.Count());
    }

    // GET: api/jobs/5   => specific job info
    [HttpGet("Id/IpAddress")]
    public ActionResult<Job> Get(string IpAddress = "string", int Id = 0)
    {
        Job job;

        if (IpAddress != "string" && Id != 0)
            job = context.Job.Where(x => x.Id == Id && x.Machine.Ip_Address == IpAddress && x.Status == 0).FirstOrDefault();
        else if (IpAddress != "string")
            job = context.Job.Where(x => x.Machine.Ip_Address == IpAddress && x.Status == 0).FirstOrDefault();
        else
            job = context.Job.Find(Id);

        if (job == null)
            return NotFound("Object does not exists");

        job.Machine = context.Machine.Find(job.Id_Machine);
        job.Groups = context.Groups.Find(job.Id_Group);
        job.Config = context.Config.Find(job.Id_Config);
        job.Config.Sources = context.Sources.Where(x => x.Id_Config == job.Id).ToList();
        job.Config.Destinations = context.Destination.Where(x => x.Id_Config == job.Id).ToList();

        return Ok(job);
    }

    // for deamon for final stats after completing job
    [HttpPut("{Id}")]
    public ActionResult Put(int id, [FromBody] Job job)
    {
        Job existingJob = context.Job.Find(id);

        if (existingJob == null)
            return NotFound("Object does not exists");

        existingJob.Id_Config = job.Id_Config;
        existingJob.Id_Group = job.Id_Group;
        existingJob.Id_Machine = job.Id_Machine;
        existingJob.Status = job.Status;
        existingJob.Time_schedule = job.Time_schedule;
        existingJob.Bytes = job.Bytes;

        context.SaveChanges();
        return Ok();
    }

    // For deamon to update job Status
    [HttpPut("{Id}/StatusTime")]
    public ActionResult PutStatus(int id, [FromBody] Job job)
    {
        Job existingJob = context.Job.Find(id);

        if (existingJob == null)
            return NotFound("Object does not exists");

        existingJob.Status = job.Status;
        existingJob.Time_start = job.Time_start;
        existingJob.Time_end = job.Time_end;

        context.SaveChanges();
        return Ok();
    }
}
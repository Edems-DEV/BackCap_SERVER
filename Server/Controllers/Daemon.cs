using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using static Org.BouncyCastle.Math.EC.ECCurve;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Daemon : ControllerBase
{
    private readonly MyContext context = new MyContext();

    //--DEAMON--
    //job
    [HttpGet("Job/get/ipAddress/")]
    public Job JobGetId(string ipAddress)
    {
        return context.Job.Include(x => x.Machine).Include(x => x.Config).Where(x => x.Machine.Ip_address == ipAddress && x.status == 0).FirstOrDefault();
    }

    [HttpGet("SourcePath/get/id_config")]
    public List<Sources> SourcePathGetIdConfig(int idConfig)
    {
        List<Sources> sources = context.Sources.Where(x => x.id_Config == idConfig).ToList();
        return sources;
    }

    [HttpGet("DestPath/get/id_config")]
    public List<Destination> DestPathGetIdConfig(int idConfig)
    {
        List<Destination> destinations = context.Destination.Where(x => x.id_Config == idConfig).ToList();
        return destinations;
    }


    [HttpPut("Job/put/time_end/")]
    public void JobPutEnd_time(int jobId, JobDaemonDto job)
    {
        Job result = context.Job.Find(jobId);

        result.time_end = job.Time_End;
        result.status = job.Status;

        context.SaveChanges();
    }

    [HttpPost("Log/post/New/")]
    public void LogPostNew(LogDto log)
    {
        Log newLog = new Log()
        {
            message = log.Message,
            time = log.Time,
            id_Job = log.Id_Job
        };

        context.Log.Add(newLog);
        context.SaveChanges();
    }
}

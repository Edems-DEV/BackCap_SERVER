using System.Net;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Timer = System.Timers.Timer;

namespace Server.Services;

public class MailManager
{
    private List<UserTimer> Users = new List<UserTimer>();

    private CronConvertor cronConvertor = new CronConvertor();

    private MyContext context;

    public MailManager(MyContext context)
    {
        this.context = context;
    }

    public async Task Run()
    {
        List<User> users = await GetUsers();
        users.ForEach(x => AssingTime(new UserTimer() { User = x }));

        Console.WriteLine();
    }

    public void AddUser(User user)
    {
        AssingTime(new UserTimer() { User = user });
    }

    public void RemoveUser(User user)
    {
        StopTimer(user);
    }

    private async Task<List<User>> GetUsers()
    {
        return await context.User.ToListAsync();
    }

    private void AssingTime(UserTimer userTimer)
    {
        // delete neřešim protože každej remove userů rovnou odebere timery
        userTimer.Timer = SetTimer(cronConvertor.CronToMiliseconds(userTimer.User.Interval_Report), userTimer);
        userTimer.Timer.Start();

        var index = Users.FindIndex(x => x.User.Id == userTimer.User.Id);

        if (index == -1)
            Users.Add(userTimer);
        else
            Users[index] = userTimer;
    }

    private void StopTimer(User user)
    {
        int index = Users.FindIndex(x => x.User.Id == user.Id);
        Users[index].Timer.Stop();
        Users[index].Timer.Dispose();
        Users.RemoveAt(index);
    }

    private Timer SetTimer(long miliseconds, UserTimer userTimer)
    {
        Timer timer = new Timer()
        {
            Interval = miliseconds,
            AutoReset = false
        };

        timer.Elapsed += async (sender, e) => await SendMail(userTimer, null);

        return timer;
    }

    private async Task SendMail(object sender, EventArgs? e)
    {
        SmtpClient smtp = new SmtpClient()
        {
            Host = "localhost",
            Port = 25
        };
        var userTimer = sender as UserTimer;

        MailMessage message = new MailMessage();
        message.From = new MailAddress("Test@test.com");
        message.To.Add(new MailAddress(userTimer.User.Email));

        MyContext tempContext = new MyContext();
        DateTime LastSendWithTolerance = userTimer.LastSend.AddSeconds(-5);
        List<Log> logs = await tempContext.Log.Where(x => x.Time > LastSendWithTolerance).ToListAsync();

        message.Subject = "Reports";
        message.SubjectEncoding = Encoding.UTF8;

        try
        {
            if (logs.Count == 0)
            {
                message.Body = "Nothing to Report";
                message.BodyEncoding = Encoding.UTF8;
                await smtp.SendMailAsync(message);
                AssingTime(userTimer);
                return;
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"nepovedlo se poslat mail na {userTimer.User.Email}");
        }

        message.Body = "Here are reports, that happened after last email" + Environment.NewLine;
        foreach (var log in logs)
        {
            List<string> targets = GetMachines(log.Id_Job, tempContext);
            message.Body += log.Time.ToString();
            targets.ForEach(x => message.Body += $" {x.ToString()}");
            message.Body += $"{Environment.NewLine} {log.Message} {Environment.NewLine}";
        }

        //logs.ForEach(x => message.Body += $"{x.Time.ToString()} {targets.ForEach(x => x.ToString())} {Environment.NewLine} {x.Message} {Environment.NewLine}");
        message.BodyEncoding = Encoding.UTF8;

        try
        {
            await smtp.SendMailAsync(message);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Nepovedlo se poslat mail na {userTimer.User.Email}");
        }

        userTimer.LastSend = DateTime.Now;
        AssingTime(userTimer);



    }

    public List<string> GetMachines(int id_log, MyContext context)
    {
        List<int> jobs = context.Job.Where(x => x.Id == id_log).ToList().Select(x => x.Id).ToList();

        List<string> targers = new();

        foreach (var id in jobs)
        {
            Job job = context.Job.Find(id);
            var id_machine = job.Id_Machine;
            var id_group = job.Id_Group;

            if (id_machine != null && id_group != null)
                continue;

            if (id_machine != null)
            {
                targers.Add(context.Machine.Find(Convert.ToInt32(id_machine)).Name);
                continue;
            }

            if (id_group != null)
            {
                targers.Add(context.Groups.Find(Convert.ToInt32(id_group)).Name);
                continue;   
            }

            targers.Add(context.Machine.Find(Convert.ToInt32(id_machine)).Name);
            targers.Add(context.Groups.Find(Convert.ToInt32(id_group)).Name);
        }

        return targers;
    }

}





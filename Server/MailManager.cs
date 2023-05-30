using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using System.Net.Mail;
using System.Text;
using Timer = System.Timers.Timer;

namespace Server;

public class MailManager
{
	private Dictionary<User, Timer> Users = new Dictionary<User, Timer>();

    private CronConvertor cronConvertor = new CronConvertor();

    private MyContext context;

	public MailManager(MyContext context)
	{
		this.context = context;
	}

	public async Task Run()
	{
		AssingTimes(await GetUsers());
	}

    private async Task<List<User>> GetUsers()
    {
		return await context.User.ToListAsync();
    }

    private void AssingTimes(List<User> users)
	{

		if (users.Count == 0)
			return;
		
		Users.Keys
			.Where(x => !users.Contains(x))
			.ToList()
			.ForEach(x => StopTimer(x));

		foreach (User user in users)
		{
			if (!Users.ContainsKey(user))
			{
				Users.Add(user, SetTimer(cronConvertor.CronToMiliseconds(user.Interval_Report), user));
				Users[user].Start();
			}
		}
		
	}

	private void StopTimer(User user)
	{
        Users[user].Stop();
        Users[user].Dispose();

        Users.Remove(user);
    }

	private Timer SetTimer(long miliseconds, User user)
	{
		Timer timer = new Timer()
		{
			Interval = miliseconds,
			AutoReset = false
		};

		timer.Elapsed += async (sender, e) => await SendMail(user, null);

		return timer;
	}

	private async Task SendMail(object sender, EventArgs? e)
	{
		SmtpClient smtp = new SmtpClient();
        var user = sender as User;

        MailAddress from = new MailAddress("Test@test.test");
		MailAddress to = new MailAddress("Test2@test.test");

		MailMessage message = new MailMessage(from, to);

		DateTime timeBeforeSend = cronConvertor.GetLastOccurence(user!.Interval_Report);
		List<Log> logs = await context.Log.Where(x => x.Time > timeBeforeSend).ToListAsync();

		message.Subject = "Reports";
		message.SubjectEncoding = Encoding.UTF8;

		message.Body = "Here are reports, that happened after last email" + Environment.NewLine;
		logs.ForEach(x => message.Body += x + Environment.NewLine);
		message.BodyEncoding = Encoding.UTF8;


		await smtp.SendMailAsync(message);


		//timer reset
		Users[user!] = SetTimer(cronConvertor.CronToMiliseconds(user!.Interval_Report), user);

		//TODO doplnit o refaktor timer reset metody, aby se nemuseli používat dvě (pokud to půjde)



	}
}

using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using System.Net.Mail;
using Timer = System.Timers.Timer;

namespace Server;

public class MailManager
{
	private Dictionary<User, Timer> Users = new Dictionary<User, Timer>();

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
		CronConvertor cronConvertor = new CronConvertor();

		foreach (User user in users)
		{
			if (!Users.ContainsKey(user))
			{
				Users.Add(user, SetTimer(cronConvertor.CronToMiliseconds(user.Interval_Report)));
			}
		}
		
	}

	private Timer SetTimer(long miliseconds)
	{
		Timer timer = new Timer()
		{
			Interval = miliseconds,
			AutoReset = false
		};

		timer.Elapsed += SendMail;

		return timer;
	}

	private void SendMail(object? sender, EventArgs? e)
	{
		SmtpClient smtp = new SmtpClient();

		MailAddress from = new MailAddress("Test@test.test");
		MailAddress to = new MailAddress("Test2@test.test");
	}
}

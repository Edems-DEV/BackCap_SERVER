using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using System.Net.Mail;
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

	private async Task SendMail(object? sender, EventArgs? e)
	{
		SmtpClient smtp = new SmtpClient();

		MailAddress from = new MailAddress("Test@test.test");
		MailAddress to = new MailAddress("Test2@test.test");

		// Todo - reset time
		MailMessage message = new MailMessage(from, to);

		message.Body = "";


		await smtp.SendMailAsync(message);


		//timer reset
		var user = sender as User;
		Users[user!] = SetTimer(cronConvertor.CronToMiliseconds(user!.Interval_Report), user);



	}
}

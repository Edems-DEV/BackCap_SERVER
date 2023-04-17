using JWT.Algorithms;
using JWT.Builder;
using Server.DatabaseTables;
using Server.Entities;
using System.Net;

namespace Server.Services;

public class AuthenticationService
{
    const string SECRET = "foo-bar";

    private MyContext context = new MyContext();

    public string Authenticate(Credentials credentials)
    {
        User user = this.context.User.Where(x => x.Email == credentials.Email && x.Password == credentials.Password).FirstOrDefault();

        if (user == null)
            throw new Exception("invalid user");

        return JwtBuilder.Create()
                  .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                  .WithSecret(SECRET)
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                  .AddClaim("user_id", user.Id)
                  .Encode();
    }

    public bool VerifyToken(string token)
    {
        try
        {
            string json = JwtBuilder.Create()
                         .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                         .WithSecret(SECRET)
                         .MustVerifySignature()
                         .Decode(token);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
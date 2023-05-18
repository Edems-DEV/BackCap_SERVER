using JWT.Algorithms;
using JWT.Builder;

namespace Server.Services;

public class encryption
{
    public string StringToHesh(string inputString, string secretKey)
    {
        try
        {
            string encryptedString = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(secretKey)
            .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
            .AddClaim("unencrypted string", inputString)
            .Encode();

            return encryptedString;
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }

    }

    public string HeshToString(string encryptedString, string secretKey)
    {
        try
        {
            string decryptedString = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secretKey)
                .MustVerifySignature()
                .Decode(encryptedString);

            return decryptedString;
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }
}

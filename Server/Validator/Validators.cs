using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Server.Validator;

public class Validators
{
    //public void ValidatorConfig(string configJson)
    //{
    //    bool validInput = true;
        
    //    //v regexu nefunguje '""' - žkoušel jsem dát pryč @/přidat $ ale pak nefuguje /n (nový řádek)
    //    //=> musí ce "" přidat 
    //    // \s - prázdný znak - mezera
    //    validInput = Regex.IsMatch(configJson, @"\n\s+{\n\s+Id:\s[1-9]+,\n\s+Type:\s[1-9]+,\n\s+retencion:\s[1-9]+,\n\s+PackageSize:\s[0-9]+,\n\s+IsCompressed:\s(true|false),\n\s+backup_interval:\s[a-zA-Z0-9],\n\s+Interval_end:\s[0-9]{4}-}[0-9]{2}-[0-9A-Z]{4}:[0-9A-Z]{2}:[0-9A-Z]{2}.[0-9A-Z]{4},\n\s+sources:\s[\n\s+{\n\s+Id:\s[0-9]+,\n\s+Id_Config:\s[0-9]+,\n\s+Path:\s[a-zA-Z0-9],\n\s+config:\s[a-zA-Z0-9]\n\s+}\n\s+],\n\s+destination:\s[{Id: [0-9]+,\n\s+Id_Config:\s[0-9]+,\n\s+destPath:\s[a-zA-Z0-9],\n\s+config:\s[a-zA-Z0-9]\n\s+}\n\s+]\n\s+}");

    //    if (!validInput)
    //        throw new Exception("Invalid job input");
    //}

    ////"2023-03-18T15:53:33.548Z"
    //public void DateTimeValidator(string dateTimeStr)
    //{
    //    Regex a = new Regex(@"[0-9]{4}-[0-9]{2}-[A-Z0-9]{5}:[A-Z0-9]{2}:[A-Z0-9]{2}:[A-Z0-9]{4}");
    //    bool result  = a.IsMatch(dateTimeStr);

    //    if (!result)
    //        throw new Exception("Wrong datetime format");
    //}

    public void DateTimeValidator(string input)
    {
        Regex a = new Regex(@"[0-9]{4}-[0-9]{2}-[0-9A-Z]{5}(:[0-9A-Z]{2}){2}.[0-9A-Z]{4}");

        bool ReturnBool;
        ReturnBool = a.IsMatch(input);

        if (ReturnBool)
            throw new Exception("Invalid datetime format");
    }

    public void EmailValidator(string input)
    {
        Regex a = new Regex(@"[a-zA-Z0-9]+[@]([a-zA-Z0-9]+(\.|)+)+\.[a-zA-Z0-9]+");

        bool ReturnBool;
        ReturnBool = a.IsMatch(input);

        if (ReturnBool)
            throw new Exception("Invalid email format");
    }

    public void IpValidator(string input)
    {
        Regex a = new Regex(@"([0-9]{3}\.){2}[0-9]{3}");

        bool ReturnBool;
        ReturnBool = a.IsMatch(input);

        if (ReturnBool)
            throw new Exception("Invalid ip format");
    }

    public void MacValidator(string input)
    {
        Regex a = new Regex(@"([0-9A-F]{2}\-){5}[0-9A-F]{2}");

        bool ReturnBool;
        ReturnBool = a.IsMatch(input);

        if (ReturnBool)
            throw new Exception("Invalid mac format");
    }
}

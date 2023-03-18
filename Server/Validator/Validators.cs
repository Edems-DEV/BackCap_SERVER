using Org.BouncyCastle.Asn1.Mozilla;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Server.Validator;

public class Validators
{
    public void ValidatorConfig(string configJson)
    {
        bool validInput = true;
        
        //v regexu nefunguje '""' - žkoušel jsem dát pryč @/přidat $ ale pak nefuguje /n (nový řádek)
        //=> musí ce "" přidat 
        // \s - prázdný znak - mezera
        validInput = Regex.IsMatch(configJson, @"\n\s+{\n\s+id:\s[1-9]+,\n\s+type:\s[1-9]+,\n\s+retencion:\s[1-9]+,\n\s+packageSize:\s[0-9]+,\n\s+isCompressed:\s(true|false),\n\s+backup_interval:\s[a-zA-Z0-9],\n\s+interval_end:\s[0-9]{4}-}[0-9]{2}-[0-9A-Z]{4}:[0-9A-Z]{2}:[0-9A-Z]{2}.[0-9A-Z]{4},\n\s+sources:\s[\n\s+{\n\s+id:\s[0-9]+,\n\s+id_Config:\s[0-9]+,\n\s+path:\s[a-zA-Z0-9],\n\s+config:\s[a-zA-Z0-9]\n\s+}\n\s+],\n\s+destination:\s[{id: [0-9]+,\n\s+id_Config:\s[0-9]+,\n\s+destPath:\s[a-zA-Z0-9],\n\s+config:\s[a-zA-Z0-9]\n\s+}\n\s+]\n\s+}");

        if (!validInput)
            throw new Exception("Invalid job input");
    }

    //"2023-03-18T15:53:33.548Z"
    public void DateTimeValidator(string dateTimeStr)
    {
        Regex a = new Regex(@"[0-9]{4}-[0-9]{2}-[A-Z0-9]{5}:[A-Z0-9]{2}:[A-Z0-9]{2}:[A-Z0-9]{4}");
        bool result  = a.IsMatch(dateTimeStr);

        if (!result)
            throw new Exception("Wrong datetime format");
    }
}

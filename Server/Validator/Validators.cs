using System.Text.RegularExpressions;

namespace Server.Validator;

public class Validators
{
    public void DateTimeValidator(string input)
    {
        bool isValid = DateTime.TryParse(input, out _);

        if (!isValid)
            throw new Exception("Invalid datetime format");
    }

    public void EmailValidator(string input)
    {
        Regex a = new Regex(@"[a-zA-Z0-9]+[@]([a-zA-Z0-9]+(\.|)+)+\.[a-zA-Z0-9]+");

        bool ReturnBool;
        ReturnBool = a.IsMatch(input);

        if (!ReturnBool)
            throw new Exception("Invalid email format");
    }

    public void IpValidator(string input)
    {
        Regex ip = new Regex("^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

        bool ReturnBool;
        ReturnBool = ip.IsMatch(input);

        if (!ReturnBool)
            throw new Exception("Invalid ip format");
    }

    public void MacValidator(string input)
    {
        Regex a = new Regex(@"^([0-9A-F]{2}-){5}[0-9A-F]{2}$");

        bool ReturnBool;
        ReturnBool = a.IsMatch(input);

        if (!ReturnBool)
            throw new Exception("Invalid mac format");
    }
}

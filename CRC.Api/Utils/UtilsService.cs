using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CRC.Api.Utils;

public class UtilsService
{
    public static string QuickHash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var inputHash = SHA256.HashData(inputBytes);
        return Convert.ToHexString(inputHash);
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidCpf(string cpf)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            var cpfRegex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$");
            
            return cpf.Length == 14 && cpfRegex.IsMatch(cpf);
        }
        catch
        {
            return false;
        }
    }
    
    public static bool IsValidPassword(string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$");
            
            if (!passwordRegex.IsMatch(password)) return false;
            
            return true;
        }
        catch
        {
            return false;
        }
    }
}
﻿using System.Security.Cryptography;
using System.Text;

namespace CRC.Service;

public class HandlerHashSenha
{
    public static string QuickHash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var inputHash = SHA256.HashData(inputBytes);
        return Convert.ToHexString(inputHash);
    }
}
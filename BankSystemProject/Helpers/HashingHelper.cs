﻿using System.Security.Cryptography;
using System.Text;

namespace BankSystemProject.Helpers
{
    public static class HashingHelper
    {
        public static string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes); 
            }
        }
    }
}

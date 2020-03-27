using PeopleList.Helpers;
using System;
using System.Text.RegularExpressions;

namespace PeopleList.Models
{
    public class People
    {
        public int id { set; get; }
        public string Name { set; get; }
        public string Surname { set; get; }
        public string Email { set; get; }
        public string Birthday { set; get; }
        public string Password { set; get; }
        public string Img { set; get; }
        public Roles Role { set; get; }

        public bool CheckEdit()
        {
            return Name !="" && Surname !="" && Email!="" && Birthday!="" && IsValidEmail();
        }

        public bool CheckAdd()
        {
            return Name != "" && Surname != "" && Password != "" && Email != "" && Birthday != "" && IsValidEmail() && IsValidPassword() && !HelperConnect.FindEmail(Email);
        }

        public bool IsValidEmail()
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(Email.ToLower(), pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        public bool IsValidPassword()
        {
            if (Password.Length < 8)
            {
                return false;
            }
            bool isLower = false;
            bool isUpper = false;
            bool isDigit = false;
            bool isPunctuation = false;
            bool isWhiteSpace = false;
            foreach (char c in Password)
            {
                isLower = Char.IsLetter(c) && Char.IsLower(c) || isLower;
                isUpper = Char.IsLetter(c) && Char.IsUpper(c) || isUpper;
                isDigit = Char.IsDigit(c) || isDigit;
                isPunctuation = Char.IsPunctuation(c) || isPunctuation;
                isWhiteSpace = Char.IsWhiteSpace(c) || isWhiteSpace;
            }
            return isLower && isUpper && isDigit && isPunctuation && !isWhiteSpace;
        }

        static bool ContainsUpperLetter(string pass)
        {
            foreach (char c in pass)
            {
                if ((Char.IsLetter(c)) && (Char.IsUpper(c)))
                    return true;
            }
            return false;
        }

        static bool ContainsDigit(string pass)
        {
            foreach (char c in pass)
            {
                if (Char.IsDigit(c))
                    return true;
            }
            return false;
        }

        static bool ContainsPunctuation(string pass)
        {
            foreach (char c in pass)
            {
                if (Char.IsPunctuation(c))
                    return true;
            }
            return false;
        }

        static bool ContainsSeparator(string pass)
        {
            foreach (char c in pass)
            {
                if (Char.IsSeparator(c))
                    return true;
            }
            return false;
        }
    }
}
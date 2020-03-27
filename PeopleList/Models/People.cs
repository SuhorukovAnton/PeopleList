using System.Text.RegularExpressions;

using PeopleList.Helpers;

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
            return Name != "" && Surname != "" && Email != "" && Birthday != "" && IsValidEmail();
        }

        public bool CheckAdd()
        {
            return CheckEdit() && IsValidPassword() && !HelperConnect.FindEmail(Email);
        }

        public bool IsValidEmail()
        {
            var pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            var isMatch = Regex.Match(Email.ToLower(), pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        public bool IsValidPassword()
        {
            if (Password.Length < 8)
            {
                return false;
            }
            var isLower = false;
            var isUpper = false;
            var isDigit = false;
            var isPunctuation = false;
            var isWhiteSpace = false;
            foreach (var c in Password)
            {
                isLower = char.IsLetter(c) && char.IsLower(c) || isLower;
                isUpper = char.IsLetter(c) && char.IsUpper(c) || isUpper;
                isDigit = char.IsDigit(c) || isDigit;
                isPunctuation = char.IsPunctuation(c) || isPunctuation;
                isWhiteSpace = char.IsWhiteSpace(c) || isWhiteSpace;
            }
            return isLower && isUpper && isDigit && isPunctuation && !isWhiteSpace;
        }

        static bool ContainsUpperLetter(string pass)
        {
            foreach (var c in pass)
            {
                if ((char.IsLetter(c)) && (char.IsUpper(c)))
                    return true;
            }
            return false;
        }

        static bool ContainsDigit(string pass)
        {
            foreach (var c in pass)
            {
                if (char.IsDigit(c))
                    return true;
            }
            return false;
        }

        static bool ContainsPunctuation(string pass)
        {
            foreach (var c in pass)
            {
                if (char.IsPunctuation(c))
                    return true;
            }
            return false;
        }

        static bool ContainsSeparator(string pass)
        {
            foreach (var c in pass)
            {
                if (char.IsSeparator(c))
                    return true;
            }
            return false;
        }
    }
}
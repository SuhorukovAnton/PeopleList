using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using PeopleList.Helpers;

namespace PeopleList.Models
{
    public class FormAuth
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [ValidEmail(ErrorMessage = "Неверный формат почты")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class FormAdd
    {
        [Required]
        public string Name { set; get; }
        [Required]
        public string Surname { set; get; }
        [DataType(DataType.EmailAddress)]
        [Required]
        [ValidEmail(ErrorMessage = "Неверный формат почты")]
        [NoFindEmail(ErrorMessage = "Пользователь с таким логином уже в системе")]
        public string Email { set; get; }
        [Required]
        [DataType(DataType.Date)]
        public string Birthday { set; get; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(20)]
        [ValidPassword(ErrorMessage = "Пароль должен содержать малый и заглавный символ, цифру и знак пунктуации")]

        public string Password { set; get; }


    }
    public class FormEdit
    {
        public int Id { get; set; }

        [Required]
        public string Name { set; get; }

        [Required]
        public string Surname { set; get; }

        [DataType(DataType.EmailAddress)]
        [ValidEmail(ErrorMessage = "Неверный формат почты")]
        [Required]
        public string Email { set; get; }

        [Required]
        [DataType(DataType.Date)]
        public string Birthday { set; get; }

    }

    public class ValidPassword : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var pass = value as string;
            var isLower = false;
            var isUpper = false;
            var isDigit = false;
            var isPunctuation = false;
            var isWhiteSpace = false;
            foreach (var c in pass)
            {
                isLower = char.IsLetter(c) && char.IsLower(c) || isLower;
                isUpper = char.IsLetter(c) && char.IsUpper(c) || isUpper;
                isDigit = char.IsDigit(c) || isDigit;
                isPunctuation = char.IsPunctuation(c) || isPunctuation;
                isWhiteSpace = char.IsWhiteSpace(c) || isWhiteSpace;
            }
            return isLower && isUpper && isDigit && isPunctuation && !isWhiteSpace;
        }
    }

    public class ValidEmail : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var email = value as string;
            var pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            var isMatch = Regex.Match(email.ToLower(), pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }
    }

    public class NoFindEmail : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var email = value as string;
            return !HelperConnect.FindEmail(email);
        }
    }
}
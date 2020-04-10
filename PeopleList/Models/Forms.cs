using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using PeopleList.Helpers;

namespace PeopleList.Models
{
    public class FormAuth
    {
        [Required (ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailRequired")]
        [DataType(DataType.EmailAddress)]
        [ValidEmail(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailValid")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class FormAdd
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "NameRequired")]
        public string Name { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "SurnameRequired")]
        public string Surname { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailRequired")]
        [DataType(DataType.EmailAddress)]
        [ValidEmail(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailValid")]
        [NoFindEmail(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EmailIsBusy")]
        public string Email { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "BirthdayRequired")]
        [DataType(DataType.Date)]
        [ValidBirthday(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "BirthdayValid")]
        public string Birthday { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PasswordMin")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PasswordMax")]
        [ValidPassword(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PasswordValid")]
        public string Password { set; get; }


    }
    public class FormEdit
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "NameRequired")]
        public string Name { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "SurnameRequired")]
        public string Surname { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PasswordValid")]
        [DataType(DataType.EmailAddress)]
        [ValidEmail(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "PasswordValid")]
        public string Email { set; get; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "BirthdayRequired")]
        [DataType(DataType.Date)]
        [ValidBirthday(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "BirthdayValid")]
        public string Birthday { set; get; }

    }

    public class ValidPassword : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var pass = value as string;
            if (pass == null)
            {
                return false;
            }
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
    public class ValidBirthday : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            try
            {
                var convertedDate = Convert.ToDateTime(value);
                var nowDate = DateTime.Now;
                return convertedDate < nowDate;
            }
            catch
            {
                return false;
            }
        }
    }

    public class ValidEmail : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var email = value as string;
            if (email == null) {
                return false;
            } 
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
            if (email == null)
            {
                return false;
            }
            return !HelperConnect.FindEmail(email).Result;
        }
    }
}
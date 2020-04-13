using System.ComponentModel.DataAnnotations;
using System.Xml;

using Newtonsoft.Json;

namespace PeopleList.Models
{
    public class People
    {
        [JsonIgnore]
        public int id { set; get; }
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
        [JsonIgnore]
        public string Password { set; get; }
        [JsonIgnore]
        public string Img { set; get; }
        public Roles Role { set; get; }

        public void ReadXml(XmlReader r)
        {
            r.ReadStartElement();
            Name = r.ReadElementContentAsString(nameof(Name), "");
            Surname = r.ReadElementContentAsString(nameof(Surname), "");
            Email = r.ReadElementContentAsString(nameof(Email), "");
            Birthday = r.ReadElementContentAsString(nameof(Birthday), "");
            Role = (Roles)int.Parse(r.ReadElementContentAsString(nameof(Role), ""));
            r.ReadEndElement();
        }

        public void WriteXml(XmlWriter w)
        {
            w.WriteElementString("Name", Name);
            w.WriteElementString("Surname", Surname);
            w.WriteElementString("Email", Email);
            w.WriteElementString("Birthday", Birthday);
            w.WriteElementString("Role", ((int)Role).ToString());
        }
    }
}
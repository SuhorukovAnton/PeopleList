using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}
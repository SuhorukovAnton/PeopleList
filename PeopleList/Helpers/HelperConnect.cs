using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PeopleList.Models;

namespace PeopleList.Helpers
{
    public static class HelperConnect
    {
        public static void AddPeoples(string name, string surname)
        {
            using (PeopleContext db = new PeopleContext())
            {
                People people = new People();
                people.Name = name;
                people.Surname = surname;
                db.People.Add(people);
                db.SaveChanges();
                List<People> w = new List<People>(db.People);
            }
        }
        public static List<People> GetPeoples()
        {
            using (PeopleContext db = new PeopleContext())
            {
                return new List<People>(db.People);
            }
        }
    }
}
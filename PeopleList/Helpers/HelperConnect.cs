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
        public static void AddPeople(string name, string surname, string password, string email, string birthday)
        {
            using (PeopleContext db = new PeopleContext())
            {
                People people = new People();
                people.Name = name;
                people.Surname = surname;
                people.Email = email;
                people.Birthday = birthday;
                people.Password = password;
                db.People.Add(people);
                db.SaveChanges();
            }
        }
        public static void RemovePeople(int id)
        {
            using (PeopleContext db = new PeopleContext())
            {
                 People people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    db.People.Remove(people);
                    db.SaveChanges();
                }
            }
        }

        public static People FindUser(string email, string password)
        {
            using (PeopleContext db = new PeopleContext())
            {
                People people = db.People.FirstOrDefault(p => p.Password == password && p.Email == email);
                return people;
            }
        }

        public static List<People> GetPeoples()
        {
            using (PeopleContext db = new PeopleContext())
            {
                return new List<People>(db.People);
            }
        }
        public static People GetPeople(int id)
        {
            using (PeopleContext db = new PeopleContext())
            {
                People people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    people.Birthday = people.Birthday.Split(' ')[0];
                    string[] tmp = people.Birthday.Split('.');
                    if(tmp.Length >= 3)
                        people.Birthday = tmp[2] + "-" + tmp[1] + "-" + tmp[0];               
                    return people;
                }
                return null;
            }
        }

        public static void EditPeople(int id,string name, string surname, string email, string birthday)
        {
            using (PeopleContext db = new PeopleContext())
            {
                People people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    people.Name = name;
                    people.Surname = surname;
                    people.Email = email;
                    people.Birthday = birthday;
                    db.Entry(people).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public static void AddImg(int id, string file)
        {
            using (PeopleContext db = new PeopleContext())
            {
                People people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    people.Birthday = people.Birthday.Split(' ')[0];
                    string[] tmp = people.Birthday.Split('.');
                    if (tmp.Length >= 3)
                        people.Birthday = tmp[2] + "-" + tmp[1] + "-" + tmp[0];
                    people.Img = file;
                    db.Entry(people).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}
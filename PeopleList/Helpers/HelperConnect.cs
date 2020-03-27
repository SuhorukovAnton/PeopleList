using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PeopleList.Models;


namespace PeopleList.Helpers
{
    public static class HelperConnect
    {
        public static void AddPeople(People people)
        {
            using (PeopleContext db = new PeopleContext())
            {
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

        public static bool FindEmail(string email)
        {
            using (PeopleContext db = new PeopleContext())
            {
                People people = db.People.FirstOrDefault(p => p.Email == email);
                return people != null;
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

        public static void EditPeople(People people)
        {
            using (PeopleContext db = new PeopleContext())
            {
                People peopleLast = db.People.FirstOrDefault(p => p.id == people.id);
                if (people != null)
                {
                    peopleLast.Name = people.Name;
                    peopleLast.Surname = people.Surname;
                    peopleLast.Email = people.Email;
                    peopleLast.Birthday = people.Birthday;
                    db.Entry(peopleLast).State = EntityState.Modified;
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
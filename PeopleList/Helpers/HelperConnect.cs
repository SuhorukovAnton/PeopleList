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
            using (var db = new PeopleContext())
            {
                db.People.Add(people);
                db.SaveChanges();
            }
        }
        public static void RemovePeople(int id)
        {
            using (var db = new PeopleContext())
            {
                var people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    db.People.Remove(people);
                    db.SaveChanges();
                }
            }
        }

        public static People FindUser(string email, string password)
        {
            var passwordHash = HelperWorkWithData.GetHash(password);
            using (var db = new PeopleContext())
            {
                return db.People.FirstOrDefault(p => p.Password == passwordHash && p.Email == email);
            }
        }

        public static bool FindEmail(string email)
        {
            using (var db = new PeopleContext())
            {
                var people = db.People.FirstOrDefault(p => p.Email == email);
                return people != null;
            }
        }

        public static List<People> GetPeoples()
        {
            using (var db = new PeopleContext())
            {
                return new List<People>(db.People);
            }
        }
        public static People GetPeople(int id)
        {
            using (var db = new PeopleContext())
            {
                var people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    people.Birthday = people.Birthday.Split(' ')[0];
                    var tmp = people.Birthday.Split('.');
                    if (tmp.Length >= 3)
                        people.Birthday = tmp[2] + "-" + tmp[1] + "-" + tmp[0];
                    return people;
                }
                return null;
            }
        }

        public static void EditPeople(People people)
        {
            using (var db = new PeopleContext())
            {
                var peopleLast = db.People.FirstOrDefault(p => p.id == people.id);
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
            using (var db = new PeopleContext())
            {
                var people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    people.Birthday = people.Birthday.Split(' ')[0];
                    var tmp = people.Birthday.Split('.');
                    if (tmp.Length >= 3)
                    {
                        people.Birthday = tmp[2] + "-" + tmp[1] + "-" + tmp[0];
                    }
                    people.Img = file;
                    db.Entry(people).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}
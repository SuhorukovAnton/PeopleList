using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using PeopleList.Models;


namespace PeopleList.Helpers
{
    public static class HelperConnect
    {
        public static void AddPeople(FormAdd formAdd)
        {
            People people = new People() { Name = formAdd.Name, Surname = formAdd.Surname, Email = formAdd.Email, Password = formAdd.Password, Birthday = formAdd.Birthday, Role = Roles.User };
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

        public static void EditPeople(int id,FormEdit formEdit)
        {
            using (var db = new PeopleContext())
            {
                var people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    people.Name = formEdit.Name;
                    people.Surname = formEdit.Surname;
                    people.Email = formEdit.Email;
                    people.Birthday = formEdit.Birthday;
                    db.Entry(people).State = EntityState.Modified;
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using NLog;

using PeopleList.Models;


namespace PeopleList.Helpers
{
    public static class HelperConnect
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public async static Task AddPeople(FormAdd formAdd)
        {
            var people = new People() { Name = formAdd.Name, Surname = formAdd.Surname, Email = formAdd.Email, Password = formAdd.Password, Birthday = formAdd.Birthday, Role = Roles.User };
            try
            {
                using (var db = new PeopleContext())
                {
                    db.People.Add(people);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                logger.Info("Wrong person format: " + e.Message);
            }
        }
        public async static Task AddPeople(People people)
        {
            try
            {
                using (var db = new PeopleContext())
                {
                    if (people.Password == null)
                    {
                        people.Password = HelperWorkWithData.GetHash("123");
                    }
                    db.People.Add(people);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                logger.Info("Wrong person format: " + e.Message);
            }
        }
        public async static Task RemovePeople(int id)
        {
            using (var db = new PeopleContext())
            {
                var people = db.People.FirstOrDefault(p => p.id == id);
                if (people != null)
                {
                    db.People.Remove(people);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async static Task<People> FindUser(string email, string password)
        {
            var passwordHash = HelperWorkWithData.GetHash(password);
            using (var db = new PeopleContext())
            {
                return await db.People.FirstOrDefaultAsync(p => p.Password == passwordHash && p.Email == email);
            }
        }

        public async static Task<bool> FindEmail(string email)
        {
            using (var db = new PeopleContext())
            {
                var people = await db.People.FirstOrDefaultAsync(p => p.Email == email);
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
        public async static Task<People> GetPeople(int id)
        {
            using (var db = new PeopleContext())
            {
                var people = await db.People.FirstOrDefaultAsync(p => p.id == id);
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
        public async static Task<People> GetPeople(string email)
        {
            using (var db = new PeopleContext())
            {
                var people = await db.People.FirstOrDefaultAsync(p => p.Email == email);
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

        public async static Task EditPeople(FormEdit formEdit)
        {
            using (var db = new PeopleContext())
            {
                var people = await db.People.FirstOrDefaultAsync(p => p.id == formEdit.Id);
                if (people != null)
                {
                    people.Name = formEdit.Name;
                    people.Surname = formEdit.Surname;
                    people.Email = formEdit.Email;
                    people.Birthday = formEdit.Birthday;
                    
                    db.Entry(people).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async static Task AddImg(int id, string file)
        {
            using (var db = new PeopleContext())
            {
                var people = await db.People.FirstOrDefaultAsync(p => p.id == id);
                if (people != null)
                {
                    people.Birthday = HelperWorkWithData.TransformDate(people.Birthday);
                    people.Img = file;
                    db.Entry(people).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
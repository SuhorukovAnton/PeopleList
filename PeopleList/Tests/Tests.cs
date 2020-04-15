
using NUnit.Framework;
using PeopleList.Helpers;
using PeopleList.Models;
using System.Threading.Tasks;


namespace PeopleList.Tests
{
    public class Tests
    {

        [Test]
        public void GetPeoples()
        {
            Assert.IsTrue(HelperConnect.GetPeoples().Count > 0);
        }

        

        [Test]
        public async Task AddPeople()
        {
            var people = new People()
            {
                Name = "Anton",
                Surname = "Sukhorukov",
                Email = "Test@test.com",
                Birthday = "1996-11-23",
                Role = Roles.SuperAdmin
            };
            await HelperConnect.AddPeople(people);
            Assert.IsTrue(await HelperConnect.FindEmail(people.Email));
        }
        [Test]
        public async Task GetPeopleSucc()
        {
            Assert.IsNotNull(await HelperConnect.GetPeople("Test@test.com"));
        }
        [Test]
        public async Task EditPeople()
        {
            var people = await HelperConnect.GetPeople("Test@test.com");
            var formPeople = new FormEdit() {
                Id = people.id,
                Email = people.Email,
                Birthday = people.Birthday,
                Name = "NewName",
                Surname = people.Surname
            };


            await HelperConnect.EditPeople(formPeople);
            people = await HelperConnect.GetPeople("Test@test.com");

            Assert.AreEqual(people.Name, formPeople.Name);
        }

        [Test]
        public async Task RemovePeople()
        {
            var people = await HelperConnect.GetPeople("Test@test.com");

            await HelperConnect.RemovePeople(people.id);

            Assert.IsFalse(await HelperConnect.FindEmail(people.Email));
        }
        public async Task GetPeopleFail()
        {
            Assert.IsNull(await HelperConnect.GetPeople("Test@test.com"));
        }
    }
}
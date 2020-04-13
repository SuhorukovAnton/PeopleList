using System.Threading.Tasks;
using System.Web;

namespace PeopleList.Core
{
    interface IReader
    {
        Task AddPeople(string path);
        void Create(HttpServerUtilityBase Server);
        void Unload(HttpResponseBase Response, HttpServerUtilityBase Server);
    }
}

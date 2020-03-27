using System.Data.Entity;

using MySql.Data.EntityFramework;

namespace PeopleList.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class PeopleContext : DbContext
    {
        public DbSet<People> People { get; set; }
    }
}
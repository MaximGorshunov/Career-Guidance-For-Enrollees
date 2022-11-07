using System.Collections.Generic;

namespace Entities
{
    public class University : IEntitiy
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Contacts { get; set; }
        public string Info { get; set; }

        public User User { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}

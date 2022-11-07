using System.Collections.Generic;

namespace Entities
{
    public class Department : IEntitiy
    {
        public int Id { get; set; }
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string Contacts { get; set; }
        public string Info { get; set; }

        public University University { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}

using System.Collections.Generic;

namespace Entities
{
    public class Profession : IEntitiy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProfType ProfType { get; set; }

        public ICollection<Question> QuestionFirst { get; set; }
        public ICollection<Question> QuestionSecond { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}

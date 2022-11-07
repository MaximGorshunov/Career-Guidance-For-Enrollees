using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ProfessionCourse : IEntitiy
    {
        [NotMapped]
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public ProfessionCourse() { }

        public ProfessionCourse(Course course, Profession profession)
        {
            Course = course;
            CourseId = course.Id;

            Profession = profession;
            ProfessionId = profession.Id;
        }
    }
}

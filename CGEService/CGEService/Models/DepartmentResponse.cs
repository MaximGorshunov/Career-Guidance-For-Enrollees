using System.Collections.Generic;

namespace CGEService.Models
{
    public class DepartmentResponse
    {
        /// <summary>
        /// Identifier of the department
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of a university to which department belongs
        /// </summary>
        public int UniversityId { get; set; }

        /// <summary>
        /// Department name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Department contact info in free form
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// Department description
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// List of all courses of the department
        /// </summary>
        public List<CourseResponse> Courses { get; set; }

        /// <summary>
        /// University to which department belongs
        /// </summary>
        public UniversityResponse University { get; set; }

        public DepartmentResponse() { }

        public DepartmentResponse( int id, int universityId, string name, string contacts, string info)
        {
            Id = id;
            UniversityId = universityId;
            Name = name;
            Contacts = contacts;
            Info = info;
        }
    }
}

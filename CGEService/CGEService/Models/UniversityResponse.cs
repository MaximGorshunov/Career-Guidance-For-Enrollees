using System.Collections.Generic;

namespace CGEService.Models
{
    public class UniversityResponse
    {
        /// <summary>
        /// Identifier of the university
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// University name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// University contact info in free form
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// University description
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// List of university's departments
        /// </summary>
        public List<DepartmentResponse> Departments { get; set; }

        public UniversityResponse() { }
        
        public UniversityResponse(int id, string name, string contacts, string info)
        {
            Id = id;
            Name = name;
            Contacts = contacts;
            Info = info;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for creating department
    /// </summary>
    public class DepartmentCreate
    {
        /// <summary>
        /// Id of university
        /// </summary>
        [Required]
        public int UniversityId { get; set; }

        /// <summary>
        /// Name of university
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Univertsity's contacts
        /// </summary>
        [Required]
        public string Contacts { get; set; }

        /// <summary>
        /// University's info
        /// </summary>
        [Required]
        public string Info { get; set; }
    }
}

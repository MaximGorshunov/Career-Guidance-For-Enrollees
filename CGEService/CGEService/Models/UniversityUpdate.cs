using System.ComponentModel.DataAnnotations;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for update university
    /// </summary>
    public class UniversityUpdate
    {
        /// <summary>
        /// University id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Name of university
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Univertsity's contacts
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// University's info
        /// </summary>
        public string Info { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for create unoversity
    /// </summary>
    public class UniversityCreate
    {
        /// <summary>
        /// id of manager
        /// if user in role manager send null
        /// </summary>
        [Required]
        public int? UserId { get; set; }

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

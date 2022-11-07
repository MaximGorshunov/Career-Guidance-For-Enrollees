using System;
using System.ComponentModel.DataAnnotations;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for updating user
    /// </summary>
    public class UserUpdate
    {
        /// <summary>
        /// User's identity key
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// User's first name
        /// </summary>
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        /// <summary>
        /// user's second name
        /// </summary>
        [Required]
        [StringLength(20)]
        public string SecondName { get; set; }
        /// <summary>
        /// User's login name
        /// </summary>
        [StringLength(20, MinimumLength = 2)]
        public string Login { get; set; }
        /// <summary>
        /// User's email
        /// </summary>
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [StringLength(60, MinimumLength = 6)]
        public string Email { get; set; }
        /// <summary>
        /// User's birthdate
        /// </summary>
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// Defines is user a male or female
        /// </summary>
        public bool? IsMan { get; set; }
        /// <summary>
        /// User's password
        /// </summary>
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; }
    }
}

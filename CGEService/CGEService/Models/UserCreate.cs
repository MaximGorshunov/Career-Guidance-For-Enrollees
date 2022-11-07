using System;
using System.ComponentModel.DataAnnotations;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for creating user
    /// </summary>
    public class UserCreate
    {
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
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Login { get; set; }

        /// <summary>
        /// User's email
        /// </summary>
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [StringLength(60, MinimumLength = 6)]
        public string Email { get; set; }
        
        /// <summary>
        /// User's birthdate
        /// </summary>
        [Required]
        public DateTime Birthdate { get; set; }
        
        /// <summary>
        /// Defines is user a male or female
        /// </summary>
        [Required]
        public bool IsMan { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; }

        /// <summary>
        /// User's role
        /// admin, user, manager
        /// </summary>
        [Required]
        public string Role { get; set; }
    }
}

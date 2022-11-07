using System;

namespace CGEService.Models
{
    public class UserResponse
    {
        /// <summary>
        /// Identifier of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Second name of the user
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Account name of the user
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Registered email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's birthdate
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Indicates that user is man (true) or woman (false)
        /// </summary>
        public bool IsMan { get; set; }

        /// <summary>
        /// Indicates user permissions in the system
        /// </summary>
        public string Role { get; set; }
    }
}

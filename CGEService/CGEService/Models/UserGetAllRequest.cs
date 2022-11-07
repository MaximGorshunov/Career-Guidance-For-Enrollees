using Services.Enums;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for getting list of users
    /// </summary>
    public class UserGetAllRequest
    {
        /// <summary>
        /// Filter by user's first name
        /// </summary>
        public string FirstNameFilter { get; set; }
        /// <summary>
        /// Filter by user's second name
        /// </summary>
        public string SecondNameFilter { get; set; }
        /// <summary>
        /// Male = 1
        /// Female = 2
        /// None = Null
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// Filter by user's login
        /// </summary>
        public string LoginFilter { get; set; }
        /// <summary>
        /// Filter by user's email
        /// </summary>
        public string EmailFilter { get; set; }
        /// <summary>
        /// Filter by user's min age
        /// </summary>
        public int? AgeMin { get; set; }
        /// <summary>
        /// Filter by user's max age
        /// </summary>
        public int? AgeMax { get; set; }
        /// <summary>
        /// user
        /// admin
        /// manager
        /// none - null
        /// </summary>
        public string Role { get; set; }
    }
}

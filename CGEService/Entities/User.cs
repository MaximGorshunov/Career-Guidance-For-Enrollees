using System;
using System.Collections.Generic;

namespace Entities
{
    public class User : IEntitiy
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirtstName { get; set; }
        public string SecondName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsMan { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
        public University University { get; set; }
        public ICollection<UserResult> UserResults { get; set; }
    }
}

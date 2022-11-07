using Entities;
using System;

namespace CGEService.Models
{
    public class AuthenticateResponse
    {
        /// <summary>
        /// Identifier of the authenticated user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of the authenticated user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Second name of the authenticated user
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Account name of the authenticated user
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Registered email of the authenticated user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Authenticated user's birthdate
        /// </summary>
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Indicates that authenticated user is man (true) or woman (false)
        /// </summary>
        public bool IsMan { get; set; }

        /// <summary>
        /// Authenticated user's role identifier
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Authenticated user's role name
        /// </summary>
        public string RoleName { get; set; } 

        /// <summary>
        /// Bearer token that should be used for authorization
        /// </summary>
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.FirtstName;
            SecondName = user.SecondName;
            Login = user.Login;
            Email = user.Email;
            Birthdate = user.Birthdate;
            IsMan = user.IsMan;
            Token = token;
            RoleId = user.RoleId;
            RoleName = user.Role.Name;
        }
    }
}

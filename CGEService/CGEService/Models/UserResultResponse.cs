using CGEService.Helpers;
using System;
using System.Collections.Generic;

namespace CGEService.Models
{
    public class UserResultResponse
    {
        /// <summary>
        /// Identifier of saved result (could be null for the results of anonymous user)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// User who completed the survey (null if anonymous)
        /// </summary>
        public UserResponse User { get; set; }

        /// <summary>
        /// Date of the survey
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Survey results
        /// </summary>
        public List<PType> Results { get; set; }

        /// <summary>
        /// Preferable professional type
        /// </summary>
        public ProfessionalTypeResponse ProfessionalType { get; set; }

        /// <summary>
        /// List of appropriate professions
        /// </summary>
        public List<ProfessionResponse> Professions { get; set; }
    }
}

using System.Collections.Generic;

namespace CGEService.Models
{
    public class QuestionResponse
    {
        /// <summary>
        /// Identifier of the question
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Order number of the question
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Response options for the question
        /// </summary>
        public ICollection<ProfessionResponse> Professions { get; set; }
    }
}
